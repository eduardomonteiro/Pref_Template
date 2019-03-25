using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto do Documento
    /// </summary>
    public class ConcursoVm
    {
        /// <summary>
        /// Titulo da Licitação
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descrição da Licitação
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Número da Licitação
        /// </summary>
        public string Numero { get; set; }

        /// <summary>
        /// Slug da Licitação
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Modalidade da Licitação
        /// </summary>
        public GenericVm ConcursoModalidade { get; set; }

        /// <summary>
        /// Status da Licitação
        /// </summary>
        public GenericVm StatusPublicacao { get; set; }

        /// <summary>
        /// Data Inicial
        /// </summary>
        public string DataInicio { get; set; }

        /// <summary>
        /// Data Final
        /// </summary>
        public string DataFim { get; set; }

        /// <summary>
        /// Arquivos da Licitação
        /// </summary>
        public List<ConcursoArquivoVm> ConcursoArquivo { get; set; }
    }
}