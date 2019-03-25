﻿using Prefeitura_Template.Areas.Admin.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace Prefeitura_Template.Models
{
    [Table("DocumentoArquivo")]
    public class DocumentoArquivo : EntidadePadrao
    {

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        public string Arquivo { get; set; }

        [NotMapped]
        public string CaminhoArquivo
        {
            get
            {
                if (string.IsNullOrEmpty(Arquivo))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.MapPath(Utils.RetornaDiretorioDocumento()) + Arquivo;
                }
            }
        }

        [NotMapped]
        public string CaminhoLogicoArquivo
        {
            get
            {
                if (string.IsNullOrEmpty(Arquivo))
                {
                    return "";
                }
                else
                {
                    return "http://" + HttpContext.Current.Request.Url.Authority + Utils.RetornaDiretorioDocumento() + Arquivo;
                }
            }
        }

        [StringLength(200, ErrorMessage = "{0}: Limite de 200 caracteres!")]
        [Display(Name = "Nome de Exibição do Arquivo")]
        public string ArquivoNome { get; set; }

        [StringLength(20, ErrorMessage = "{0}: Limite de 20 caracteres!")]
        [Display(Name = "Tamanho do Arquivo")]
        public string Tamanho { get; set; }

        [Display(Name = "Documento")]
        public int DocumentoId { get; set; }

        [NotMapped]
        public string Formato
        {
            get
            {
                return Path.GetExtension(Arquivo.ToLower());
            }
        }
    }
}