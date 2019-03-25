using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prefeitura_Template.Models
{
    [Table("AaZ")]
    public class AaZ : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(1000, ErrorMessage = "{0}: Limite de 1000 caracteres!")]
        public string Url { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Categoria")]
        public int AaZCategoriaId { get; set; }

        public virtual AaZCategoria AaZCategoria { get; set; }
    }
}