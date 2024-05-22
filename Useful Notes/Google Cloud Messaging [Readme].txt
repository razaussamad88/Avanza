
=============================================================
~~~~~~~  PRE-REQ  KEYS  AND  FILES 
=============================================================
1. Create a Online Project on FireBase.
2. Get following keys from the newly created FireBase project.
	- Web API Key
	- Server Key
	- Project Id
	- Service Account Id
	- Service Account's Downloaded JSON File Path (File)


=============================================================
~~~~~~~  PRE-REQ  TOOLS 
=============================================================

1. Node.JS latest
2. Following Visual Studio Packages
	- Install-Package FirebaseAdmin
	- install-package Google.Cloud.Translation.V2 -pre
	- Install-Package Google.Api.Gax
	- Install-Package Google.Apis.Auth
	- Install-Package Google.Apis.Translate.v2
	- PushNotification_dotNET


########## WEB PUSH ##########
1. Generate a web application with in-built JavaScript code for Cloud Messaging.
2. Host the application on web server. (or host on firebase server locally)
3. Whenever WEB link access on any device, Device will receive a TOKEN from Cloud Messaging Server and that token stored in localStorage.
   Then Ambit received that token and share with RDV.


   
########## ANDRIOD PUSH ##########
1. Generate JSON for Android from FireBase. And share with Android team.
2. Whenever APP install on any Android device, Device ID will be share with Ambit and then Ambit share with RDV.



########## iOS PUSH ##########
1. Generate pList for iOS from FireBase. And share with iOS team.
2. Get APNS (p8) from iOS team. And upload to FireBase iOS App.
3. Whenever APP install on any iOS device, Device ID will be share with Ambit and then Ambit share with RDV.








##################################################################################################################################
WEB LINKS FOR CLOUD MESSAGING LEARNING
==================================================================================================================================
	
	https://console.firebase.google.com/u/1/project/avanzasymmetry/overview
	
	https://firebase.google.com/docs/admin/setup
	https://firebase.google.com/docs/auth/admin/create-custom-tokens
	https://cloud.google.com/translate/docs/reference/libraries
	https://firebase.google.com/docs/web/setup?authuser=1

	https://console.cloud.google.com/iam-admin/serviceaccounts?project=avanzasymmetry&authuser=1&pli=1

	https://firebase.google.com/docs/cloud-messaging/send-message#customize_messages_across_platforms

	https://firebase.google.com/docs/hosting/quickstart
	C:\Users\avanza\AppData\Roaming\npm
	firebase init
	firebase deploy
	firebase serve

	https://firebase.google.com/docs/cloud-messaging/http-server-ref
	https://github.com/web-push-libs/web-push/
	https://firebase.google.com/docs/cloud-messaging/js/client?authuser=1
	npm install web-push -g
