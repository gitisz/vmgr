using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Vmgr.SharePoint
{
    /// <summary>
    /// Accepts cert errors likely to happen when calling a WFE directly.
    /// </summary>
    public class CertificatePolicy
    {
        public static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
}