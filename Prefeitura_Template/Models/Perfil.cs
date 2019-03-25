using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prefeitura_Template.Models
{
    [Table("Perfil")]
    public class Perfil : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public virtual ICollection<Perfil_Area> Perfil_Area { get; set; }
    }
}