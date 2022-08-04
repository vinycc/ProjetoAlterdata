/*
 
 * TODO:
 
 * Manter sua solução com dados sincronizados com a origem dos dados
 
 * Há um problema no cadastro de DEVs atual, os e-mails deveriam ter domínio '@prosoft.com.br', realize essa correção
 * Está validando apenas novos DEVs. Não está atualizando os já cadastrados.

 */ 


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
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

        // Validar email e verificar se dominio está correto
        static bool validateEmail(string email)
        {
            if (email == null)
            {
                return false;
            }

            if (new EmailAddressAttribute().IsValid(email) && email.EndsWith("@prosoft.com.br"))
            {
                return true;
            }
            else
            {

                return false;
            }
        }


        // Obter todos os DEVs cadastrados no WebService
        [HttpGet]
        public async Task<ActionResult<List<Desenvolvedor>>> Get()
        {
            List<Desenvolvedor> listaDesenvolvedores = new List<Desenvolvedor>();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string json = (new WebClient()).DownloadString("https://61a170e06c3b400017e69d00.mockapi.io/DevTest/Dev");

            var devs = JsonConvert.DeserializeObject<IEnumerable<Desenvolvedor>>(json);
            listaDesenvolvedores.AddRange(devs);

            return Ok(listaDesenvolvedores);
        }


        // Busca um DEV específico no WebService
        [HttpGet("{id}")]
        public async Task<ActionResult<Desenvolvedor>> Get(string id)
        {
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


        // Obter todos os DEVs cadastrados no Banco de Dados
        [HttpGet]
        [Route("DataBase/")]
        public async Task<ActionResult<List<Desenvolvedor>>> GetfromDB()
        {
            return Ok(await _context.Desenvolvedores.ToListAsync());
        }


        // Obter um determinado DEV no banco de dados
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


        // Inserir um novo DEV
        [HttpPost]
        [Route("DataBase/")]
        public async Task<ActionResult<List<Desenvolvedor>>> AddDesenvolvedor(Desenvolvedor dev)
        {
            if (validateEmail(dev.email))
            {
                _context.Desenvolvedores.Add(dev);
                await _context.SaveChangesAsync();

                return Ok(await _context.Desenvolvedores.ToListAsync());
            }
            else
            {
                return BadRequest("Email invalido");
            }
        }


        // Atualizar dado de DEV específico
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


        // Exclui DEV específico do Banco de Dados
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
