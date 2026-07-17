using System.ComponentModel.DataAnnotations;

namespace ProjetoMIragnum
{
    public class DTO
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string Senha { get; set; } = string.Empty;

      


    }
}
