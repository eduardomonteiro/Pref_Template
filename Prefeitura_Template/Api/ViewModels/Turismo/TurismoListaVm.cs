using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto da Lista
    /// </summary>
    public class TurismoListaVm
    {
        /// <summary>
        /// Nome do Patrimonio
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Texto descritivo do Patrimonio
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Link do Goggle Maps
        /// </summary>
        public string LinkMaps { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Patrimonio
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Patrimonio
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }
    }
}