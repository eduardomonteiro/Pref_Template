using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Models
{
    [Table("GaleriaFotoGaleria")]
    public class GaleriaFotoGaleria : EntidadePadrao
    {
        public int GaleriaFotoId { get; set; }

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
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioGaleriaImagem()) + Imagem;
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
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioGaleriaImagem() + Imagem;
                }
            }
        }

        [StringLength(500, ErrorMessage = "{0}: Limite de 500 caracteres!")]
        [Display(Name = "Crédito")]
        public string Credito { get; set; }

        [StringLength(500, ErrorMessage = "{0}: Limite de 500 caracteres!")]
        public string Legenda { get; set; }

        [NotMapped]
        public virtual ICollection<Tag> Tag
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    List<Tag> TagList = db.Tag.Where(x => x.AreaId == 13 && x.RegistroId == Id).ToList();
                    return TagList;
                }
            }
        }

        [NotMapped]
        public string TagsString
        {
            get
            {
                if (Tag != null && Tag.Count > 0)
                {
                    return String.Join(",", Tag.Select(x => x.Descricao).ToArray());
                }
                else
                {
                    return "";
                }
            }
        }
    }
}