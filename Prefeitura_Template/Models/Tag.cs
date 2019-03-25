using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("Tag")]
    public class Tag : EntidadePadrao
    {
        public string Descricao { get; set; }

        public int RegistroId { get; set; }

        public int AreaId { get; set; }

        public string Slug { get; set; }
    }
}