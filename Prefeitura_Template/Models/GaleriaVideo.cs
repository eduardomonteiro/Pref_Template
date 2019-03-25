using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("GaleriaVideo")]
    public class GaleriaVideo : EntidadePadrao
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
        public int GaleriaVideoCategoriaId { get; set; }

        public virtual GaleriaVideoCategoria GaleriaVideoCategoria { get; set; }

        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Slug { get; set; }

        public bool Destaque { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(1000, ErrorMessage = "{0}: Limite de 1000 caracteres!")]
        [Display(Name = "Link do Vídeo")]
        public string LinkVideo { get; set; }

        [NotMapped]
        public string ImagemVideo
        {
            get
            {
                return "https://i1.ytimg.com/vi/" + LinkVideo + "/sddefault.jpg";
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
                    List<Tag> TagList = db.Tag.Where(x => x.AreaId == 21 && x.RegistroId == Id).ToList();
                    return TagList;
                }
            }
        }
    }
}