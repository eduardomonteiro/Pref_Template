using Prefeitura_Template.Areas.Admin.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace Prefeitura_Template.Models
{
    [Table("ServicoPin")]
    public class ServicoPin : EntidadePadrao
    {
        [Display(Name = "Serviço")]
        public int ServicoId { get; set; }

        [Display(Name = "Latitude")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Latitude { get; set; }

        [Display(Name = "Longitude")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Longitude { get; set; }
    }
}