using ConnNet.Sockets;
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

        private void NewDefaultSocketClient()
        {
            ITcpClient mockTcpClient = new TcpClientAdapterMock();
            Ip = "0.0.0.0";
            Port = 80;
            _socketc = new SocketClient(Ip, Port, mockTcpClient);
        }

        [Test]
        public void SetConnectionTest()
        {
            NewDefaultSocketClient();

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
            NewDefaultSocketClient();



        }
    }
}
