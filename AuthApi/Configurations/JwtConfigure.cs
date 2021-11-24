using System.Text;

namespace AuthApi.Configurations
{
    public class JwtConfigure
    {
        public string Key { get; set; } = string.Empty;
        public byte[] KeyBytes => Encoding.UTF8.GetBytes(Key);
        public string Issuer { get; set; } = string.Empty;
    }
}