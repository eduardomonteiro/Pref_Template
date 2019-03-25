using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Areas.Admin.Models
{
    public class NewsLetterViewModel
    {
        public string Email { get; set; }

        public string Nome { get; set; }

        public string Sexo { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Status { get; set; }
    }
}