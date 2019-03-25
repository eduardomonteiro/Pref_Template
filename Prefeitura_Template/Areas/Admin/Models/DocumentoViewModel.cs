using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace Prefeitura_Template.Areas.Admin.Models
{
    public class DocumentoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(60, ErrorMessage = "{0}: Limite de 60 caracteres!")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Categoria")]
        public int DocumentoCategoriaId { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Nome do Arquivo")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string ArquivoNome { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string Arquivo { get; set; }

        public string CaminhoLogicoArquivo { get; set; }

        [Display(Name = "Data da Publicação")]
        public DateTime? DataPublicacao { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Texto")]
        [AllowHtml]
        public string Texto { get; set; }

        public string Tag { get; set; }

        public int Status { get; set; }
    }
}