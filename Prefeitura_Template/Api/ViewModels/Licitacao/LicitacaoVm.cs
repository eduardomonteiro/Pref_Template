using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto do Documento
    /// </summary>
    public class LicitacaoVm
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
        public GenericVm LicitacaoModalidade { get; set; }

        /// <summary>
        /// Status da Licitação
        /// </summary>
        public GenericVm StatusPublicacao { get; set; }

        /// <summary>
        /// Data de Abertura
        /// </summary>
        public string DataAbertura { get; set; }

        /// <summary>
        /// Data da Publicação
        /// </summary>
        public string DataPublicacao { get; set; }

        /// <summary>
        /// Data d Alteracao
        /// </summary>
        public string DataAlteracaoEditavel { get; set; }

        /// <summary>
        /// Arquivos da Licitação
        /// </summary>
        public List<LicitacaoArquivoVm> LicitacaoArquivo { get; set; }
    }
}