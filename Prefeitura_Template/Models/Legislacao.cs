using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("Legislacao")]
    public class Legislacao : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(60, ErrorMessage = "{0}: Limite de 60 caracteres!")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [AllowHtml]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Categoria")]
        public int LegislacaoCategoriaId { get; set; }

        public virtual LegislacaoCategoria LegislacaoCategoria { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Data da Publicação")]
        public DateTime DataPublicacao { get; set; }

        public virtual ICollection<LegislacaoArquivo> LegislacaoArquivo { get; set; }
    }
}