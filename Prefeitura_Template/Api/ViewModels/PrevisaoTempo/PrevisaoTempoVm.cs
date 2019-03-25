using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Nome da Cidade
    /// </summary>
    public class PrevisaoTempoVm
    {
        /// <summary>
        /// Dia
        /// </summary>
        [XmlElement("dia")]
        public string dia { get; set; }

        /// <summary>
        /// Tempo
        /// </summary>
        [XmlElement("tempo")]
        public string tempo { get; set; }

        /// <summary>
        /// Temperatura máxima
        /// </summary>
        [XmlElement("maxima")]
        public string maxima { get; set; }

        /// <summary>
        /// Temperatura Mínima
        /// </summary>
        [XmlElement("minima")]
        public string minima { get; set; }

        /// <summary>
        /// IUV
        /// </summary>
        [XmlElement("iuv")]
        public string iuv { get; set; }

        /// <summary>
        /// Icone Branco
        /// </summary>
        public string iconebranco { get; set; }

        /// <summary>
        /// Icone Preto
        /// </summary>
        public string iconepreto { get; set; }
    }
}