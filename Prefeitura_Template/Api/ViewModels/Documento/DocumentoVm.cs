using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto do Documento
    /// </summary>
    public class DocumentoVm
    {
        /// <summary>
        /// Titulo do documento
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Categoria do documento
        /// </summary>
        public GenericVm DocumentoCategoria { get; set; }

        /// <summary>
        /// Objeto do Arquivo
        /// </summary>
        public DocumentoArquivoVm DocumentoArquivoAtual { get; set; }

        /// <summary>
        /// Data de puclicação do documento
        /// </summary>
        public string DataPublicacao { get; set; }

        /// <summary>
        /// Texto do documento
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public List<TagVm> Tag { get; set; }
    }
}