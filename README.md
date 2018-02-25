# Phos
Phos is a simple personal project for updating my anime list automatically when I finish watching an episode on my Plex server.
It uses Plex webhooks and the MAL API to handle everything.

If you want to pull the code down and use it for yourself, you will need to generate a Web.config file with MalUserName, MalPassword,
a RootDirectory for your website, and a TraceOutputFilename for logs. As this was intended as a personal project I didn't add any
kind of security related to adding credentials, so feel free to add that yourself if you want it. 
