using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("GaleriaAudio")]
    public class GaleriaAudio : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(60, ErrorMessage = "{0}: Limite de 60 caracteres!")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [AllowHtml]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        public int GaleriaAudioCategoriaId { get; set; }

        public virtual GaleriaAudioCategoria GaleriaAudioCategoria { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string Audio { get; set; }

        [NotMapped]
        public string CaminhoAudio
        {
            get
            {
                if (string.IsNullOrEmpty(Audio))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioAudio()) + Audio;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoAudio
        {
            get
            {
                if (string.IsNullOrEmpty(Audio))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioAudio() + Audio;
                }
            }
        }

        [Display(Name = "Data da Publicação")]
        public DateTime? DataPublicacao { get; set; }

        [NotMapped]
        public virtual ICollection<Tag> Tag
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    List<Tag> TagList = db.Tag.Where(x => x.AreaId == 14 && x.RegistroId == Id).ToList();
                    return TagList;
                }
            }
        }
    }
}