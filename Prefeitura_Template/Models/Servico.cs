using Prefeitura_Template.Areas.Admin.Enums;
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
    [Table("Servico")]
    public class Servico : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(140, ErrorMessage = "{0}: Limite de 140 caracteres!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        [Display(Name = "Chamada")]
        public string Chamada { get; set; }

        [Display(Name = "Slug")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string Slug { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        public int ServicoCategoriaId { get; set; }

        public bool Destaque { get; set; }

        public virtual ServicoCategoria ServicoCategoria { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Ícone")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string Icone { get; set; }

        [NotMapped]
        public string CaminhoIcone
        {
            get
            {
                if (string.IsNullOrEmpty(Icone))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioServico()) + Icone;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoIcone
        {
            get
            {
                if (string.IsNullOrEmpty(Icone))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioServico() + Icone;
                }
            }
        }

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
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioServico()) + Imagem;
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
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioServico() + Imagem;
                }
            }
        }

        [AllowHtml]
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Link Externo")]
        [StringLength(500, ErrorMessage = "{0}: Limite de 500 caracteres!")]
        public string LinkExterno { get; set; }

        public virtual ICollection<ServicoArquivo> ServicoArquivo { get; set; }

        public virtual ICollection<ServicoPin> ServicoPin { get; set; }

        public virtual ICollection<SecretariaServico> SecretariaServico { get; set; }

        [NotMapped]
        public List<int> Secretarias
        {
            get
            {
                if (SecretariaServico != null && SecretariaServico.Count > 0)
                {
                    List<int> Ids = new List<int>();
                    foreach (var item in SecretariaServico)
                    {
                        Ids.Add(item.SecretariaId);
                    }
                    return Ids;
                }
                else
                {
                    return null;
                }
            }
        }

        [NotMapped]
        [Display(Name = "Secretarias")]
        public string SecretariasNomes
        {
            get
            {
                if (SecretariaServico != null && SecretariaServico.Count > 0)
                {
                    return String.Join("/ ", SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo && x.Ordem > 0).OrderBy(x => x.Ordem).Select(x => x.SecretariaNome).ToArray());
                }
                else
                {
                    return "";
                }
            }
        }
    }
}