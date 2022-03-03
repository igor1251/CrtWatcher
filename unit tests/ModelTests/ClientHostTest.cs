using NUnit.Framework;
using System;
using DataStructures;

namespace ModelTests
{
    [TestFixture]
    public class ClientHostTest
    {
        ClientHost host;

        [SetUp]
        public void Setup()
        {
            host = new ClientHost();
        }

        [Test]
        public void FieldsTest()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                host.IP = "weitrvuwetib";
            });
            Assert.Throws<ArgumentException>(() =>
            {
                host.ConnectionPort = 31451345;
            });
            Assert.DoesNotThrow(() =>
            {
                host.IP = "localhost";
            });
            Assert.DoesNotThrow(() =>
            {
                host.IP = "192.168.128.210";
            });
            Assert.DoesNotThrow(() =>
            {
                host.ConnectionPort = 5000;
            });
        }
    }
}
