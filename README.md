# BiedaVR-server

Very simple project for motion tracking in AGH-UST.
Idea for a project is to create experience like in HTC Vive - moving in VR.
However HTC Vive is very expensive, so we decided to use:
- Google Cardboard
- Kinect v1

You need to run server on one computer with connected Kinect, and type IP of a phone with Cardboard.
"Server" sends position of all joints to VR app, which is translated onto position of "balls" in environment.
By default, phone listen on port 8082.

Client is [here](https://github.com/kklocek/BiedaVR-client)

Made by Mateusz Majcher and Konrad Klocek.
