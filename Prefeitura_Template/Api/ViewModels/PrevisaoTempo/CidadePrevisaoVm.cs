using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de retorno
    /// </summary>
    [XmlRoot("cidade")]
    public class CidadePrevisaoVm
    {
        /// <summary>
        /// Nome da Cidade
        /// </summary>
        [XmlElement("nome")]
        public string nome { get; set; }

        /// <summary>
        /// UF
        /// </summary>
        [XmlElement("uf")]
        public string uf { get; set; }

        /// <summary>
        /// Data de Atualização
        /// </summary>
        [XmlElement("atualizacao")]
        public string atualizacao { get; set; }

        /// <summary>
        /// Lista com as previsões
        /// </summary>
        [XmlElement("previsao")]
        public List<PrevisaoTempoVm> previsao { get; set; }
    }
}