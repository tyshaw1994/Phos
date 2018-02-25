# Phos
Phos is a simple personal project for updating my anime list automatically when I finish watching an episode on my Plex server.
It uses Plex webhooks and the MAL API to handle everything.

If you want to pull the code down and use it for yourself, you will need to generate a Web.config file with MalUserName, MalPassword,
a RootDirectory for your website, and a TraceOutputFilename for logs. As this was intended as a personal project I didn't add any
kind of security related to adding credentials, so feel free to add that yourself if you want it. 

Setup instructions:
- pull down the code
- create a new Publish Profile for your local machine to host the site
- Post your MAL username, password, and the email associated to your Plex account to the api/PlexRegister endpoint, or create a file called "creds.txt" in the root Phos directory
