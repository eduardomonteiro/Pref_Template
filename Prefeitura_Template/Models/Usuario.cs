using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prefeitura_Template.Models
{
    [Table("Usuario")]
    public class Usuario : ApplicationUser
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(300, ErrorMessage = "{0}: Limite de 300 caracteres!")]
        public string Nome { get; set; }

        [StringLength(15)]
        public string Tema { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Perfil")]
        public int PerfilId { get; set; }

        public virtual Perfil Perfil { get; set; }

        public virtual ICollection<Log> Logs { get; set; }

        public virtual ICollection<UsuarioSecretaria> UsuarioSecretaria { get; set; }

        public DateTime? DataAtualizacao { get; set; }

        public DateTime DataCadastro { get; set; }

        public int Status { get; set; }

        [NotMapped]
        public string NomeStatus { get { return EnumExtensions.GetEnumDisplayName(typeof(StatusPadrao), Status); } }
    }
}