using System;
using System.Security.Cryptography;

namespace SystemSazek.Core.Sazky{
    public class PasswordHasher
    {


        public static (string hashed, string salt) ZahashujHeslo(string password)
        {
            byte[] salt = new byte[16];

            using (var rngServiceProvider = new RNGCryptoServiceProvider())
            {
                rngServiceProvider.GetBytes(salt);
            }

            using (var pbkdf2_hashing = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2_hashing.GetBytes(20);
                return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
            }
        }

        public static bool OvereniHesla(string password, string storedSalt, string storedHash)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            byte[] storedHashBytes = Convert.FromBase64String(storedHash);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] newHash = pbkdf2.GetBytes(20);

                for (int i = 0; i < 20; i++)
                {
                    if (newHash[i] != storedHashBytes[i])
                        return false;
                }
            }

            return true;
        }
    }
}

