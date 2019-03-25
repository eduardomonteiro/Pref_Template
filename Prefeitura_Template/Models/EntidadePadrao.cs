using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prefeitura_Template.Models
{
    public class EntidadePadrao
    {
        [Key]
        public int Id { get; set; }

        public DateTime? DataAtualizacao { get; set; }

        public DateTime DataCadastro { get; set; }

        public int Status { get; set; }

        [NotMapped]
        public string NomeStatus { get { return EnumExtensions.GetEnumDisplayName(typeof(StatusPadrao), Status); } }
    }
}