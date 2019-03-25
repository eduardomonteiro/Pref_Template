using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class InformativoVm
    {
        /// <summary>
        /// Informativo que são destaques
        /// </summary>
        public List<InformativoListaVm> InformativoDestaque { get; set; }

        /// <summary>
        /// Informativo que nao sao destaques
        /// </summary>
        public List<InformativoListaVm> Informativo { get; set; }
    }
}