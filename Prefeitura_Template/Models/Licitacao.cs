using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("Licitacao")]
    public class Licitacao : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(140, ErrorMessage = "{0}: Limite de 60 caracteres!")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [AllowHtml]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(30, ErrorMessage = "{0}: Limite de 30 caracteres!")]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Modalidade")]
        public int LicitacaoModalidadeId { get; set; }

        public virtual LicitacaoModalidade LicitacaoModalidade { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Status da Publicação")]
        public int StatusPublicacaoId { get; set; }

        public virtual StatusPublicacao StatusPublicacao { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Data de Abertura")]
        public DateTime DataAbertura { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Data Publicação")]
        public DateTime DataPublicacao { get; set; }

        [Display(Name = "Data de Alteração")]
        public DateTime? DataAlteracaoEditavel { get; set; }

        public virtual ICollection<LicitacaoArquivo> LicitacaoArquivo { get; set; }
    }
}