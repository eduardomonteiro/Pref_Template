using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prefeitura_Template.Models
{
    [Table("Area")]
    public class Area : EntidadePadrao
    {
        [StringLength(100, ErrorMessage = "Limite de 100 caracteres!")]
        public string Descricao { get; set; }

        [StringLength(100, ErrorMessage = "Limite de 100 caracteres!")]
        public string Nome { get; set; }

        [StringLength(50, ErrorMessage = "Limite de 50 caracteres!")]
        public string Action { get; set; }

        [StringLength(30, ErrorMessage = "Limite de 30 caracteres!")]
        public string LinkClass { get; set; }

        public int AreaPai { get; set; }

        public int Ordem { get; set; }

        public virtual ICollection<Perfil_Area> Perfil_Area { get; set; }

        public virtual ICollection<Log> Log { get; set; }
    }
}