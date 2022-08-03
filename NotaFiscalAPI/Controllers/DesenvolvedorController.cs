using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace NotaFiscalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesenvolvedorController : ControllerBase
    {
        
        private readonly DataContext _context;

        public DesenvolvedorController(DataContext context)
        {
            this._context = context;
        }


        // Busca todos os desenvolvedores cadastrados no Banco de Dados.
        [HttpGet]
        public async Task<ActionResult<List<Desenvolvedor>>> Get()
        {
            List<Desenvolvedor> listaDesenvolvedores = new List<Desenvolvedor>();


            //Fetch the JSON string from URL.
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string json = (new WebClient()).DownloadString("https://61a170e06c3b400017e69d00.mockapi.io/DevTest/Dev");

            var devs = JsonConvert.DeserializeObject<IEnumerable<Desenvolvedor>>(json);
            listaDesenvolvedores.AddRange(devs);


            return Ok(listaDesenvolvedores);

        }

        
        // Busca um desenvolvedor do WebService
        [HttpGet("{id}")]
        public async Task<ActionResult<Desenvolvedor>> Get(string id)
        {
            //Fetch the JSON string from URL.
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                string json = (new WebClient()).DownloadString("https://61a170e06c3b400017e69d00.mockapi.io/DevTest/Dev/" + id);
                var dev = JsonConvert.DeserializeObject<Desenvolvedor>(json);

                return Ok(dev);
            }
            catch (Exception)
            {
                return BadRequest("Desenvolvedor não encontrado");
            }
        }


        // Busca todos os desenvolvedores cadastrados no banco de dados
        [HttpGet]
        [Route("DataBase/")]
        public async Task<ActionResult<List<Desenvolvedor>>> GetfromDB()
        {

            return Ok(await _context.Desenvolvedores.ToListAsync());

        }


        // Busca um determinado desenvolvedor do banco de dados
        [HttpGet("DataBase/{id}")]
        public async Task<ActionResult<Desenvolvedor>> GetfromDB(string id)
        {

            var dev = await _context.Desenvolvedores.FindAsync(id);

            if (dev == null)
            {
                return BadRequest("Desenvolvedor não encontrado");
            }

            return Ok(dev);

        }

        // Insere desenvolvedor do banco de dodos
        [HttpPost]
        [Route("DataBase/")]
        public async Task<ActionResult<List<Desenvolvedor>>> AddDesenvolvedor(Desenvolvedor dev)
        {
            _context.Desenvolvedores.Add(dev);
            await _context.SaveChangesAsync();

            return Ok(await _context.Desenvolvedores.ToListAsync());

        }


        // Altera dados do desenvolvedor
        [HttpPut]
        [Route("DataBase/")]
        public async Task<ActionResult<List<Desenvolvedor>>> UpdateDesenvolvedor(Desenvolvedor request)
        {
            var dbDev = await _context.Desenvolvedores.FindAsync(request.id);
            if (dbDev == null)
            {
                return BadRequest("Desenvolvedor não encontrado");
            }

            dbDev.name = request.name;
            dbDev.createdAt = request.createdAt;
            dbDev.avatar = request.avatar;
            dbDev.squad = request.squad;
            dbDev.login = request.login;
            dbDev.login = request.login;

            await _context.SaveChangesAsync();

            return Ok(await _context.Desenvolvedores.ToListAsync());
        }


        // Exclui Desenvolvedor do Banco de Dados
        [HttpDelete]
        [Route("DataBase/")]
        public async Task<ActionResult<Desenvolvedor>> DeleteDesenvolvedor(string id)
        {

            var dbDev = await _context.Desenvolvedores.FindAsync(id);

            if (dbDev == null)
            {
                return BadRequest("Desenvolvedor não encontrado");
            }

            _context.Desenvolvedores.Remove(dbDev);
            await _context.SaveChangesAsync();

            return Ok(await _context.Desenvolvedores.ToListAsync());

        }

    }
}
