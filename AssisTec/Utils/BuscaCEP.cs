using System;
using System.Data;

public class BuscaCEP
    {
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Bairro { get; set; }

        public DataSet Consultar()
        {
            if (string.IsNullOrWhiteSpace(Cep))
                return null;

            string cepLimpo = Cep.Replace("-", "").Replace(".", "").Trim();
            if (cepLimpo.Length != 8)
                return null;

            string urlXml = "http://cep.republicavirtual.com.br/web_cep.php?cep=" + cepLimpo + "&formato=xml";
            
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(urlXml);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow resultado = ds.Tables[0].Rows[0];
                    
                    string resultadoCod = resultado["resultado"].ToString();

                    if (resultadoCod == "1" || resultadoCod == "2")
                    {
                        string tipoLogradouro = resultado["tipo_logradouro"].ToString().Trim();
                        string logradouro = resultado["logradouro"].ToString().Trim();
                        
                        Rua = (tipoLogradouro + " " + logradouro).Trim();
                        Bairro = resultado["bairro"].ToString().Trim();
                        Cidade = resultado["cidade"].ToString().Trim();
                        Estado = resultado["uf"].ToString().Trim();

                        return ds;
                    }
                }
                
                LimparCampos();
                return null;
            }
            catch (Exception ex)
            {
                LimparCampos();
                throw new Exception("Falha de conexão ao consultar o CEP externo: " + ex.Message, ex);
            }
        }

        private void LimparCampos()
        {
            Rua = string.Empty;
            Bairro = string.Empty;
            Cidade = string.Empty;
            Estado = string.Empty;
        }
    }