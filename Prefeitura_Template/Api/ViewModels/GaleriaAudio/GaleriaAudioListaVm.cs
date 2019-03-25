using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class GaleriaAudioListaVm
    {
        /// <summary>
        /// Titulo do Audio
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descrição do Audio
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Categoria do Audio
        /// </summary>
        public GenericVm GaleriaAudioCategoria { get; set; }

        /// <summary>
        /// Arquivo do Audio
        /// </summary>
        public string Audio { get; set; }

        /// <summary>
        /// Caminho completo do arquivo de audio
        /// </summary>
        public string CaminhoLogicoAudio { get; set; }

        /// <summary>
        /// Data da publicação
        /// </summary>
        public string DataPublicacao { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public List<TagVm> Tag { get; set; }
    }
}