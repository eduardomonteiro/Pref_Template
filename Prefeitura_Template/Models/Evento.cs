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
    [Table("Evento")]
    public class Evento : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(60, ErrorMessage = "{0}: Limite de 60 caracteres!")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(140, ErrorMessage = "{0}: Limite de 140 caracteres!")]
        [Display(Name = "Subtítulo")]
        public string SubTitulo { get; set; }

        [AllowHtml]
        public string Texto { get; set; }

        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Slug { get; set; }

        [Display(Name = "Data/Hora do Evento")]
        public DateTime? DataHorarioEvento { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Categoria")]
        public int EventoCategoriaId { get; set; }

        public virtual EventoCategoria EventoCategoria { get; set; }

        public virtual ICollection<EventoGaleria> EventoGaleria { get; set; }

        [Display(Name = "Video do Youtube")]
        [StringLength(1000, ErrorMessage = "{0}: Limite de 1000 caracteres!")]
        public string LinkVideo { get; set; }

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
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioEvento()) + Imagem;
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
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioEvento() + Imagem;
                }
            }
        }

        [NotMapped]
        public virtual ICollection<Tag> Tag
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    List<Tag> TagList = db.Tag.Where(x => x.AreaId == 7 && x.RegistroId == Id).ToList();
                    return TagList;
                }
            }
        }
    }
}