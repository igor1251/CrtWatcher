using NUnit.Framework;
using System;
using DataStructures;

namespace ModelTests
{
    [TestFixture]
    public class CertificateModelTest
    {
        Certificate certificate;

        [SetUp]
        public void Setup()
        {
            certificate = new Certificate();
        }

        [Test]
        public void FieldsTest()
        {
            Assert.Throws<ArgumentException>(() => {
                certificate.ID = -1;
            });
            Assert.Throws<ArgumentException>(() => {
                certificate.CertificateHash = "";
            });
            Assert.Throws<ArgumentException>(() => {
                certificate.CertificateHash = " ";
            });
            Assert.Throws<ArgumentException>(() => {
                certificate.CertificateHash = null;
            });
            Assert.Throws<ArgumentException>(() => {
                certificate.Algorithm = "";
            });
            Assert.Throws<ArgumentException>(() => {
                certificate.Algorithm = null;
            });
            Assert.Throws<ArgumentException>(() => {
                certificate.Algorithm = " ";
            });
        }
    }
}