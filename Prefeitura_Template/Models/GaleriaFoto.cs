using Prefeitura_Template.Areas.Admin.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("GaleriaFoto")]
    public class GaleriaFoto : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(60, ErrorMessage = "{0}: Limite de 60 caracteres!")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Slug { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Data da Publicação")]
        public DateTime? DataPublicacao { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Categoria")]
        public int GaleriaFotoCategoriaId { get; set; }

        public virtual GaleriaFotoCategoria GaleriaFotoCategoria { get; set; }

        [StringLength(60, ErrorMessage = "{0}: Limite de 60 caracteres!")]
        [Display(Name = "Nome do Fotógrafo")]
        public string Fotografo { get; set; }

        public virtual ICollection<GaleriaFotoGaleria> GaleriaFotoGaleria { get; set; }

        public bool Destaque { get; set; }

        [NotMapped]
        public int QuantidadeFoto
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    return db.GaleriaFotoGaleria.Where(x => x.GaleriaFotoId == Id && x.Status == (int)StatusPadrao.Ativo).Count();
                }
            }
        }
    }
}