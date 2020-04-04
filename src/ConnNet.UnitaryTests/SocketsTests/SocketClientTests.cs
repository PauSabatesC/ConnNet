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

        /// <summary>
        /// Requirements:
        /// -[TODO] receives valid arguments
        /// -[DONE] arguments saves in the instance _socketc
        /// </summary>
        [Test]
        public void SetConnectionTest()
        {
            var mockTcpClient = new Mock<ITcpClient>();

            NewDefaultSocketClient(mockTcpClient.Object);
            int conn_timeout = 0;
            _socketc.SetConnectionOptions(0);
            Assert.AreEqual(conn_timeout, _socketc.ConnectionTimeout);



        }

        /// <summary>
        /// Requirements:
        /// -[DONE] _socketc returns true or false if conenction has established
        /// -[DONE] ITcpClient.GetStream() is called if connection is ok, if not it's not called
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ConnectTestIsOk()
        {
            var mockTcpClient = new Mock<ITcpClient>();
            mockTcpClient.Setup(foo => foo.Connected()).Returns(true);
            mockTcpClient.Setup(foo => foo.Connect("", 80)).Returns(It.IsAny<Task>());
            mockTcpClient.Setup(foo => foo.IsValidNetStream()).Returns(true);

            NewDefaultSocketClient(mockTcpClient.Object);
            bool res = await _socketc.Connect();
            Assert.IsTrue(res);
            mockTcpClient.Verify(foo => foo.GetStream(), Times.Once());

        }

        /// <summary>
        /// Requirements:
        /// -[DONE] _socketc returns true or false if conenction has established 
        /// -[DONE] ITcpClient.GetStream() is called if connection is ok, if not it's not called 
        /// -[DONE] if the connection is ok but ocurred a problem getting network stream returns false 
        /// </summary>
        [Test]
        public async Task ConnectTestNotOK()
        {
            var mockTcpClient = new Mock<ITcpClient>();
            mockTcpClient.Setup(foo => foo.Connect("", 80)).Returns(It.IsAny<Task>());
            mockTcpClient.Setup(foo => foo.Connected()).Returns(false);

            NewDefaultSocketClient(mockTcpClient.Object);
            bool res = await _socketc.Connect();
            Assert.IsFalse(res);
            mockTcpClient.Verify(foo => foo.GetStream(), Times.Never());


            //if the connection is ok but ocurred a problem getting network stream
            var mockTcpClient2 = new Mock<ITcpClient>();
            mockTcpClient2.Setup(foo => foo.Connect("", 80)).Returns(It.IsAny<Task>());
            mockTcpClient2.Setup(foo => foo.Connected()).Returns(true);
            mockTcpClient2.Setup(foo => foo.IsValidNetStream()).Returns(false);

            NewDefaultSocketClient(mockTcpClient2.Object);
            res = await _socketc.Connect();
            Assert.IsFalse(res);
            mockTcpClient2.Verify(foo => foo.GetStream(), Times.Once());

        }

        /// <summary>
        /// Requirements:
        /// -[TODO] if null received 
        /// -[TODO] if string no ip format 
        /// -[TODO] if string is dns 
        /// -[TODO] if port is <0 
        /// </summary>
        /// <returns></returns>
        public async Task ConnectionTestArgumentValidation()
        {

        }


        /// <summary>
        /// Requirements:
        /// -[DONE] receives valid string 
        /// -[TODO] should mirror return of send(byte[]) if false, it returns false
        /// </summary>
        [Test]
        public async Task SendStringTest()
        {
            var mockTcpClient = new Mock<ITcpClient>();
            mockTcpClient.Setup(foo => foo.SendData(It.IsAny<byte[]>())).Returns(It.Is<Task<bool>>(x => true));
            //receives a valid string
            NewDefaultSocketClient(mockTcpClient.Object);
            Assert.ThrowsAsync<ArgumentException>(() =>  _socketc.Send(""));
            Assert.ThrowsAsync<ArgumentException>(() => _socketc.Send(" "));

            //should mirror return of send(byte[]) if false, it returns false
            bool res = await _socketc.Send("test");
            Assert.IsTrue(res);
            mockTcpClient.Setup(foo => foo.SendData(It.IsAny<byte[]>())).Returns(It.Is<Task<bool>>(x => true));
            NewDefaultSocketClient(mockTcpClient.Object);
            res = await _socketc.Send("test");
            Assert.IsFalse(res);
        }

        /// <summary>
        /// Requirements:
        /// -[DONE] receives valid byte[] 
        /// -[DONE] calls ITcpClient.SendData(byte[]) and awaits
        /// -[TODO] returns true if ITcpClient.SendData(byte[]) is true and reverse
        /// </summary>
        [Test]
        public async Task SendBytesTest()
        {
            //receives a valid string
            var mockTcpClient = new Mock<ITcpClient>();

            NewDefaultSocketClient(mockTcpClient.Object);
            byte[] nullBytes = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => _socketc.Send(nullBytes));

            //calls ITcpClient.SendData(byte[]) and awaits
            byte[] testBytes = Utils.Conversor.StringToBytes("test");
            await _socketc.Send(testBytes);
            mockTcpClient.Verify(foo => foo.SendData(testBytes), Times.Once());

            //returns true if ITcpClient.SendData(byte[]) is true and reverse            
            mockTcpClient.Setup(foo => foo.SendData(It.IsAny<byte[]>())).Returns(Task.FromResult(true)).Verifiable();
            NewDefaultSocketClient(mockTcpClient.Object);
            
            byte[] byteTest = new byte[] { 0x04 };
            bool res = await _socketc.Send(byteTest);
            Assert.IsTrue(res);

        }


    }
}
