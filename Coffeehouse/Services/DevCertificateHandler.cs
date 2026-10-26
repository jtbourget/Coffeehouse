#if DEBUG
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Coffeehouse.Services;

public class DevCertificateHandler : HttpClientHandler
{
    public DevCertificateHandler()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            ServerCertificateCustomValidationCallback = ValidateLocalhostCertificate;
        }
    }

    private static bool ValidateLocalhostCertificate(
        HttpRequestMessage request,
        X509Certificate2? cert,
        X509Chain? chain,
        SslPolicyErrors errors)
    {
        if (cert != null && cert.Issuer.Equals("CN=localhost", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return errors == SslPolicyErrors.None;
    }
}
#endif
