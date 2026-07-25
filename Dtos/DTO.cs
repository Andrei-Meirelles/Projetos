using System.ComponentModel.DataAnnotations;

namespace ProjetoMIragnum
{
    public class DTORequest
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        [MinLength(5)]
        public string Senha { get; set; } = string.Empty;
        public string Cargo { get; set; } = "Usuario";




    }

   
}
