using practise.DTO;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace practise.Repository
{
    public class HashService
    {
        public HashResult Hash(string data)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            return Hash(data,salt);

        }
        public HashResult Hash(string data,byte[] salt)
        {

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: data,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
                ));
            return new HashResult
            {
                Hash= hashed,
                Salt= salt
            };
        }

    }
}
