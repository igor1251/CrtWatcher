using NUnit.Framework;
using DataStructures;

namespace ModelTests
{
    [TestFixture]
    public class UserModelTest
    {
        User user;
        Settings settings;
        Certificate certificate;
        ClientHost host;

        [SetUp]
        public void Setup()
        {
            user = new User();
            settings = new Settings();
            certificate = new Certificate();
            host = new ClientHost();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}