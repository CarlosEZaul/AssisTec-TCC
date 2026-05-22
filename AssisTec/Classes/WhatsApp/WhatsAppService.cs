using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AssisTec.WhatsApp
{
    public class WhatsAppService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<bool> EnviarMensagemPadraoAsync()
        {
            var payload = new
            {
                messaging_product = "whatsapp",
                to   = WhatsAppConfig.Destinatario,
                type = "text",
                text = new { body = "Teste de mensagem!" }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://graph.facebook.com/v19.0/{WhatsAppConfig.PhoneNumberId}/messages");

            request.Headers.Add("Authorization", $"Bearer {WhatsAppConfig.AccessToken}");
            request.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}