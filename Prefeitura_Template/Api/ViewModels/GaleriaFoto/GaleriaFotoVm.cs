using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class GaleriaFotoVm
    {
        /// <summary>
        /// Fotos que são destaques
        /// </summary>
        public List<GaleriaFotoListaVm> GaleriaFotoDestaque { get; set; }

        /// <summary>
        /// Fotos que nao sao destaques
        /// </summary>
        public List<GaleriaFotoListaVm> GaleriaFoto { get; set; }
    }
}