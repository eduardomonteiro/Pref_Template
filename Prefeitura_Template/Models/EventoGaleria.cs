using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Prefeitura_Template.Models
{
    [Table("EventoGaleria")]
    public class EventoGaleria : EntidadePadrao
    {
        public int EventoId { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string Midia { get; set; }

        [NotMapped]
        public string CaminhoMidia
        {
            get
            {
                if (string.IsNullOrEmpty(Midia))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioEvento()) + Midia;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoMidia
        {
            get
            {
                if (string.IsNullOrEmpty(Midia))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioEvento() + Midia;
                }
            }
        }

        [StringLength(500, ErrorMessage = "{0}: Limite de 500 caracteres!")]
        [Display(Name = "Crédito")]
        public string Credito { get; set; }

        [StringLength(500, ErrorMessage = "{0}: Limite de 500 caracteres!")]
        public string Legenda { get; set; }
    }
}