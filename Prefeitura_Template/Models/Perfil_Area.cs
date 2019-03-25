using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prefeitura_Template.Models
{
    [Table("Perfil_Area")]
    public class Perfil_Area
    {
        [Key]
        public int Id { get; set; }

        public int PerfilId { get; set; }

        public int AreaId { get; set; }

        public bool Leitura { get; set; }

        public bool Criacao { get; set; }

        public bool Alteracao { get; set; }

        public bool Exclusao { get; set; }

        public virtual Area Area { get; set; }

        public virtual Perfil Perfil { get; set; }
    }
}