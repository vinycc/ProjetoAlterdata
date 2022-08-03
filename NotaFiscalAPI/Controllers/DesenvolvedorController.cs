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
        public static List<Desenvolvedor> listaDesenvolvedores = new List<Desenvolvedor>();

        private readonly DataContext _context;

        public DesenvolvedorController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Desenvolvedor>>> Get()
        {
            //Fetch the JSON string from URL.
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string json = (new WebClient()).DownloadString("https://61a170e06c3b400017e69d00.mockapi.io/DevTest/Dev");

            var devs = JsonConvert.DeserializeObject<IEnumerable<Desenvolvedor>>(json);
            listaDesenvolvedores.AddRange(devs);


            return Ok(listaDesenvolvedores);

        }

        
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

        [HttpGet]
        [Route("DataBase/")]
        public async Task<ActionResult<List<Desenvolvedor>>> GetfromDB()
        {

            return Ok(await _context.Desenvolvedores.ToListAsync());

        }


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

        [HttpPost]
        [Route("DataBase/")]
        public async Task<ActionResult<List<Desenvolvedor>>> AddDesenvolvedor(Desenvolvedor dev)
        {
            _context.Desenvolvedores.Add(dev);
            await _context.SaveChangesAsync();

            return Ok(await _context.Desenvolvedores.ToListAsync());

            //listaDesenvolvedores.Add(dev);
            //return Ok(listaDesenvolvedores);
        }

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
