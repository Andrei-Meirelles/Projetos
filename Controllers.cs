using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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

        public async Task<IActionResult> Post(DTO usuarioDto)

        {
            var Usuarionovo = new Usuario(usuarioDto.Email, usuarioDto.Senha);
            if (Usuarionovo == null || string.IsNullOrWhiteSpace(usuarioDto.Email) || string.IsNullOrWhiteSpace(usuarioDto.Senha))
            {
                return BadRequest();
            }
            _myContext.Usuarios.Add(Usuarionovo);
            await _myContext.SaveChangesAsync();
            return Ok(Usuarionovo);
        }
        [HttpPut("{Id}")]

        public async Task<IActionResult> Put(int Id, DTO usuarioDto)
        {
            var UsuarioEditar = await _myContext.Usuarios.FindAsync(Id);
            if (UsuarioEditar == null)
            {
                return NotFound();
            }
            UsuarioEditar.Email = usuarioDto.Email;
            UsuarioEditar.Senha = usuarioDto.Senha;
             await _myContext.SaveChangesAsync();
            return Ok(UsuarioEditar);

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
