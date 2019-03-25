using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
namespace Prefeitura_Template.Models
{
    [Table("Enquete")]
    public class Enquete : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(1500, ErrorMessage = "{0}: Limite de 1500 caracteres!")]
        [AllowHtml]
        public string Pergunta { get; set; }

        [Display(Name = "Data da Publicação")]
        public DateTime? DataPublicacao { get; set; }

        [Display(Name = "Data de Início")]
        public DateTime DataInicial { get; set; }

        [Display(Name = "Data de Encerramento")]
        public DateTime DataEncerramento { get; set; }

        public virtual ICollection<EnqueteOpcao> EnqueteOpcao { get; set; }

        [NotMapped]
        public bool Encerrado
        {
            get
            {
                if(DataEncerramento < DateTime.Now)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}