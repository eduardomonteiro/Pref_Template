using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("Concurso")]
    public class Concurso : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(60, ErrorMessage = "{0}: Limite de 60 caracteres!")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Descrição")]
        [AllowHtml]
        public string Descricao { get; set; }

        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(30, ErrorMessage = "{0}: Limite de 30 caracteres!")]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Modalidade")]
        public int ConcursoModalidadeId { get; set; }

        public virtual ConcursoModalidade ConcursoModalidade { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Status da Publicação")]
        public int StatusPublicacaoId { get; set; }

        public virtual StatusPublicacao StatusPublicacao { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Data Inicial")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Data Final")]
        public DateTime DataFim { get; set; }

        public virtual ICollection<ConcursoArquivo> ConcursoArquivo { get; set; }
    }
}