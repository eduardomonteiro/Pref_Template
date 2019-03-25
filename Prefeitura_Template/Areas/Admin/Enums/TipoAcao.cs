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
    public enum TipoAcao
    {
        [Display(Name = "Visualização")]
        Visualizacao = 0,
        [Display(Name = "Adição")]
        Adicao = 1,
        [Display(Name = "Edição")]
        Edicao = 2,
        [Display(Name = "Exclusão")]
        Exclusao = 3,
        [Display(Name = "Ativar")]
        Ativar = 4,
        [Display(Name = "Desativar")]
        Desativar = 5,
        [Display(Name = "Vincular")]
        Vincular = 6,
        [Display(Name = "Desvincular")]
        Desvincular = 7,
        [Display(Name = "Login")]
        Login = 8,
        [Display(Name = "LogOff")]
        LogOff = 9,
        [Display(Name = "Upload")]
        Upload = 10,
        [Display(Name = "Exportação")]
        Exportacao = 11
    }

   


}
