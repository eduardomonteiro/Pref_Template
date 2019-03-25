using Prefeitura_Template.Areas.Admin.Enums;
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
    [Table("Secretaria")]
    public class Secretaria : EntidadePadrao
    {
        [StringLength(300, ErrorMessage = "{0}: Limite de 300 caracteres!")]
        public string Nome { get; set; }

        [Display(Name = "Slug")]
        [StringLength(300, ErrorMessage = "{0}: Limite de 300 caracteres!")]
        public string Slug { get; set; }

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
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioSecretaria()) + Icone;
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
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioSecretaria() + Icone;
                }
            }
        }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name ="Nome do Responsável")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string NomeResponsavel { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Foto do Responsável")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string ImagemResponsavel { get; set; }

        [NotMapped]
        public string CaminhoImagemResponsavel
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemResponsavel))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioSecretaria()) + ImagemResponsavel;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoImagemResponsavel
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemResponsavel))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioSecretaria() + ImagemResponsavel;
                }
            }
        }

        [Display(Name = "Cargo")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Cargo { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string Email { get; set; }

        [Display(Name = "Telefone")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Telefone { get; set; }

        [Display(Name = "Horário de Atendimento")]
        [StringLength(300, ErrorMessage = "{0}: Limite de 300 caracteres!")]
        public string HorarioAtendimento { get; set; }

        [Display(Name = "Endereço")]
        [StringLength(300, ErrorMessage = "{0}: Limite de 300 caracteres!")]
        public string Endereco { get; set; }

        [Display(Name = "Latitude")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Latitude { get; set; }

        [Display(Name = "Longitude")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Longitude { get; set; }

        [Display(Name = "Atribuições")]
        [StringLength(1000, ErrorMessage = "{0}: Limite de 1000 caracteres!")]
        public string Atribuicao { get; set; }

        [Display(Name = "Imagem do Local")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string ImagemLocal { get; set; }

        [NotMapped]
        public string CaminhoImagemLocal
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemLocal))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioSecretaria()) + ImagemLocal;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoImagemLocal
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemLocal))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioSecretaria() + ImagemLocal;
                }
            }
        }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Categoria")]
        public int SecretariaCategoriaId { get; set; }

        public virtual SecretariaCategoria SecretariaCategoria { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Prefixo")]
        public int SecretariaNomePrefixoId { get; set; }

        public virtual SecretariaNomePrefixo SecretariaNomePrefixo { get; set; }

        [NotMapped]
        public string Prefixo
        {
            get
            {
                return SecretariaNomePrefixo.Descricao;
            }
        }

        [NotMapped]
        public string NomeComPrefixo
        {
            get
            {
                return Prefixo + " " + Nome;
            }
        }

        public virtual ICollection<SecretariaServico> SecretariaServico { get; set; }

        [NotMapped]
        [Display(Name = "Principais Serviços")]
        public string PincipaisServicos
        {
            get
            {
                if (SecretariaServico != null && SecretariaServico.Count > 0)
                {
                    return String.Join(", ", SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo && x.Ordem > 0).OrderBy(x => x.Ordem).Take(3).Select(x => x.ServicoNome).ToArray());
                }
                else
                {
                    return "";
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
                    List<Tag> TagList = db.Tag.Where(x => x.AreaId == 11 && x.RegistroId == Id).ToList();
                    return TagList;
                }
            }
        }
    }
}