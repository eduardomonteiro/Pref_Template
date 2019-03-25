using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("PerguntasFrequentes")]
    public class PerguntasFrequentes : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(500, ErrorMessage = "{0}: Limite de 500 caracteres!")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [AllowHtml]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Categoria")]
        public int PerguntasFrequentesCategoriaId { get; set; }

        public virtual PerguntasFrequentesCategoria PerguntasFrequentesCategoria { get; set; }
    }
}