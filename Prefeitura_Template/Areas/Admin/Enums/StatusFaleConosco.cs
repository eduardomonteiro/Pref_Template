using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Areas.Admin.Enums
{
    [Flags]
    public enum StatusFaleConosco
    {
        Novo = 0,
        [Display(Name = "Em Análise")]
        Analise = 1,
        Respondido = 2
    }
}