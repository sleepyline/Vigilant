using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using VigiLant.Contratos;


namespace VigiLant.Services
{
    public class HashService : IHashService
    {
        private const int SaltSize = 16;        
        private const int KeySize = 32;          
        private const int Iterations = 10000;   

        public string GerarHash(string senha)
        {
            // 1. Gerar um salt aleatório
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // 2. Gerar o hash da senha (key)
            byte[] hash = KeyDerivation.Pbkdf2(
                password: senha,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: KeySize);

            byte[] hashBytes = new byte[1 + SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, hashBytes, 1, SaltSize);
            Buffer.BlockCopy(hash, 0, hashBytes, 1 + SaltSize, KeySize);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerificarHash(string senhaDigitada, string senhaHashArmazenada)
        {
            byte[] hashBytes;
            try
            {
                hashBytes = Convert.FromBase64String(senhaHashArmazenada);
            }
            catch (FormatException)
            {
                return false;
            }

            if (hashBytes.Length != 1 + SaltSize + KeySize)
            {
                return false;
            }
            
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 1, salt, 0, SaltSize);
            
            byte[] storedHash = new byte[KeySize];
            Buffer.BlockCopy(hashBytes, 1 + SaltSize, storedHash, 0, KeySize);

            byte[] currentHash = KeyDerivation.Pbkdf2(
                password: senhaDigitada,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: KeySize);


            return CryptographicOperations.FixedTimeEquals(currentHash, storedHash);
        }
    }
}