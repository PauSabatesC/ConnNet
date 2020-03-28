using ConnNet.Sockets;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnNet.UnitaryTests.SocketClientTests
{
    public class SocketClientTests
    {
        private SocketClient _socketc;
        private string _ip;
        private int _port;

        public string Ip { get => _ip; set => _ip = value; }
        public int Port { get => _port; set => _port = value; }

        [SetUp]
        public void Setup()
        {
        }

        private void NewDefaultSocketClient(ITcpClient mockTcpClient, IAsyncSocket mockAsyncSocket)
        {
            Ip = "0.0.0.0";
            Port = 80;
            _socketc = new SocketClient(Ip, Port, mockTcpClient, mockAsyncSocket);
        }

        [Test]
        public void SetConnectionTest()
        {
            var mockTcpClient = new Mock<ITcpClient>();
            var mockedAsyncSocked = new Mock<IAsyncSocket>();

            NewDefaultSocketClient(mockTcpClient.Object, mockedAsyncSocked.Object);
            int default_conn_timeout = 5000;
            int conn_timeout = 0;
            //socketc.SetConnection("0.0.0.0", 80);
            Assert.AreEqual(Ip, _socketc.SocketIP);
            Assert.AreEqual(Port, _socketc.SocketPort);
            Assert.AreEqual(default_conn_timeout, _socketc.ConnectionTimeout);

            _socketc.SetConnection("0.0.0.0", 80,0);
            Assert.AreEqual(conn_timeout, _socketc.ConnectionTimeout);



        }

        [Test]
        public void ConnectTest()
        {
            //aux variables
            IAsyncResult ar = new Mock<IAsyncResult>().Object;
            //IAsyncResult mockedIAsyncResult = Mock.Of<IAsyncResult>();

            var mockedAsyncSocked = new Mock<IAsyncSocket>();
            mockedAsyncSocked.Setup(foo => foo.AsyncWaitHandle(ar, 5000)).Returns(true);

            // --- Test if the connection is ok ---
            var mockTcpClient = new Mock<ITcpClient>();
            mockTcpClient.Setup(foo => foo.Connected()).Returns(true);
            mockTcpClient.Setup(foo => foo.BeginConnect(_ip,_port,null,null)).Returns(ar);
            mockTcpClient.Setup(foo => foo.EndConnect(It.IsAny<IAsyncResult>()));

            NewDefaultSocketClient(mockTcpClient.Object, mockedAsyncSocked.Object);
            bool res = _socketc.Connect();
            Assert.IsTrue(res);

            //--- Test if connection not okay ---
            mockTcpClient.Setup(foo => foo.Connected()).Returns(false);

            NewDefaultSocketClient(mockTcpClient.Object, mockedAsyncSocked.Object);
            res = _socketc.Connect();
            Assert.IsFalse(res);



        }
    }
}
