using BCrypt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using ProjetoMIragnum.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProjetoMIragnum.Service;
using System.ComponentModel.DataAnnotations;

namespace ProjetoMIragnum.Service
{


    [ApiController]
    [Route("api/[Controller]")]
    public class MiragController : ControllerBase
    {
        private readonly UsuarioService _usuarioservice;

        public MiragController(UsuarioService usuarioservice)
        {
            _usuarioservice = usuarioservice;
        }
     

        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDto login)
        {

            var token = await _usuarioservice.login(login);
            if (token == null)
            {
                return BadRequest("Email ou senha inválidos");
            }
            return Ok(new
            {
                token = token
            });

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuario = await _usuarioservice.Get();
            if (usuario.Count == 0)
            {
                return NotFound("Lista vazia");
            }
            return Ok(usuario);

        }
      
        [HttpPost]

        public async Task<IActionResult> Post(DTORequest usuarioDto)

        {
            var Postar = await _usuarioservice.Post(usuarioDto);
          
            if (Postar == null)
            {
                return BadRequest("Esse email ja existe");
            }

            return Ok(Postar);
               
            
        }

        [HttpPut("{Id}")]

        public async Task<IActionResult> Put(int Id, DTORequest usuarioDto)
        {
           var putting = await _usuarioservice.Put(Id, usuarioDto);
            if (putting == null)
            {
                return BadRequest("Usuario não encontrado");
            }

            return Ok("Usuario atualizado");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var Deletar = await _usuarioservice.Delete(Id);

            if (Deletar == null)
            {
                return NotFound();
            }
            else if (User.IsInRole("Admin"))
            {
                return BadRequest("Um administrador não pode apagar outro administrador.");
            }
            
            return Ok("Usuario deletado.");

        }

        [HttpGet("{Id}")]

        public async Task<IActionResult> GetporId(int Id)
        {
          var GetId = await _usuarioservice.GetporId(Id);
            if (GetId == null)
            {
                return NotFound();
            }
            return Ok(GetId);
        }




      


    }

    
}
