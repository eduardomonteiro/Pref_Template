using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class GaleriaVideoVm
    {
        /// <summary>
        /// Videos que são destaques
        /// </summary>
        public List<GaleriaVideoListaVm> GaleriaVideoDestaque { get; set; }

        /// <summary>
        /// Videos que nao sao destaques
        /// </summary>
        public List<GaleriaVideoListaVm> GaleriaVideo { get; set; }
    }
}