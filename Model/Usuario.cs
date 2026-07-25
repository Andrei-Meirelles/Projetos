namespace ProjetoMIragnum
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;

        public string Cargo { get; set; } = "Usuario";

        public Usuario(string email, string senha, string cargo)
        {
            Email = email;
            Senha = senha;
            Cargo = cargo;

        }
    }

}
