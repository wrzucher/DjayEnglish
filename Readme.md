# DjayEnglish setup

We use TTS text-to-speach generator: https://github.com/mozilla/TTS/
Before you run project you have to run docker container with TTS server
(and install docker too, if you don't have it)

    docker run -it -p 5002:5002 synesthesiam/mozillatts:en