using System.ComponentModel.DataAnnotations;

namespace ProjetoMIragnum
{
    public class DTORequest
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string Senha { get; set; } = string.Empty;

      


    }

    public class DtoUsuarioResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
