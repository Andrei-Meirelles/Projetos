using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoMIragnum.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ProjetoMIragnum.Service
{
    public class UsuarioService
    {
        private readonly MyDbContext _myContext;
        private readonly IConfiguration _configuration;

        public UsuarioService(MyDbContext context, IConfiguration configuration)
        {

            _myContext = context;
            _configuration = configuration;
        }

        public async Task<List<DtoUsuarioResponse>> Get(
          int page = 1,

          int pageSize = 10)
        {
            int skip = (page - 1) * pageSize;

            var getting = await _myContext.Usuarios.Skip(skip).Take(pageSize).ToListAsync();
           


            var usuariosemsenha = getting.Select(u => new DtoUsuarioResponse
            {
                Id = u.Id,
                Email = u.Email,




            }).ToList();
            {


            }
            return usuariosemsenha;
        }

            public async Task<string> login(LoginDto login)
        {
            {

                // 1 - Procurar o usuário
                var usuario = await _myContext.Usuarios.FirstOrDefaultAsync(u => u.Email == login.Email);

                // 2 - Verificar se existe
                if (usuario == null)
                {
                    return null!;
                }

                // 3 - Verificar a senha
                bool senhaCorreta = BCrypt.Net.BCrypt.Verify(login.Senha, usuario.Senha);

                if (!senhaCorreta)
                {
                    return null!;
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



                return jwt;




            }
        }


        public async Task<DtoUsuarioResponse> Post(DTORequest usuarioDto)

        {

            bool emailExist = await _myContext.Usuarios.AnyAsync(u => u.Email == usuarioDto.Email);
            if (emailExist)
            {
                return null!;
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



            return usuariosemsenha;
        }

             public async Task<DtoUsuarioResponse> Put(int Id, DTORequest usuarioDto)
        {
            var UsuarioEditar = await _myContext.Usuarios.FindAsync(Id);
            
            string HashSenha = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha);

            UsuarioEditar!.Email = usuarioDto.Email;
            UsuarioEditar.Senha = HashSenha;
            await _myContext.SaveChangesAsync();

            var usuariosemsenha = new DtoUsuarioResponse
            {
                Id = UsuarioEditar.Id,
                Email = UsuarioEditar.Email
            };
            return usuariosemsenha;


           

        }
        public async Task<string> Delete(int Id)
        {
            string mensagem = "Usuario deletado";
            var UsuarioDeletado = await _myContext.Usuarios.FindAsync(Id);
          
            _myContext.Usuarios.Remove(UsuarioDeletado!);
            await _myContext.SaveChangesAsync();

            return mensagem;

           

        }

        public async Task<DtoUsuarioResponse> GetporId(int Id)
        {
            var usuarioPorId = await _myContext.Usuarios.FindAsync(Id);

            var usuariosemsenha = new DtoUsuarioResponse
            {
                Id = usuarioPorId!.Id,
                Email = usuarioPorId.Email
            };

            return usuariosemsenha;
        }


    }



}

    

