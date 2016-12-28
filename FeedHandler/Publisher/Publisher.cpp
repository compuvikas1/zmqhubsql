// Publisher.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <assert.h>
#include <signal.h>
#include <stdarg.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include <iostream>
#include <fstream>
#include <sstream> 
#include "zmq.hpp"

#define _CRTDBG_MAP_ALLOC
#include <crtdbg.h>
#include "Publisher.h"
#include "JSONValue.h"

#define srandom srand
#define random rand

//typedef void(*sig_t)(int);

//  Provide random number from 0..(num-1)
#define randof(num)  (int) ((float) (num) * random () / (RAND_MAX + 1.0))

int gotSignalToStop;

//  Receive 0MQ string from socket and convert into C string
//  Caller must free returned string. Returns NULL if the context
//  is being terminated.
static char * s_recv(void *socket) {
	char buffer[256];
	int size = zmq_recv(socket, buffer, 255, 0);
	if (size == -1)
		return NULL;
	//return strndup(buffer, sizeof(buffer) - 1);
	return _strdup(buffer);
	// remember that the strdup family of functions use malloc/alloc for space for the new string.  It must be manually
	// freed when you are done with it.  Failure to do so will allow a heap attack.
}

//  Convert C string to 0MQ string and send to socket
static int s_send(void *socket, char *string) {
	int size = zmq_send(socket, string, strlen(string), 0);
	return size;
}

//  Sends string as 0MQ string, as multipart non-terminal
static int s_sendmore(void *socket, char *string) {
	int size = zmq_send(socket, string, strlen(string), ZMQ_SNDMORE);
	return size;
}

//  Receives all message parts from socket, prints neatly
//
static void s_dump(void *socket)
{
	int rc;

	zmq_msg_t message;
	rc = zmq_msg_init(&message);
	assert(rc == 0);

	puts("----------------------------------------");
	//  Process all parts of the message
	do {
		int size = zmq_msg_recv(&message, socket, 0);
		assert(size >= 0);

		//  Dump the message as text or binary
		char *data = (char*)zmq_msg_data(&message);
		assert(data != 0);
		int is_text = 1;
		int char_nbr;
		for (char_nbr = 0; char_nbr < size; char_nbr++) {
			if ((unsigned char)data[char_nbr] < 32
				|| (unsigned char)data[char_nbr] > 126) {
				is_text = 0;
			}
		}

		printf("[%03d] ", size);
		for (char_nbr = 0; char_nbr < size; char_nbr++) {
			if (is_text) {
				printf("%c", data[char_nbr]);
			}
			else {
				printf("%02X", (unsigned char)data[char_nbr]);
			}
		}
		printf("\n");
	} while (zmq_msg_more(&message));

	rc = zmq_msg_close(&message);
	assert(rc == 0);
}

static void s_set_id(void *socket, intptr_t id)
{
	char identity[10];
	sprintf_s(identity, "%04X", (int)id);
	zmq_setsockopt(socket, ZMQ_IDENTITY, identity, strlen(identity));
}

//  Sleep for a number of milliseconds
static void s_sleep(int msecs)
{
	Sleep(msecs);
}

//  Return current system clock as milliseconds
static int64_t s_clock(void)
{
	SYSTEMTIME st;
	GetSystemTime(&st);
	return (int64_t)st.wSecond * 1000 + st.wMilliseconds;
}

void my_handler(int s) {
	gotSignalToStop = 1;
	printf("Caught signal %d\n", s);
}

int readFile(std::string fileName, std::string &out) {
	std::ifstream inFile;
	inFile.open(fileName);//open the input file

	std::stringstream oss;
	oss << inFile.rdbuf();//read the file
	out = oss.str();
	return 0;
}

int OSIParser(std::string input, std::string &symbol, std::string &date, std::string &calllPut , std::string &strike) {
	symbol = input.substr(0, 6);
	date = input.substr(7,12);
	calllPut = input.substr(12, 13);
	strike = input.substr(13, 21);
	return 0;
}

int getLocalTime(long input, std::string &out) {
	time_t tmx = input;
	struct tm tm;
	localtime_s(&tm,&tmx);
	//std::cout << "Coming here ["<<error_local<<"] ["<<input<<"] ["<<tmx<<"]" << std::endl;
	char date[20];
	strftime(date, sizeof(date), "%Y-%m-%d %H:%M:%S", &tm);
	out = date;
	return 0;
}

int processSymbol(std::string sym, std::string value, int localcnt, std::string &outString) {
	//std::cout << "Data for sym [" << sym << "]" << std::endl;
	JSONValue *jsonValue = JSON::Parse(value.c_str());
	double iterator = 0;
	std::string symbol = "";
	if (!jsonValue) {
		std::cout << "No content from json" << std::endl;
		return -1;
	}
	else {
		JSONObject root;
		std::string OSI;
		std::string strike;
		std::string callPut;
		std::string date;
		std::string symbol;

		if (jsonValue->IsObject() == false)
		{
			std::cout << "The root element is not an object, did you change the example?" << std::endl;
			return -1;
		}
		else
		{
			//std::cout << "Symbol, TimeStamp, Expiry, Call/Put, strike, BidSize, BidPrice, AskPrice, AskSize, Volume" << std::endl;
			root = jsonValue->AsObject();
			if (root.find(L"OSI") != root.end() && root[L"OSI"]->IsString())
			{
				std::wstring OSIstr(root[L"OSI"]->AsString());
				std::string input(OSIstr.begin(), OSIstr.end());
				symbol = input.substr(0, 5);
				date = input.substr(6, 11);
				callPut = input.substr(12, 13);
				strike = input.substr(13, 20); //TODO: divide by 1000					
			}
			if (root.find(L"dim") != root.end() && root[L"dim"]->IsNumber())
			{
				iterator = root[L"dim"]->AsNumber();
			}
			for (int i = 0; i < iterator; i++) {
				std::string s = "quote" + std::to_string(i);
				std::wstring quote(s.begin(), s.end());
				std::string ltime;
				JSONObject quot = root[quote]->AsObject();
				if (quot[L"isNBBO"]->AsNumber() == 1) {
					getLocalTime((long)quot[L"time"]->AsNumber(), ltime);
					double lstrike = std::stod(strike);
					std::string data = symbol;
					data += "," + ltime; // std::to_string(quot[L"time"]->AsNumber());
					data += ",2016-09-30, " + callPut + " , " + std::to_string(lstrike / 1000) + " , " + std::to_string(quot[L"Bsz"]->AsNumber());
					data += ", " + std::to_string(quot[L"Bpx"]->AsNumber());
					data += ", " + std::to_string(quot[L"Asz"]->AsNumber());
					data += ", " + std::to_string(quot[L"Apx"]->AsNumber());
					data += ", " + std::to_string(localcnt);
					std::cout << data << std::endl;
					outString = data;
					return 0;
				}
			}
		}
	}
	return -1;
}

int main(int argc , char *argv[])
{
	signal(SIGINT, my_handler);
	char errorStr[1000] = { '\0' };
	int localcnt = 1001;
	std::string lfilePath = "C:\\Users\\amodus\\Desktop\\FeedHandler\\";
	//std::string lfilePath = "D:\\Rahul\\zmqhub\\FeedHandler\\resources\\";
	std::string lfiles;
	if (argc > 2) {
		lfilePath = argv[1];
		lfiles = argv[2];
	} else{
		lfiles = "SPY,GLD,DLD,CET";
	}
	void *context = zmq_ctx_new();
	void *publisher = zmq_socket(context, ZMQ_PUB);
	int rc = zmq_bind(publisher, "tcp://127.0.0.1:5551");
	if (rc != 0) {
		strerror_s(errorStr, errno);
		std::cout << errno << " [" << errorStr << "] " << std::endl;
		exit(0);
	}
	//assert(rc == 0);
	gotSignalToStop = 0;
	srandom((unsigned)time(NULL));

	int ierror = -1;
	std::string value;
	std::string delim = ",";
	std::vector<std::string> tokens;
	std::vector<std::string> contents;
	size_t prev = 0, pos = 0;
	do
	{
		pos = lfiles.find(delim, prev);
		if (pos == std::string::npos) pos = lfiles.length();
		std::string token = lfiles.substr(prev, pos - prev);
		if (!token.empty()) tokens.push_back(token);
		prev = pos + delim.length();
	} while (pos < lfiles.length() && prev < lfiles.length());

	std::cout <<"Vector size : "<< tokens.size() << std::endl;

	for (std::vector<std::string>::iterator it = tokens.begin(); it != tokens.end(); ++it) {
		std::string lfileName = lfilePath+"\\"+*it+".json";
		std::cout << "Read the file [" << lfileName << "]" << std::endl;
		ierror = readFile(lfileName, value);
		if (ierror != 0) {			
			std::cout << "Unable to read the file [" << lfileName << "]" << std::endl;
			continue;
		}
		else {
			contents.push_back(value);
		}
	}

	//std::cout << "Vector size : " << contents.size() << std::endl;

	while(1) {
		std::string outString;
		int i = 0;
		for (std::vector<std::string>::iterator it = contents.begin(); it != contents.end(); ++it) {
			std::cout << "Processing [" << tokens.at(i) << "]" << std::endl;
			processSymbol(tokens.at(i++),*it, localcnt++, outString);
			s_send(publisher, (char *)outString.c_str());
		}
		Sleep(5000);
	}
	zmq_close(publisher);
	zmq_ctx_destroy(context);
	_CrtDumpMemoryLeaks();
	Sleep(10000);
    return 0;
}