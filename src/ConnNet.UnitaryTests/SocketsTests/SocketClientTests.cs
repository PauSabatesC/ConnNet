using ConnNet.Sockets;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConnNet.UnitaryTests.SocketClientTests
{
    [TestFixture]
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

        private void NewDefaultSocketClient(ITcpClient mockTcpClient)
        {
            Ip = "0.0.0.0";
            Port = 80;
            _socketc = new SocketClient(Ip, Port, mockTcpClient);
        }

        [Test]
        public void SetConnectionTest()
        {
            var mockTcpClient = new Mock<ITcpClient>();

            NewDefaultSocketClient(mockTcpClient.Object);
            int conn_timeout = 0;
            _socketc.SetConnectionOptions(0);
            Assert.AreEqual(conn_timeout, _socketc.ConnectionTimeout);



        }

        [Test]
        public void SetConnectionTestArgumentValidation()
        {
            //check null received

            //

        }

        [Test]
        public async Task ConnectTestIsOk()
        {
            var mockTcpClient = new Mock<ITcpClient>();
            mockTcpClient.Setup(foo => foo.Connected()).Returns(true);
            mockTcpClient.Setup(foo => foo.Connect("", 80)).Returns(It.IsAny<Task>());

            NewDefaultSocketClient(mockTcpClient.Object);
            bool res = await _socketc.Connect();
            Assert.IsTrue(res);
            mockTcpClient.Verify(foo => foo.GetStream(), Times.Once());

        }

        [Test]
        public async Task ConnectTestNotOK()
        {
            var mockTcpClient2 = new Mock<ITcpClient>();
            mockTcpClient2.Setup(foo => foo.Connect("", 80)).Returns(It.IsAny<Task>());
            mockTcpClient2.Setup(foo => foo.Connected()).Returns(false);

            NewDefaultSocketClient(mockTcpClient2.Object);
            bool res = await _socketc.Connect();
            Assert.IsFalse(res);
            mockTcpClient2.Verify(foo => foo.GetStream(), Times.Never());
        }

        public async Task ConnectionTestArgumentValidation()
        {
            //if null received 

            //if string no ip format

            //if string is dns

            //if port is <0
        }


        [Test]
        public void SendTest()
        {
            var mockTcpClient = new Mock<ITcpClient>();

            NewDefaultSocketClient(mockTcpClient.Object);

            //should return true if all ok


            //false if problem


        }
    }
}
