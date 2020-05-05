<p align="center">
  <img src="https://github.com/PauSabatesC/SockNet/blob/develop/SockNet.PNG" alt="SockNet logo"/>
</p>

<h1 align="center">
  The easiest and fastest way to work with sockets in C#
</h1>

[![CodeFactor](https://www.codefactor.io/repository/github/pausabatesc/socknet/badge)](https://www.codefactor.io/repository/github/pausabatesc/socknet)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/97b9677cd0354202b7d0bb4fd9e364fb)](https://www.codacy.com/manual/PauSabatesC/SockNet?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=PauSabatesC/SockNet&amp;utm_campaign=Badge_Grade)
[![Nuget](https://img.shields.io/nuget/v/SockNet)](https://www.nuget.org/packages/SockNet/)
[![Build status](https://ci.appveyor.com/api/projects/status/x9mkgssl3n6yb9p7?svg=true)](https://ci.appveyor.com/project/PauSabatesC/socknet-kaa6k)


### This library is a wrapper of common tcp and network streams libaries in order to act as a more high-level and developer friendly api to deal with socket communications.


## Installation
[![Nuget](https://img.shields.io/nuget/v/SockNet)](https://www.nuget.org/packages/SockNet/)

```powershell
dotnet add package SockNet
```

**Supported Runtimes**

<table>
<tr>
<th></th>

<th>Windows</th>
	
<th>Linux</th>

<th>Mac OS X</th>

</tr>
<tr>
 <td><strong>.NET Framework</strong</td>
 <td align='center'><strong>v4.6+</strong></td>
 <td align='center'>n/a</td>
 <td align='center'>n/a</td>
</tr>
<tr>
 <td><strong>.NET Standard</strong></td>
 <td colspan='3' align='center'>.NET Standard 2.0+</td>
</tr>
<tr>
 <td><strong>.NET Core</strong></td>
 <td colspan='3' align='center'>v2.0+</td>
</tr>
<tr>
	<td colspan='4'></td>
</tr>
</table>


If you want to get the latest development build: [![Build status](https://ci.appveyor.com/api/projects/status/x9mkgssl3n6yb9p7/branch/develop?svg=true)](https://ci.appveyor.com/project/PauSabatesC/socknet-kaa6k/branch/develop)

---

## Features
🚀 **Production ready** library. Forget to waste time testing buffers and tcp framing communication, we cover you.

🧵 **Asynchronous** connections and write/read petitions.

📚 **Developer friendly API** with many method overloads to satisfy all user use cases.

🎭 **Client and/or Server** sockets creation. Use it just for your .Net client or for both client and server!


## Why?
Working with sockets with C# can be really overwhelming due to its different libraries and many methods to choose from.

For example you have to deal with TcpClient, NetworkStream, TcpListener classes with really similar methods between them, but with different purposes, and within each class there are different ways to achieve the same goal. 

So you end up searching in the internet and official documentation every time you need to work with sockets but you don't know which of the many solutions you found is the more correct,fully asynchronous, production ready and newer option.


## Usage example

**Client socket**

To create a client socket connection you only need these methods:

Initializes the client indicating the server address:
```cs
  var client = new SocketClient("127.0.0.1", 80);
```
Sends and receive data asynchronously once its connected:
```cs
  if (await client.Connect())
  {
      await client.Send("Am I cool?");
      var recData = await client.ReceiveBytes();
  }
```

**Server socket**

To create a socket server listening asynchronously to many petitions and able to response them:

Initialization of the server:
```cs
  var server = new SocketServer();
  server.InitializeSocketServer("127.0.0.1", 80);
  server.SetReaderBufferBytes(1024);
  server.StartListening();
```
Sends and receive data once its connected:
```cs
  if(server.IsNewData())
  {
      var data = server.GetData();
      // Do whatever you want with data
      Task.Run(() => DoSomething(data, server));
  }
```
Send data back to the client:
```cs
  server.ResponseToClient(data.Key, "this is cool!");
```
<br>

**FAQ: But how the receive method works? What if I have a custom socket message with delimitators for instance?**

Both socket and client have different methods to achive that, and the goal is to keep adding as much as possible to satisfy all common uses cases.

For instance you can set the server to receive data with these methods:
```cs
// Sets the server to receive an unknown number of bytes. Reads data until it stops receiving.
SetReaderBytes();
SetReaderBufferBytes(int bufferSize);

// Sets the server to receive the specified number of bytes
SetReaderNumberOfBytes(int bufferSize, int numberBytesToRead);

//Sets the receiver to get the tcp data telegram. Reads from the start delimitator until the end delimitator is reached.
SetReaderBytesWithDelimitators(byte[] startDelimitator, byte[] endDelimitator);
SetReaderBytesWithEndDelimitator(byte[] endDelimitator);

//... more to be added!!
```

## Documentation

The API expose to different interfaces: ISocketClient and ISocketServer.

Both are xml documented so Visual Studio Intellisense will explain each method and parameter, as well as its different method overloads options.
Nevertheless, you can see it <a href="https://github.com/PauSabatesC/SockNet/blob/develop/src/SockNet/ClientSocket/ISocketClient.cs">here</a> and <a href="https://github.com/PauSabatesC/SockNet/blob/develop/src/SockNet/ServerSocket/ISocketServer.cs">here</a>.


## Ready to try example

**Client**
```cs
  byte[] recData = null;
  SocketClient client = new SocketClient("127.0.0.1", 9999);
  try
  {
      if (await client.Connect())
      {
          await client.Send("this is a test");
          recData = await client.ReceiveBytes();
      }
      Console.WriteLine("Received data: " + Encoding.UTF8.GetString(recData));
  }
  catch(Exception e)
  {
      Console.WriteLine("Exception raised: " + e);
  }
  //...
  client.Disconnect();
```

**Server**
```cs
  var socketServer = new SocketServer();
  socketServer.InitializeSocketServer("127.0.0.1", 9999);
  socketServer.SetReaderBufferBytes(1024);
  socketServer.StartListening();

  bool openServer = true;
  while (openServer)
  {
      if(socketServer.IsNewData())
      {
          var data = socketServer.GetData();
          // Do whatever you want with data
          Task.Run(() => DoSomething(data, socketServer));
      }
  }

  //.... 
  socketServer.CloseServer();


  private static void DoSomething(KeyValuePair<TcpClient, byte[]> data, SocketServer server)
  {
    Console.WriteLine(((IPEndPoint)data.Key.Client.RemoteEndPoint).Address.ToString() + ": " + Encoding.UTF8.GetString(data.Value));
    server.ResponseToClient(data.Key, "received");
  }
```

## Contribute!

> Feel free to contribute! Submit your pull requests, report issues or propose new features!
