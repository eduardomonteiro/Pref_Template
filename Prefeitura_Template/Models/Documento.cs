using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("Documento")]
    public class Documento:EntidadePadrao
    {
        public Documento()
        {
            this.DocumentoArquivo = new List<DocumentoArquivo>();
        }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(60, ErrorMessage = "{0}: Limite de 60 caracteres!")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Categoria")]
        public int DocumentoCategoriaId { get; set; }

        public virtual DocumentoCategoria DocumentoCategoria { get; set; }

        public virtual ICollection<DocumentoArquivo> DocumentoArquivo { get; set; }

        [NotMapped]
        public virtual DocumentoArquivo DocumentoArquivoAtual
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    DocumentoArquivo Arquivo = DocumentoArquivo.Where(x => x.Status == (int)StatusPadrao.Ativo).FirstOrDefault();
                    return Arquivo;
                }
            }
        }

        [Display(Name = "Data da Publicação")]
        public DateTime? DataPublicacao { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Texto")]
        [AllowHtml]
        public string Texto { get; set; }

        [NotMapped]
        public virtual ICollection<Tag> Tag
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    List<Tag> TagList = db.Tag.Where(x => x.AreaId == 10 && x.RegistroId == Id).ToList();
                    return TagList;
                }
            }
        }
    }
}