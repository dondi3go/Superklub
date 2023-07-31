# Superklub

A c# client for supersynk server used to exhange 3D visualization data

The goal was to make it usable in various dotnet implementations : Unity, official .Net.

The goal is also to have connectitivity between different kind of 3D applications : 
* VR with Unity
* Simple 3D apps made with Visual Studio and Raylib
* Some Console logger made with Visual Studio

And possibly have various apps contributing to a common 3D environement.

## History

A first project realizing connectivity between XR apps have been done in 2021.

But it was considered (by me) too dependant on Unity at the time, I could
not use it in C# projects unrelated to Unity (Console, Raylib).

## Difficulties

* Http requests are not handled in the same way in Unity and in officiel .Net
* The serialization systems are not the same (at all)
* Some basic structures like Vector3 and Quaternion are also different

## What you can do with this repo

Not much. The repo was created to share the Superklub code between
the different projects using it.

Go see **SuperklubMSVSTests** to get the HttpClient usable with
official .Net implementation and see some usages.

Go see **SuperklubUnityTest** to get the Unity HTTPClient and see
how the updates in the scene graph can be done.

A better way to distribute Superklub for Unity would be to create
a Unity package, and include some additional Unity code in it.

## Improvements
* Many parts need rework, especially the Json Serialization part
