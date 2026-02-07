using System.Data;

public class BuscaCEP
{
    private string CEP;
    private string Rua;
    private string Cidade;
    private string Estado;
    private string Bairro;
    

    public string cep
    {
        get { return CEP; }
        set { CEP = value; }
    }

    public string rua
    {
        get { return Rua; }
        set { Rua = value; }
    }

    public string cidade
    {
        get { return Cidade; }
        set { Cidade = value; }
    }

    public string estado
    {
        get { return Estado; }
        set { Estado = value; }
    }

    public string bairro
    {
        get { return Bairro; }
        set { Bairro = value; }
    }
    


    public DataSet Consultar()
    {
        if (string.IsNullOrEmpty(cep))
            return null;

        
        string xml = $"http://cep.republicavirtual.com.br/web_cep.php?cep={cep}&formato=xml";
        
        DataSet ds = new DataSet();
        ds.ReadXml(xml);
        
        rua = ds.Tables[0].Rows[0]["logradouro"].ToString();
        cidade = ds.Tables[0].Rows[0]["cidade"].ToString();
        bairro = ds.Tables[0].Rows[0]["bairro"].ToString();
        estado = ds.Tables[0].Rows[0]["uf"].ToString();

        return ds;
    }
}