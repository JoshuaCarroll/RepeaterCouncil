using Microsoft.AspNetCore.Identity;
using RepeaterCouncil.Data;
using System.Security.Cryptography;
using System.Text;

namespace RepeaterCouncil.Services
{
    public class CustomPasswordHasher : IPasswordHasher<IdentityUser>
    {
        private readonly ApplicationDbContext _context;

        public CustomPasswordHasher(ApplicationDbContext context)
        {
            _context = context;
        }

        public string HashPassword(IdentityUser user, string password)
        {
            throw new NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            // Retrieve the salt dynamically from the Keys table at runtime
            var salt = _context.Keys.SingleOrDefault()?.Key1;

            string saltedInput = providedPassword + user.UserName.ToUpper() + salt;

            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedInput));
                string hashString = Convert.ToBase64String(hashBytes);

                if (hashedPassword == hashString)
                {
                    return PasswordVerificationResult.Success;
                }
            }

            return PasswordVerificationResult.Failed;
        }
    }
}
