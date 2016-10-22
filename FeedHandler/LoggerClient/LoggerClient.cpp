// LoggerClient.cpp : Defines the entry point for the console application.
//



#include "stdafx.h"
#include "zmq.hpp"

#include <iostream>
#include <sys/timeb.h>
#include <chrono>

void getCurrentTime(std::string &currTime) {
	char time1[24] = { '\0' };
	std::chrono::system_clock::time_point now = std::chrono::system_clock::now();
	std::chrono::system_clock::duration tp = now.time_since_epoch();
	tp -= std::chrono::duration_cast<std::chrono::seconds>(tp);
	time_t tt = std::chrono::system_clock::to_time_t(now);
	struct tm tm;
	localtime_s(&tm, &tt);
	std::snprintf(time1, 24, "[%04u-%02u-%02u %02u:%02u:%02u.%03u]", tm.tm_year + 1900, tm.tm_mon + 1, tm.tm_mday, tm.tm_hour, tm.tm_min, tm.tm_sec,
		static_cast<unsigned>(tp / std::chrono::milliseconds(1)));
	currTime = time1;
}

int main()
{
	std::string  timestamp; //YYYY-MM-DD HH:MM:SS.xxxx
	std::string logServerStr = "tcp://127.0.0.1:5558";
	zmq::context_t context(1);
	zmq::socket_t socket(context, ZMQ_PUSH);
	socket.connect(logServerStr);

	while (1) {
		getCurrentTime(timestamp);
		std::string logStatement = timestamp + " INFO LoggerClient : Testing";
		std::cout << "Sending to "<< logServerStr <<" : " << logStatement << std::endl;
		zmq::message_t request(logStatement.length());
		memcpy((void *)request.data(), logStatement.c_str(), logStatement.length());
		socket.send(request);
		Sleep(1000);
	}
    return 0;
}

