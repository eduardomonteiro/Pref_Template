using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class GaleriaFotoListaVm
    {
        /// <summary>
        /// Titulo da Galeria
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Slug da Galeria
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Descrição da Galeria
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Data e hora da publicação da Galeria
        /// </summary>
        public string DataPublicacao { get; set; }

        /// <summary>
        /// Categoria da Galeria
        /// </summary>
        public GenericVm GaleriaFotoCategoria { get; set; }

        /// <summary>
        /// Fotografo da Galeria
        /// </summary>
        public string Fotografo { get; set; }

        /// <summary>
        /// Fotos da galeria
        /// </summary>
        public List<GaleriaFotoGaleriaVm> GaleriaFotoGaleria { get; set; }

        /// <summary>
        /// Galeria destaque? True = Sim , False = Não
        /// </summary>
        public bool Destaque { get; set; }

        /// <summary>
        /// Quantidade de Fotos na galeria
        /// </summary>
        public int QuantidadeFoto { get; set; }
    }
}