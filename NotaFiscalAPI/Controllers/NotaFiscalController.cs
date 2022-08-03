using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace NotaFiscalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotaFiscalController : ControllerBase
    {

        public static List<NotaFiscal> notasFiscais = new List<NotaFiscal>();

        [HttpGet]
        public async Task<ActionResult<List<NotaFiscal>>> Get()
        {
            //Fetch the JSON string from URL.
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string json = (new WebClient()).DownloadString("https://61a170e06c3b400017e69d00.mockapi.io/DevTest/invoice");

            var nfs = JsonConvert.DeserializeObject<IEnumerable<NotaFiscal>>(json);
            notasFiscais.AddRange(nfs);

            int quantidadeAlterada = 0;
            // Corrigir o valor contido em "nfeProc.NFe.infNFe.ide.det.prod.marca" utilizando o
            // campo "nfeProc.NFe.infNFe.ide.det.prod.xProd"
            foreach (var nota in notasFiscais)
            {
                bool alterado = false;
                for (int i = 0; i < nota.nfeProc.NFe.infNFe.det.Count(); i++)
                {
                    // Indicar quantos documentos necessitaram de ajuste em função de erro na marca
                    if (nota.nfeProc.NFe.infNFe.det[i].prod.marca != nota.nfeProc.NFe.infNFe.det[i].prod.xProd)
                    {
                        alterado = true;
                        
                    }
                    nota.nfeProc.NFe.infNFe.det[i].prod.marca = nota.nfeProc.NFe.infNFe.det[i].prod.xProd;
                }
                if (alterado)
                {
                    quantidadeAlterada += 1;
                }
            }

            return Ok(notasFiscais);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotaFiscal>> Get(int id)
        {
            //Fetch the JSON string from URL.
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            
            try
            {
                string json = (new WebClient()).DownloadString("https://61a170e06c3b400017e69d00.mockapi.io/DevTest/invoice/" + id);
                var nf = JsonConvert.DeserializeObject<NotaFiscal>(json);
                return Ok(nf);
            }
            catch(Exception)
            {
                return BadRequest("NF não encontrada");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<NotaFiscal>>> AddNotaFiscal(NotaFiscal nf)
        {
            notasFiscais.Add(nf);
            return Ok(notasFiscais);
        }
    }
}
