using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Application.Common.Model;

public class JwtConfiguration
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public int ExpiryDays { get; set; }


    public JwtConfiguration(IConfiguration configuration)
    {
        var section = configuration.GetSection("JWT");

        Issuer = section.GetValue<string>("Issuer") ?? throw new ArgumentNullException(nameof(Issuer), "Issuer cannot be null");
        Audience = section.GetValue<string>("Audience") ?? throw new ArgumentNullException(nameof(Audience), "Audience cannot be null");
        Secret = section.GetValue<string>("Secret") ?? throw new ArgumentNullException(nameof(Secret), "Secret cannot be null");
        ExpiryDays = Convert.ToInt32(section.GetValue<string>("ExpiryDays"), CultureInfo.InvariantCulture);
    }
}
