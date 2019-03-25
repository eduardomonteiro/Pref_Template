using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("Turismo")]
    public class Turismo : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Descrição")]
        [AllowHtml]
        public string Descricao { get; set; }

        [Display(Name = "Link do Google Maps")]
        public string LinkMaps { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string Imagem { get; set; }

        [NotMapped]
        public string CaminhoImagem
        {
            get
            {
                if (string.IsNullOrEmpty(Imagem))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioTurismo()) + Imagem;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoImagem
        {
            get
            {
                if (string.IsNullOrEmpty(Imagem))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioTurismo() + Imagem;
                }
            }
        }
    }
}