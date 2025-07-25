using Newtonsoft.Json;

namespace AssisTec
{
    public class WebResponse
    {
        [JsonProperty("cep")]
        public string cep { get; set; }
        
        [JsonProperty("logradouro")]
        public string rua { get; set; }
        
        [JsonProperty("Complemento")]
        public string complemento { get; set; }
        
        [JsonProperty("bairro")]
        public string bairro { get; set; }
        
        [JsonProperty("localidade")]
        public string cidade { get; set; }
        
        [JsonProperty("estado")]
        public string estado { get; set; }
        
        [JsonProperty("uf")]
        public string uf { get; set; }
    }
}