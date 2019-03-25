using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto do Documento Vinculado
    /// </summary>
    public class DocumentoVinculadoVm
    {
        /// <summary>
        /// Nome do Documento
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Formato do Documento
        /// </summary>
        public string Formato { get; set; }

        /// <summary>
        /// Arquivo do Documento
        /// </summary>
        public string Arquivo { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo do Documento
        /// </summary>
        public string CaminhoLogicoArquivo { get; set; }

        /// <summary>
        /// Tags que fizeram o vínculo
        /// </summary>
        public List<TagVm> Tag { get; set; }
    }
}