using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class GaleriaAudioVm
    {
        /// <summary>
        /// Audios que são destaques
        /// </summary>
        public List<GaleriaAudioListaVm> GaleriaAudioDestaque { get; set; }

        /// <summary>
        /// Audios que nao sao destaques
        /// </summary>
        public List<GaleriaAudioListaVm> GaleriaAudio { get; set; }
    }
}