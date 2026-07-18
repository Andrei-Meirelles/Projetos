using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using BCrypt;

namespace ProjetoMIragnum
{


    [ApiController]
    [Route("api/[Controller]")]
    public class MiragController : ControllerBase
    {
        private readonly MyDbContext _myContext;

        public MiragController(MyDbContext context)
        {
            _myContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var getting = await _myContext.Usuarios.ToListAsync();
            if (getting.Count == 0)
            {
                return NotFound();
            }
            return Ok(getting);

        }
        [HttpPost]

        public async Task<IActionResult> Post(DTORequest usuarioDto)

        {
            if(string.IsNullOrWhiteSpace(usuarioDto.Email) || string.IsNullOrWhiteSpace(usuarioDto.Senha))
            {
                return BadRequest();

            }

            string Senhahash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha);
                
            var Usuarionovo = new Usuario(usuarioDto.Email, Senhahash);

            _myContext.Usuarios.Add(Usuarionovo);

           


            var usuariosemsenha = new DtoUsuarioResponse
            {
                Id = Usuarionovo.Id,
                Email = Usuarionovo.Email
            };
            await _myContext.SaveChangesAsync();


            return Ok(usuariosemsenha);
               
            
        }
        [HttpPut("{Id}")]

        public async Task<IActionResult> Put(int Id, DTORequest usuarioDto)
        {
            var UsuarioEditar = await _myContext.Usuarios.FindAsync(Id);
            if (UsuarioEditar == null)
            {
                return NotFound();
            }
            string HashSenha = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha);

            UsuarioEditar.Email = usuarioDto.Email;
            UsuarioEditar.Senha = HashSenha;
            await _myContext.SaveChangesAsync();

            var usuariosemsenha = new DtoUsuarioResponse
            {
                Id = UsuarioEditar.Id,
                Email = UsuarioEditar.Email
            };


            return Ok();

        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var UsuarioDeletado = await _myContext.Usuarios.FindAsync(Id);
            if (UsuarioDeletado == null)
            {
                return NotFound();
            }
             _myContext.Usuarios.Remove(UsuarioDeletado);
            await _myContext.SaveChangesAsync();
            return Ok("Usuario deletado.");

        }

        [HttpGet("{Id}")]

        public async Task<IActionResult> GetporId(int Id)
        {
            var usuarioPorId = await _myContext.Usuarios.FindAsync(Id);
            if (usuarioPorId == null)
            {
                return NotFound();
            }
            return Ok(usuarioPorId);
        }

      


    }

    
}
