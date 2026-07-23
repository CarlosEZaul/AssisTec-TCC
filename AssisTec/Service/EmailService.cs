using System;
using System.Security.Cryptography;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AssisTec.Service
{
    public class EmailService
    {
        private readonly string _remetenteEmail;
        private readonly string _remetenteNome;
        private readonly string _senhaApp;
        private readonly string _smtpServer;
        private readonly int _smtpPorta;

        public EmailService(string remetenteEmail, string senhaApp, string remetenteNome = "AssisTec", string smtpServer = "smtp.gmail.com", int smtpPorta = 587)
        {
            _remetenteEmail = remetenteEmail;
            _senhaApp = senhaApp;
            _remetenteNome = remetenteNome;
            _smtpServer = smtpServer;
            _smtpPorta = smtpPorta;
        }

        public string GerarCodigoVerificacao()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                uint randomNum = BitConverter.ToUInt32(bytes, 0);
                return (randomNum % 1000000).ToString("D6");
            }
        }

        public bool EnviarCodigoVerificacao(string emailDestino, string codigo)
        {
            try
            {
                string codigoExibicao = (codigo.Length == 6) 
                    ? $"{codigo.Substring(0, 3)}-{codigo.Substring(3, 3)}" 
                    : codigo;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_remetenteNome, _remetenteEmail));
                message.To.Add(new MailboxAddress("", emailDestino));
                message.Subject = "Código de Verificação - AssisTec";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif; padding: 20px; color: #333;'>
                            <h2 style='color: #0056b3;'>Redefinição de Senha</h2>
                            <p>Utilize o código abaixo para redefinir sua senha no sistema AssisTec:</p>
                            <div style='background-color: #f4f4f4; padding: 10px 20px; display: inline-block; border-radius: 5px; margin: 10px 0;'>
                                <h1 style='color: #0056b3; letter-spacing: 5px; margin: 0;'>{codigoExibicao}</h1>
                            </div>
                            <p style='font-size: 12px; color: #777;'>Se você não solicitou este código, ignore este e-mail.</p>
                        </div>"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(_smtpServer, _smtpPorta, SecureSocketOptions.StartTls);
                    client.Authenticate(_remetenteEmail, _senhaApp);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                return false;
            }
        }
    }
}