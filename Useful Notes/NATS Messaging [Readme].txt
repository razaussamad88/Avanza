
::: HOW TO IMPLEMENT NATS MESSAGING SYSTEMS ::: 

=============================================================
~~~~~~~  PRE-REQ  TOOLS 
=============================================================

1. NATS Server
2. NATS Client
3. Chocolatey - Package Manager for Windows (if needed)



##########  NATS  MESSAGING  COMMANDS  ##########


CREATE AND USE CONNECTION
------------------------------
	nats context save nats_avanza --server=nats://172.16.11.71:4222 --user=nouman --password=nouman.nawaz@avanzasolutions.com
	nats context select nats_avanza


STREAM COMMANDS
------------------------------
	nats stream info ASPIREWEBUI
	nats stream purge ASPIREWEBUI
	nats stream ls
	
	
	nats subscribe "ASPIREWEBUI.FREE_REQUESTS"
	
	



##################################################################################################################################
WEB LINKS FOR NATS MESSAGING LEARNING
==================================================================================================================================

	https://natsbyexample.com/		(Best for learning)
	
	https://chocolatey.org/install
	https://cloud.synadia.com/systems/2g8AiHm2C0m0KTY7vCWp6B68lif/welcome	
	
	https://www.scaleway.com/en/docs/serverless/messaging/api-cli/nats-cli/#create-a-stream
	
	NATS Tutorial Links
		https://docs.nats.io/nats-concepts/core-nats/reqreply
		https://docs.nats.io/nats-concepts/jetstream
		https://docs.nats.io/nats-concepts/what-is-nats
		https://docs.nats.io/nats-concepts/what-is-nats/walkthrough_setup
		https://docs.nats.io/running-a-nats-service/introduction/installation
		https://docs.nats.io/running-a-nats-service/introduction/windows_srv
		https://docs.nats.io/using-nats/developer
		https://docs.nats.io/using-nats/nats-tools/nats_cli
		
	GitHub Links
		https://github.com/nats-io/nats.net
		https://github.com/nats-io/natscli
	
	YouTube Links
		https://www.youtube.com/watch?v=HfUVwzfrrNo
		https://www.youtube.com/watch?v=hjXIUPZ7ArM