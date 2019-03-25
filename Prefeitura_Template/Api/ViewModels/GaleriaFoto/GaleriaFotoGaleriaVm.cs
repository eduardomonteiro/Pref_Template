using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class GaleriaFotoGaleriaVm
    {
        /// <summary>
        /// Arquivo de imagem
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo da imagem
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }

        /// <summary>
        /// Credito da Foto
        /// </summary>
        public string Credito { get; set; }

        /// <summary>
        /// Legenda da Foto
        /// </summary>
        public string Legenda { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public virtual ICollection<TagVm> Tag { get; set; }
    }
}