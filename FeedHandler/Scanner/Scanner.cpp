// Scanner.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <assert.h>
#include <signal.h>
#include <stdarg.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/timeb.h>
#include<iostream>
#include <fstream>
#include <sstream> 
#include <chrono>

//using namespace std::chrono;


#include "zmq.hpp"
#include "logger.h"

std::string component;
LOGLEVEL level;
std::string logServerStr;

#define srandom srand
#define random rand

//  Provide random number from 0..(num-1)
#define randof(num)  (int) ((float) (num) * random () / (RAND_MAX + 1.0))

static char * s_recv(void *socket) {
	char buffer[256];
	int size = zmq_recv(socket, buffer, 255, 0);
	if (size == -1)
		return NULL;
	return _strdup(buffer);
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

void getCurrentTime(std::string &currTime) {
	char time1[24] = {'\0'};
	std::chrono::system_clock::time_point now = std::chrono::system_clock::now();
	std::chrono::system_clock::duration tp = now.time_since_epoch();
	tp -= std::chrono::duration_cast<std::chrono::seconds>(tp);
	time_t tt = std::chrono::system_clock::to_time_t(now);
	struct tm tm;
	localtime_s(&tm, &tt);
	std::snprintf(time1,24,"[%04u-%02u-%02u %02u:%02u:%02u.%03u]",tm.tm_year + 1900,tm.tm_mon + 1, tm.tm_mday, tm.tm_hour, tm.tm_min, tm.tm_sec,
	static_cast<unsigned>(tp/std::chrono::milliseconds(1)));
	currTime = time1;
}

int readFile(std::string fileName, std::string &out) {
	std::ifstream inFile;
	try {
		std::cout << "Going to read " << fileName << std::endl;
		inFile.open(fileName);//open the input file
		std::stringstream oss;
		oss << inFile.rdbuf();//read the file
		out = oss.str();
		return 0;		
	} catch (std::system_error e){	
		std::cerr << "Unable to open " + fileName + " due to " + e.code().message()  << std::endl;
		return -1;
	}	
}

LOGLEVEL getLogLevel(std::string ll) {
	if (ll == "INFO") return INFO;
	if (ll == "ERR") return ERR;
	if (ll == "WAR") return WAR;
	if (ll == "DEBUG") return DEBUG;
	return INFO;
}

void readConfig(std::string configFile){
	size_t prev = 0, pos = 0;
	std::string confdata;
	std::string delim("\n");
	TCHAR pwd[MAX_PATH];
	GetCurrentDirectory(MAX_PATH, pwd);
	std::cout << "Going to read local config " << configFile << std::endl;
	if (readFile(configFile, confdata) != 0) {
		std::cerr << "Unable to read the config. going with default settings" << std::endl;
		return;
	}
	std::cout << "Able to read local config : " << configFile << "with size "<<confdata.size()<< std::endl;
	if (confdata.size() < 10) {
		std::cerr << "Very less bytes in config, So ignoring it" << std::endl;
		return;
	}
	do
	{
		pos = confdata.find(delim, prev);
		if (pos == std::string::npos) pos = confdata.length();
		std::string token = confdata.substr(prev, pos - prev);
		if (!token.empty()) {
			if (token[0] == '#') {
				prev = pos + delim.length();
				continue;
			}
			size_t p1;
			if (token.substr(0, 8) == "LOGLEVEL") {
				p1 = token.find("=", 0); p1++;
				level = getLogLevel(token.substr(p1, token.length()));
			}
			if (token.substr(0, 9) == "LOGSERVER") {
				p1 = token.find("=", 0); p1++;
				logServerStr = token.substr(p1, token.length());
			}
			if (token.substr(0, 9) == "COMPONENT") {
				p1 = token.find("=", 0); p1++;
				component = token.substr(p1, token.length());
			}
			if (token.substr(0, 12) == "COMPONENTLOG") {
				p1 = token.find("=", 0); p1++;
				component = (LOGLEVEL)std::stoi(token.substr(p1, token.length()));
			}
		}
		prev = pos + delim.length();
	} while (pos < confdata.length() && prev < confdata.length());
}

int zmqLog(LOGLEVEL loglevel, std::string comp , std::string data) {
	std::cout << __LINE__ << " " << logServerStr << " "<<loglevel<<" : Living : " << comp << " : " << data<< std::endl;
	if (loglevel >= level) {
		std::string  timestamp; //YYYY-MM-DD HH:MM:SS.xxxx
		getCurrentTime(timestamp);
		std::string logStatement = timestamp + " " + std::to_string(loglevel)+" "+component+" : "+data;
		zmq::context_t context(1);
		zmq::socket_t socket(context, ZMQ_PUSH);
		socket.connect(logServerStr);
		zmq::message_t request(logStatement.length());
		memcpy((void *)request.data(), logStatement.c_str(), logStatement.length());
		socket.send(request);
		return 0;
	}	else {
		return 1;
	}
}

int main(int argc, char *argv[])
{
	//  Socket to talk to server
	int loopCount = 0;
	int client_no = 0;
	char *filter = NULL;
	std::string configPath;
	if (argc > 2) {
		filter = _strdup(argv[2]);
		configPath = argv[1];
	}
	else {
		std::cerr << "Usage ./" << argv[0] << " <config path> <symbol> " << std::endl;
		exit(0);
	}
	component = "SCANNER";
	readConfig(configPath);
	
	void *context = zmq_ctx_new();
	void *subscriber = zmq_socket(context, ZMQ_SUB);
	int rc = zmq_connect(subscriber, "tcp://127.0.0.1:5551");
	assert(rc == 0);
	std::cout << __LINE__ << " DBG : Living : " << std::endl;
	
	
	if (filter != NULL) {
		if (filter == "ALL") {
			rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "", 0);
			assert(rc == 0);
		}
		else {
			rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, filter, strlen(filter));
		}
	}
	else {
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "NIFTY", 5);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "BANKINDIA", 9);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "LICHSGFIN", 9);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "BHARATFORG", 10);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "CADILAHC", 8);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "BANKNIFTY", 9);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "WOCKPHARMA", 10);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "VGH.IDX", 7);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "XFI.IDX", 7);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "XIBX.IDX", 8);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "XID.IDX", 7);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "XII.IDX", 7);
	}
	//assert(rc == 0);

	zmqLog(INFO, "Scanner", "Collecting updates from Publisher for following symbols VGH, XFI, XIBX, XID and XII");
	//std::cout << "Collecting updates from Publisher for following symbols VGH, XFI, XIBX, XID and XII\n" << std::endl;

	int update_nbr;
	while (loopCount++ < 100) {
		for (update_nbr = 0; update_nbr < 10; update_nbr++) {
			char *string = s_recv(subscriber);
			string[strlen(string) - 1] = '\0';
			zmqLog(INFO,"Vikas", "Reading : [" + std::to_string(update_nbr) + "] [" + string + "]");
			//std::cout << "Reading : ["<<update_nbr<<"] [" << string << "]" << std::endl;
			free(string);
		}
	}
	zmq_close(subscriber);
	zmq_ctx_destroy(context);
	std::cout << "Exit !!" << std::endl;
	Sleep(10000);
    return 0;
}
