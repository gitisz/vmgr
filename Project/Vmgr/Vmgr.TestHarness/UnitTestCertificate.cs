using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;
using Vmgr.Crypto;

namespace Vmgr.TestHarness
{
    [TestClass]
    public class UnitTestCertificate
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (CryptContext ctx = new CryptContext())
            {
                ctx.Open();

                X509Certificate2 cert = ctx.CreateSelfSignedCertificate(
                    new SelfSignedCertProperties
                    {
                        IsPrivateKeyExportable = true,
                        KeyBitLength = 4096,
                        Name = new X500DistinguishedName("CN=TEST.CERT"),
                        ValidFrom = DateTime.Today.AddDays(-1),
                        ValidTo = DateTime.Today.AddYears(1),
                    }
                )
                ;

                X509Store storePersonal = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                storePersonal.Open(OpenFlags.ReadWrite);
                storePersonal.Add(cert);
                storePersonal.Close();

                X509Store storeRoot = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                storeRoot.Open(OpenFlags.ReadWrite);
                storeRoot.Add(cert);
                storeRoot.Close();
            }
        }
    }
}
