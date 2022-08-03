using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NotaFiscalAPI
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    [Keyless]
    public class Dest
    {
        public string CPF { get; set; }
        public string xNome { get; set; }
        public EnderDest enderDest { get; set; }
        public string indIEDest { get; set; }
    }

    public class Det
    {
        [JsonProperty("@nItem")]
        public int NItem { get; set; }
        public Prod prod { get; set; }
    }

    public class Emit
    {
        public string CNPJ { get; set; }
        public string xNome { get; set; }
        public string xFant { get; set; }
        public EnderEmit enderEmit { get; set; }
        public string IE { get; set; }
        public string CRT { get; set; }
    }

    public class EnderDest
    {
        public string xLgr { get; set; }
        public int nro { get; set; }
        public string xCpl { get; set; }
        public string xBairro { get; set; }
        public string cMun { get; set; }
        public string xMun { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string cPais { get; set; }
        public string xPais { get; set; }
        public string fone { get; set; }
    }

    public class EnderEmit
    {
        public string xLgr { get; set; }
        public int nro { get; set; }
        public string xBairro { get; set; }
        public string cMun { get; set; }
        public string xMun { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string cPais { get; set; }
        public string xPais { get; set; }
    }

    public class Ide
    {
        public string cUF { get; set; }
        public string cNF { get; set; }
        public string natOp { get; set; }
        public string mod { get; set; }
        public string serie { get; set; }
        public string nNF { get; set; }
        public DateTime dhEmi { get; set; }
        public string tpNF { get; set; }
        public string idDest { get; set; }
        public string cMunFG { get; set; }
        public string tpImp { get; set; }
        public string tpEmis { get; set; }
        public string cDV { get; set; }
        public string tpAmb { get; set; }
        public string finNFe { get; set; }
        public string indFinal { get; set; }
        public string indPres { get; set; }
        public string procEmi { get; set; }
        public string verProc { get; set; }
        public DateTime dhSaiEnt { get; set; }
    }

    [Keyless]
    public class InfNFe
    {
        [JsonProperty("@Id")]
        public string Id { get; set; }

        [JsonProperty("@versao")]
        public string Versao { get; set; }
        public Ide ide { get; set; }
        public Emit emit { get; set; }
        public Dest dest { get; set; }
        public List<Det> det { get; set; }
    }

    public class NFe
    {
        public InfNFe infNFe { get; set; }
    }

    public class NfeProc
    {
        [JsonProperty("@versao")]
        public string Versao { get; set; }

        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }
        public NFe NFe { get; set; }
    }

    public class Prod
    {
        public string cProd { get; set; }
        public string cEAN { get; set; }
        public string xProd { get; set; }
        public int NCM { get; set; }
        public int CFOP { get; set; }
        public string uCom { get; set; }
        public int qCom { get; set; }
        public int vUnCom { get; set; }
        public int vProd { get; set; }
        public int cEANTrib { get; set; }
        public string uTrib { get; set; }
        public int qTrib { get; set; }
        public int vUnTrib { get; set; }
        public string indTot { get; set; }
        public string preco { get; set; }
        public string marca { get; set; }
    }

    public class NotaFiscal
    {
        public NfeProc nfeProc { get ; set; }
        public string id { get; set; }
        public int quantidadeAlterados { get; set; }
    }



}
