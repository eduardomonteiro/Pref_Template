using Prefeitura_Template.Areas.Admin.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;


namespace Prefeitura_Template.Models
{
    [Table("Timeline")]
    public class Timeline : EntidadePadrao
    {
        [Display(Name = "Ano")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Ano { get; set; }

        public int Ordem { get; set; }

        [AllowHtml]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Imagem")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
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
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioTimeLine()) + Imagem;
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
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioTimeLine() + Imagem;
                }
            }
        }
    }
}