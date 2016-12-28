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
#include <time.h>
#include<iostream>

#include "zmq.hpp"


int timepass(int tmes) {
	int i = 0;
	while (i++ < tmes) {
		std::cout << "This is me again" << std::endl;
		Sleep(2);
	}
	return tmes;
}

#define srandom srand
#define random rand

//  Provide random number from 0..(num-1)
#define randof(num)  (int) ((float) (num) * random () / (RAND_MAX + 1.0))

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

int main(int argc, char *argv[])
{
	//  Socket to talk to server
	int loopCount = 0;
	int client_no = 0;
	std::cout<<"Collecting updates from Publisher for following symbols VGH, XFI, XIBX, XID and XII\n"<<std::endl;
	void *context = zmq_ctx_new();
	void *subscriber = zmq_socket(context, ZMQ_SUB);
	int rc = zmq_connect(subscriber, "tcp://127.0.0.1:5551");
	assert(rc == 0);
	char *filter = NULL;
	if (argc > 1) {
		filter = _strdup(argv[1]);
		//client_no = atoi(argv[1]);
	}
	//  Subscribe to zipcode, default is NYC, 10001
	//char *filter = client_no;
	if (filter != NULL) {
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, filter, strlen(filter));
	}
	else {
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "VGH.IDX", 7);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "XFI.IDX", 7);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "XIBX.IDX", 8);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "XID.IDX", 7);
		rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, "XII.IDX", 7);
	}
	//rc = zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, filter, 0);
	assert(rc == 0);
	//  Process 100 updates
	int update_nbr;
	long total_temp = 0;
	while (loopCount++ < 100) {
		for (update_nbr = 0; update_nbr < 10; update_nbr++) {
			char *string = s_recv(subscriber);
			int zipcode, temperature, relhumidity;
			std::cout << "Reading : ["<<update_nbr<<"] [" << string << "]" << std::endl;
			//sscanf_s(string, "%d %d %d", &zipcode, &temperature, &relhumidity);
			//total_temp += temperature;
			free(string);
		}
		//printf("[Client: %d] Last 10 reading for zipcode '%s' was %dF\n", client_no, filter, (int)(total_temp / update_nbr));
		total_temp = 0;
	}
	zmq_close(subscriber);
	zmq_ctx_destroy(context);
	std::cout << "Exit !!" << std::endl;
	Sleep(10000);
    return 0;
}