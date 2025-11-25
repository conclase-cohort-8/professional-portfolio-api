using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProfessionalPortfolio.Application.Common
{
    public static class Extensions
    {
        public static Guid GetLoggedInUserId(this ClaimsPrincipal? claimsPrincipal)
        {
            if(claimsPrincipal != null && Guid.TryParse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return userId;
            return Guid.Empty;
        }

        public static string GenerateOtp(int length = 5)
        {
            if (length <= 0) throw new ArgumentException("OTP length must be positive.");

            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            int randomNumber = BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF;

            int otpValue = randomNumber % (int)Math.Pow(10, length);

            return otpValue.ToString(new string('0', length));
        }

        public static (string Hash, string Salt) HashOtp(string otp)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16);

            using var hmac = new HMACSHA256(saltBytes);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));

            return (Convert.ToBase64String(hash), Convert.ToBase64String(saltBytes));
        }

        public static bool VerifyOtp(string otp, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);

            using var hmac = new HMACSHA256(saltBytes);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));

            var computedHash = Convert.ToBase64String(hash);
            return computedHash == storedHash;
        }
    }
}
