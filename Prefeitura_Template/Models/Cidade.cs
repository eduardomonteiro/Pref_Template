using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("Cidade")]
    public class Cidade
    {
        [Key]
        public int Id { get; set; }

        public DateTime? DataAtualizacao { get; set; }

        [AllowHtml]
        public string ExPrefeito { get; set; }

        [AllowHtml]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Imagem da Descrição")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string ImagemDescricao { get; set; }

        [NotMapped]
        public string CaminhoImagemDescricao
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemDescricao))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioCidade()) + ImagemDescricao;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoImagemDescricao
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemDescricao))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioCidade() + ImagemDescricao;
                }
            }
        }

        [Display(Name = "Descrição da Bandeira")]
        [AllowHtml]
        public string DescricaoBandeira { get; set; }

        [Display(Name = "Imagem da Descrição da Bandeira")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string ImagemBandeira { get; set; }

        [NotMapped]
        public string CaminhoImagemBandeira
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemBandeira))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioCidade()) + ImagemBandeira;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoImagemBandeira
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemBandeira))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioCidade() + ImagemBandeira;
                }
            }
        }

        [AllowHtml]
        [Display(Name = "Descrição do Invista na Cidade")]
        public string DescricaoInvista { get; set; }

        [Display(Name = "Imagem da Descrição do Invista na Cidade")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string ImagemInvista { get; set; }

        [NotMapped]
        public string CaminhoImagemInvista
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemInvista))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioCidade()) + ImagemInvista;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoImagemInvista
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemInvista))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioCidade() + ImagemInvista;
                }
            }
        }

        [AllowHtml]
        [Display(Name = "Descrição do Brasão")]
        public string DescricaoBrasao { get; set; }

        [Display(Name = "Imagem da Descrição do Brasão")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string ImagemBrasao { get; set; }

        [NotMapped]
        public string CaminhoImagemBrasao
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemBrasao))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioCidade()) + ImagemBrasao;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoImagemBrasao
        {
            get
            {
                if (string.IsNullOrEmpty(ImagemBrasao))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioCidade() + ImagemBrasao;
                }
            }
        }

        [AllowHtml]
        [Display(Name = "Patrimônio Histórico Cultural")]
        public string PatrimonioHistoricoCultural { get; set; }

        [AllowHtml]
        public string Turismo { get; set; }

        [AllowHtml]
        [Display(Name = "Descrição do Hino")]
        public string DescricaoHino { get; set; }

        [Display(Name = "Áudio do Hino")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string AudioHino { get; set; }

        [NotMapped]
        public string CaminhoAudioHino
        {
            get
            {
                if (string.IsNullOrEmpty(AudioHino))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioCidade()) + AudioHino;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoAudioHino
        {
            get
            {
                if (string.IsNullOrEmpty(AudioHino))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioCidade() + AudioHino;
                }
            }
        }

        [Display(Name = "Data da Fundação")]
        public DateTime? DataFundacao { get; set; }

        [Display(Name = "Prefeito Atual")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string AtualPrefeito { get; set; }

        [Display(Name = "População")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Populacao { get; set; }

        [Display(Name = "Clima")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Clima { get; set; }

        [Display(Name = "Área")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Area { get; set; }

        [Display(Name = "Densidade")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Densidade { get; set; }

        [Display(Name = "Altitude")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Altitude { get; set; }

        [Display(Name = "Telefone (Fale Conosco)")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Telefone { get; set; }

        [Display(Name = "E-mail (Fale Conosco)")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Email { get; set; }
    }
}