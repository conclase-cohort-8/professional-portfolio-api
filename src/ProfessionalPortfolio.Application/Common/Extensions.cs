using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProfessionalPortfolio.Application.Common
{
    public static class Extensions
    {
        public static string GetLoggedInUserId(this ClaimsPrincipal? claimsPrincipal)
        {
            return claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public static string GenerateOtp(int length = 5)
        {
            if(length < 4)
            {
                throw new InvalidOperationException("OTP length must be at least 4");
            }

            using var range = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            range.GetBytes(bytes);

            int randomNumber = BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF;
            var otp = randomNumber % (int)Math.Pow(10, length);
            return otp.ToString(new string('0', length));
        }

        public static (string Hash, string Salt) HashOtp(string otp)
        {
            var saltByte = RandomNumberGenerator.GetBytes(16);

            using var hmac = new HMACSHA256(saltByte);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));

            return (Convert.ToBase64String(hash), Convert.ToBase64String(saltByte));
        }

        public static bool IsAValidOtp(string otp, string storedSalt, string storedHash, DateTime time)
        {
            var saltByte = Convert.FromBase64String(storedSalt);

            using var hmac = new HMACSHA256(saltByte);
            var hashedOtp = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));

            var computedHash = Convert.ToBase64String(hashedOtp);
            return computedHash == storedHash && time > DateTime.UtcNow;
        }
    }
}
