using WebApi.Infrastructure.Security;

namespace WebApi.Extensions
{
    public static class StringExtensions
    {
        public static string Encrypt(this string str, string salt)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            return new Sha256Hash(str, salt).ToBase64String();
        }
    }
}