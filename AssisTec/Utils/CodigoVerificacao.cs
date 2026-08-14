using System;
using System.Collections.Concurrent;

namespace AssisTec.Service
{
    public static class CodigoVerificacao
    {
        public class InfoCodigo
        {
            public string Codigo { get; set; }
            public DateTime DataCriacao { get; set; }
        }

        private static readonly ConcurrentDictionary<string, InfoCodigo> _codigos = new ConcurrentDictionary<string, InfoCodigo>();

        public static string GerarESalvar(string email)
        {
            Random rand = new Random();
            string codigo = rand.Next(100000, 999999).ToString();

            var info = new InfoCodigo
            {
                Codigo = codigo,
                DataCriacao = DateTime.Now
            };

            _codigos[email.ToLower()] = info;
            return codigo;
        }

        public static InfoCodigo ObterInfo(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            if (_codigos.TryGetValue(email.ToLower(), out var info))
            {
                return info;
            }

            return null;
        }

        public static (bool valido, string mensagem) ValidarCodigo(string email, string codigoDigitado)
        {
            if (string.IsNullOrEmpty(email) || !_codigos.TryGetValue(email.ToLower(), out var info))
            {
                return (false, "Nenhum código foi solicitado para este e-mail.");
            }

            TimeSpan tempoDecorrido = DateTime.Now - info.DataCriacao;

            if (tempoDecorrido.TotalHours > 2)
            {
                _codigos.TryRemove(email.ToLower(), out _);
                return (false, "O código expirou (limite de 2 horas). Solicite um novo código.");
            }

            if (info.Codigo == codigoDigitado)
            {
                _codigos.TryRemove(email.ToLower(), out _);
                return (true, "Código validado com sucesso.");
            }

            return (false, "Código incorreto. Tente novamente.");
        }
    }
}