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

namespace ProjetoMIragnum.NovaPasta
{


    [ApiController]
    [Route("api/[Controller]")]
    public class MiragController : ControllerBase
    {
        private readonly MyDbContext _myContext;
        private readonly IConfiguration _configuration;

        public MiragController(MyDbContext context, IConfiguration configuration)
        {
           
            _myContext = context;
            _configuration = configuration;
        }
     

        [HttpPost("Login")]
        public IActionResult login(LoginDto login)
        {

            // 1 - Procurar o usuário
            var usuario = _myContext.Usuarios
                .FirstOrDefault(u => u.Email == login.Email);

            // 2 - Verificar se existe
            if (usuario == null)
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            // 3 - Verificar a senha
            bool senhaCorreta = BCrypt.Net.BCrypt.Verify(login.Senha, usuario.Senha);

            if (!senhaCorreta)
            {
                return Unauthorized("Email ou senha inválidos.");
            }
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
        new Claim(ClaimTypes.Name, usuario.Email),
        new Claim(ClaimTypes.Role, usuario.Cargo)
    }),

                Expires = DateTime.UtcNow.AddHours(2),

                Issuer = _configuration["Jwt:Issuer"],

                Audience = _configuration["Jwt:Audience"],

                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = tokenHandler.WriteToken(token);



            return Ok(new
            {
                token = jwt
            });

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get(
            int page = 1,
            
            int pageSize = 10)
        {
            int skip = (page - 1) * pageSize;
           
            var getting = await _myContext.Usuarios.Skip(skip).Take(pageSize).ToListAsync();
            if (getting.Count == 0)
            {
                return NotFound();
            }
            var usuariosemsenha = getting.Select(u => new DtoUsuarioResponse
            {
                Id = u.Id,
                Email = u.Email,
               

            



            }).ToList();
            {
                

            };
            
            return Ok(usuariosemsenha);

        }
        [HttpPost]

        public async Task<IActionResult> Post(DTORequest usuarioDto)

        {
            if(string.IsNullOrWhiteSpace(usuarioDto.Email) || string.IsNullOrWhiteSpace(usuarioDto.Senha))
            {
                return BadRequest();

            }
            bool emailExist = await _myContext.Usuarios.AnyAsync(u => u.Email == usuarioDto.Email); 
            if (emailExist)
            {
                return BadRequest("Esse Email ja existe");
            }

            string Senhahash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha);
                
            var Usuarionovo = new Usuario(usuarioDto.Email, Senhahash);

            _myContext.Usuarios.Add(Usuarionovo);
            await _myContext.SaveChangesAsync();



            var usuariosemsenha = new DtoUsuarioResponse
            {
                Id = Usuarionovo.Id,
                Email = Usuarionovo.Email
                
            };
           


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


            return Ok("Usuario atualizado");

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var UsuarioDeletado = await _myContext.Usuarios.FindAsync(Id);
            if (UsuarioDeletado == null)
            {
                return NotFound();
            }
            else if (User.IsInRole("Admin"))
            {
                return BadRequest("Um administrador não pode apagar outro administrador.");
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
