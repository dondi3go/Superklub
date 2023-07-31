# Superklub
A c# client for supersynk server used to exhange 3D visualization data

The goal was to make it usable in various dotnet implementations : Unity, official .Net.
The goal is also to have connectitivity between different kind of 3D applications : 
* VR with unity
* Simple 3D apps made with Visual Studio and Raylib
* Some Console logger made with Visual Studio

And possibly have various apps contributing to a common 3D environement.

## History

A first project realizing Connectivity between XR apps have been done in 2021.
But it was considered (by me) too dependant on Unity.

## Difficulties

* Http requests are not handled in the same way in Unity and in officiel .Net
* The serialization systems are not the same (at all)
* Some basic structures like Vector3 are also different
