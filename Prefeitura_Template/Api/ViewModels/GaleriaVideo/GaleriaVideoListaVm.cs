using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class GaleriaVideoListaVm
    {
        /// <summary>
        /// Titulo do Video
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descrição do Video
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Slug do Video
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Categoria do Video
        /// </summary>
        public GenericVm GaleriaVideoCategoria { get; set; }

        /// <summary>
        /// Galeria destaque? True = Sim , False = Não
        /// </summary>
        public bool Destaque { get; set; }

        /// <summary>
        /// Link do Video no youtube
        /// </summary>
        public string LinkVideo { get; set; }

        /// <summary>
        /// Link do Video no youtube
        /// </summary>
        public string ImagemVideo { get; set; }

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