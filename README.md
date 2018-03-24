# Phos
Phos is a simple personal project for updating my anime list automatically when I finish watching an episode on my Plex server.
It uses Plex webhooks and the MAL API to handle everything.

Setup instructions:
- pull down the code
- create a new Publish Profile for your local machine to host the site
- Post your MAL username, password, and the email associated to your Plex account to the api/PlexRegister endpoint, or create a file called "creds.txt" in the root Phos directory
