# zmqHub
A Publisher is an one way data distribution. It is based on publisher-subscriber pattern. A VC++ code for publishing the data to client. 
		it uses libzmq for its communications. A non blocking, asynchronous call is being made between publisher and subscriber. we have not set
		any start or end. its like never ending broadcast. Had added some signal processing for capturing the ctrl+C signal and doing clean exit.
		
Subscriber:
		Sub is a client code based on same publisher-subscriber pattern. It allows client to subscribe on topic and then receive only messages which
		are relevant to client. A client can subscribe multiple subscriber. So any message matching the subscriber, will be available for client to
		process. We also haave added some logic for calculating average for multiple messages and then display in one go. 
		
NetMQ
  We have created a subscriber using NetMQ as we need to explore the option for C#. this client is exactly same copy as Scanner. 
