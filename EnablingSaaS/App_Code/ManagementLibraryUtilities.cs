using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace WamlAndWaws
{
    public static class ManagementLibraryUtilities
    {
        private static X509Certificate2 GetCertificate()
        {
            string certPath = HttpContext.Current.Server.MapPath(
                ConfigurationManager.AppSettings["CERTIFICATE-PATH"]
                );

            var x509Cert = new X509Certificate2(certPath,
                ConfigurationManager.AppSettings["CERTIFICATE-PASSWORD"]
                );

            return x509Cert;
        }

        public static SubscriptionCloudCredentials GetCredentials()
        {
            var subscriptionId = ConfigurationManager.AppSettings["AZURE-SUBSCRIPTION-ID"];

            return new CertificateCloudCredentials(subscriptionId, GetCertificate());
        }
    }
}