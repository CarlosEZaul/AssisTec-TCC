using System;
using System.Drawing.Imaging;
using System.IO;
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

                var bodyBuilder = new BodyBuilder();
                string contentId = "logo_assistec_cid";
                
                using (var logo = Properties.Resources.logopng)
                using (var stream = new MemoryStream())
                {
                    logo.Save(stream, ImageFormat.Png);
                    byte[] bytesImagem = stream.ToArray();

                    MimeEntity image = bodyBuilder.LinkedResources.Add("logo.png", bytesImagem, ContentType.Parse("image/png"));
                    image.ContentId = contentId;
                    image.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);
                }

                bodyBuilder.HtmlBody = $@"
                <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background-color: #0d0d0d; padding: 40px 0;'>
                    <tr>
                        <td align='center'>
                            <table role='presentation' width='400' cellpadding='0' cellspacing='0' style='background-color: #1a1a1a; border-radius: 12px; padding: 40px 30px; font-family: Arial, sans-serif; text-align: center; box-shadow: 0 4px 20px rgba(0,0,0,0.4);'>
                                <tr>
                                    <td align='center' style='padding-bottom: 20px;'>
                                        <img src='cid:{contentId}' alt='Logo AssisTec' style='max-width: 160px; height: auto;' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align='center'>
                                        <h2 style='color: #4da3ff; margin: 0 0 10px 0; font-weight: 600;'>Código de Verificação</h2>
                                        <p style='color: #cccccc; font-size: 14px; margin: 0 0 20px 0;'>
                                            Utilize o código abaixo no sistema AssisTec:
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='center'>
                                        <div style='background-color: #262626; padding: 15px 30px; display: inline-block; border-radius: 8px; border: 1px solid #333;'>
                                            <h1 style='color: #4da3ff; letter-spacing: 8px; margin: 0; font-size: 32px;'>{codigoExibicao}</h1>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='center' style='padding-top: 20px;'>
                                        <p style='color: #888888; font-size: 13px; margin: 0;'>
                                            Este código expira em <strong>30 minutos</strong>.
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='center' style='padding-top: 15px;'>
                                        <p style='font-size: 12px; color: #777777; margin: 0;'>
                                            Se você não solicitou este código, ignore este e-mail.
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>";

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