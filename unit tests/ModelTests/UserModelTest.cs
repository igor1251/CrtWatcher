using NUnit.Framework;
using System;
using DataStructures;

namespace ModelTests
{
    [TestFixture]
    public class UserModelTest
    {
        User user;

        [SetUp]
        public void Setup()
        {
            user = new User();
        }

        [Test]
        public void FieldsTest()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                user.ID = -1;
            });
            Assert.DoesNotThrow(() =>
            {
                user.ID = 10;
            });
            Assert.Throws<ArgumentException>(() =>
            {
                user.UserPhone = "erwuvybweurv";
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                user.UserPhone = null;
            });
            Assert.DoesNotThrow(() =>
            {
                user.UserPhone = "8(961)003-71-51";
            });
            Assert.DoesNotThrow(() =>
            {
                user.UserPhone = "+7(961)003-71-51";
            });
            Assert.DoesNotThrow(() =>
            {
                user.UserPhone = "+79610037151";
            });
            Assert.DoesNotThrow(() =>
            {
                user.UserPhone = "89610037151";
            });
            Assert.DoesNotThrow(() =>
            {
                user.UserPhone = "+7 961 003 71 51";
            });
            Assert.DoesNotThrow(() =>
            {
                user.UserPhone = "8 961 003 71 51";
            });
            Assert.Throws<ArgumentException>(() =>
            {
                user.UserPhone = "898989898";
            });
        }
    }
}