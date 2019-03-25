using Prefeitura_Template.Areas.Admin.Utils;
using Prefeitura_Template.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("Projeto")]
    public class Projeto : EntidadePadrao
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
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Slug { get; set; }

        [Display(Name = "Secretaria")]
        public int SecretariaId { get; set; }

        public virtual Secretaria Secretaria { get; set; }

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
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioProjeto()) + Imagem;
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
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioProjeto() + Imagem;
                }
            }
        }

        [Display(Name = "Video do Youtube")]
        [StringLength(1000, ErrorMessage = "{0}: Limite de 1000 caracteres!")]
        public string LinkVideo { get; set; }

        [NotMapped]
        public virtual ICollection<Tag> Tag
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    List<Tag> TagList = db.Tag.Where(x => x.AreaId == 9 && x.RegistroId == Id).ToList();
                    return TagList;
                }
            }
        }

        [Display(Name = "Link Externo")]
        [StringLength(1000, ErrorMessage = "{0}: Limite de 1000 caracteres!")]
        public string LinkExterno { get; set; }

        //[StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        //[Display(Name = "Nome do Arquivo")]
        //public string ArquivoNome { get; set; }

        //[StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        //public string Arquivo { get; set; }

        //[NotMapped]
        //public string CaminhoArquivo
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(Arquivo))
        //        {
        //            return "";
        //        }
        //        else
        //        {
        //            return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioProjeto()) + Arquivo;
        //        }
        //    }
        //}

        //[NotMapped]
        //public string CaminhoLogicoArquivo
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(Arquivo))
        //        {
        //            return "";
        //        }
        //        else
        //        {
        //            return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioProjeto() + Arquivo;
        //        }
        //    }
        //}

        public virtual ICollection<ProjetoArquivo> ProjetoArquivo { get; set; }

        [NotMapped]
        public virtual ICollection<NoticiaVinculadaVm> Noticias { get; set; }
        [NotMapped]
        public virtual ICollection<EventoVinculadoVm> Eventos { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        public int ProjetoCategoriaId { get; set; }

        public virtual ProjetoCategoria ProjetoCategoria { get; set; }
    }
}