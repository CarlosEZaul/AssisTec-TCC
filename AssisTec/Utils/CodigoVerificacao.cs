using System;
using System.Runtime.Caching;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using MemoryCache = System.Runtime.Caching.MemoryCache;

namespace AssisTec.Service
{
    public static class CodigoVerificacao
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;

        public static string GerarESalvar(string email)
        {
            string codigo = GerarCodigoCriptografico();

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            string chaveCache = ObterChaveCache(email);
            Cache.Set(chaveCache, codigo, policy);

            return codigo;
        }

        public static bool Validar(string email, string codigoDigitado)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(codigoDigitado))
            {
                return false;
            }

            string chaveCache = ObterChaveCache(email);
            string codigoSalvo = Cache.Get(chaveCache) as string;

            if (codigoSalvo == null)
            {
                return false;
            }

            if (string.Equals(codigoSalvo, codigoDigitado, StringComparison.Ordinal))
            {
                Cache.Remove(chaveCache);
                return true;
            }

            return false;
        }

        private static string GerarCodigoCriptografico()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                uint randomNum = BitConverter.ToUInt32(bytes, 0);
                return (randomNum % 1000000).ToString("D6");
            }
        }

        private static string ObterChaveCache(string email)
        {
            return $"codigo_verificacao:{email.ToLowerInvariant().Trim()}";
        }
        
        public static bool ValidarSemRemover(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            string chaveCache = ObterChaveCache(email);
            return Cache.Contains(chaveCache);
        }
    }
}