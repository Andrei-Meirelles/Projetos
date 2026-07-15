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

        public async Task<IActionResult> Post(Usuario usuario)

        {
            var Usuarionovo = new Usuario(usuario.Email, usuario.Senha);
            if (Usuarionovo == null || string.IsNullOrWhiteSpace(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Senha))
            {
                return BadRequest();
            }
            _myContext.Usuarios.Add(Usuarionovo);
            await _myContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{Id}")]

        public async Task<IActionResult> Put(int Id, Usuario usuarioNovo)
        {
            var UsuarioEditar = await _myContext.Usuarios.FindAsync(Id);
            if (UsuarioEditar == null)
            {
                return NotFound();
            }
            UsuarioEditar.Email = usuarioNovo.Email;
            UsuarioEditar.Senha = usuarioNovo.Senha;
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
            return NoContent();

        }

    }

    
}
