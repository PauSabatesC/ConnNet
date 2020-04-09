using ConnNet.Sockets;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        /// -[TODO] if connection reaches timeout
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
        /// -[TODO] if received connectionTimeout too, check if >= 0
        /// </summary>
        /// <returns></returns>
        /*public async Task ConnectionTestArgumentValidation()
        {

        }*/

        /// <summary>
        /// Requirements:
        /// -[DONE] receives valid string 
        /// -[DONE] should mirror return of send(byte[]) so it has to end up calling ITcpClient.Send()
        /// </summary>
        [Test]
        public async Task SendStringTest()
        {
            var mockTcpClient = new Mock<ITcpClient>();
            mockTcpClient.Setup(foo => foo.Connected()).Returns(true);
            mockTcpClient.Setup(foo => foo.IsValidNetStream()).Returns(true);
            mockTcpClient.Setup(foo => foo.CanWrite()).Returns(true);
            //receives a valid string
            NewDefaultSocketClient(mockTcpClient.Object);
            Assert.ThrowsAsync<ArgumentException>(() =>  _socketc.Send(""));
            Assert.ThrowsAsync<ArgumentException>(() => _socketc.Send(" "));

            //should mirror return of send(byte[]) so it has to end up calling ITcpClient.Send()
            NewDefaultSocketClient(mockTcpClient.Object);
            await _socketc.Send("test");
            mockTcpClient.Verify(foo => foo.SendData(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.Once());
            
        }

        /// <summary>
        /// Requirements:
        /// -[DONE] send(byte[]) should mirror return of send(byte[], int) so it has to end up calling ITcpClient.Send()
        /// -[DONE] catch OperationCanceledException thrown for timeout and resend with user friendly message
        /// -[DONE] check timeout is >=0
        /// -[DONE] check ITcpClient.IsValidStream() is true before call ITcpClient.SendData(byte[])
        /// </summary>
        [Test]
        public async Task SendBytesTest()
        {
            var mockTcpClient = new Mock<ITcpClient>();
            mockTcpClient.Setup(foo => foo.Connected()).Returns(true);
            mockTcpClient.Setup(foo => foo.IsValidNetStream()).Returns(true);
            mockTcpClient.Setup(foo => foo.CanWrite()).Returns(true);
            NewDefaultSocketClient(mockTcpClient.Object);
            byte[] testBytes = Utils.Conversor.StringToBytes("test");

            //check timeout is >=0
            Assert.ThrowsAsync<ArgumentException>(() => _socketc.Send(testBytes, -5000));
            Assert.ThrowsAsync<ArgumentException>(() => _socketc.Send(testBytes, 0));

            //send(byte[]) should mirror return of send(byte[], int) so it has to end up calling ITcpClient.Send()
            //byte[] byteTest = new byte[] { 0x04 };
            int timeoutTest = 5000;

            await _socketc.Send(testBytes);
            mockTcpClient.Verify(foo => foo.SendData(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            await _socketc.Send(testBytes, timeoutTest);
            mockTcpClient.Verify(foo => foo.SendData(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            //mockTcpClient.Setup(foo => foo.SendData(It.IsAny<byte[]>())).Returns(Task.FromResult(true)).Verifiable();
            //mockTcpClient.Setup(foo => foo.SendData(It.IsAny<byte[]>(), It.IsAny<CancellationToken>())).Returns(It.IsAny<Task>()).Verifiable();
            
            //check ITcpClient.IsValidStream() is true before call ITcpClient.SendData(byte[])
            var mockTcpClient2 = new Mock<ITcpClient>();
            mockTcpClient.Setup(foo => foo.Connected()).Returns(true);
            mockTcpClient2.Setup(foo => foo.IsValidNetStream()).Returns(false);
            mockTcpClient2.Setup(foo => foo.CanWrite()).Returns(true);
            NewDefaultSocketClient(mockTcpClient2.Object);
            Assert.ThrowsAsync<Exception>(() => _socketc.Send(testBytes, 5000));
            mockTcpClient.Setup(foo => foo.Connected()).Returns(true);
            mockTcpClient2.Setup(foo => foo.IsValidNetStream()).Returns(true);
            mockTcpClient2.Setup(foo => foo.CanWrite()).Returns(false);
            NewDefaultSocketClient(mockTcpClient2.Object);
            Assert.ThrowsAsync<Exception>(() => _socketc.Send(testBytes, 5000));

            //catch OperationCanceledException thrown for timeout and resend with user friendly message
            var mockTcpClient3 = new Mock<ITcpClient>();
            mockTcpClient3.Setup(foo => foo.Connected()).Returns(true);
            mockTcpClient3.Setup(foo => foo.IsValidNetStream()).Returns(true);
            mockTcpClient3.Setup(foo => foo.CanWrite()).Returns(true);
            mockTcpClient3.Setup(foo => foo.SendData(It.IsAny<byte[]>(), It.IsAny<CancellationToken>())).Throws(new OperationCanceledException()).Verifiable();
            NewDefaultSocketClient(mockTcpClient3.Object);
            var ex = Assert.ThrowsAsync<OperationCanceledException>(() => _socketc.Send(testBytes, 5000));
            Assert.That(ex.Message, Is.EqualTo("Timeout of " + 5000.ToString() + " trying to send the data."));
            //the same test as above but simulating a delay
            /*mockTcpClient3.Setup(foo => foo.SendData(It.IsAny<byte[]>(), It.IsAny<CancellationToken>())).Callback(() => Thread.Sleep(4000));
            NewDefaultSocketClient(mockTcpClient3.Object);
            ex = Assert.ThrowsAsync<OperationCanceledException>(() => _socketc.Send(testBytes, 2000));
            Assert.That(ex.Message, Is.EqualTo("Timeout of " + 5000.ToString() + " trying to send the data."));
            */

        }

        /// <summary>
        /// Requirements:
        /// -[TODO] should return a string or byte[]
        /// -[TODO] can receive string or byte delimitator Example:"<etx>" so read only until then, and starting delimitator too
        /// -[TODO] can receive timeout so OperationCanceledException exception has to be handled with custom message
        /// -[TODO] can receive number of bytes to read
        /// -[TODO] can receive buffer size
        /// </summary>
        [Test]
        public async Task ReceiveTest()
        {
            byte[] testBuffer = Utils.Conversor.StringToBytes("test");
            KeyValuePair<int,byte[]> kayPairReturned = new KeyValuePair<int, byte[]>(4, testBuffer);
            
            //should return a string or byte[]
            var  mockTcpClient = new Mock<ITcpClient>();
            mockTcpClient.SetupSequence(foo => foo.DataAvailable()).Returns(true).Returns(false);
            mockTcpClient.Setup(foo => foo.CanRead()).Returns(true);
            mockTcpClient.Setup(foo => foo.ReadData(It.IsAny<byte[]>(),It.IsAny<CancellationToken>())).Returns(Task.FromResult(kayPairReturned));
            NewDefaultSocketClient(mockTcpClient.Object);
            string recString = await _socketc.ReceiveString();
            Assert.AreEqual("test", recString);
        }


    }
}