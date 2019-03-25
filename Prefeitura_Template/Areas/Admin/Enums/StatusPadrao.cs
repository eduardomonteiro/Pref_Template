using Prefeitura_Template.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prefeitura_Template.Areas.Admin.Enums
{
    [Flags]
    public enum StatusPadrao
    {
        Ativo = 1,
        Inativo = 0,
        [Ignored]
        Excluido = 3
    }
}
