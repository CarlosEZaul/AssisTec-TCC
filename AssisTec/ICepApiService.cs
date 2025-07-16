using System.Threading.Tasks;
using System.Net;
using Refit;

namespace AssisTec
{
    public interface ICepApiService
    {
        [Get("/ws/{cep}/json")]
        Task<WebResponse> GetAdressAsync(string cep);
    }
}