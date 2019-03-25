using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Obejto de retorno
    /// </summary>
    public class LegislacaoVm
    {
        /// <summary>
        /// Titulo da Legislacao
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descricao da Legislacao
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Categoria da Legislacao
        /// </summary>
        public GenericVm LegislacaoCategoria { get; set; }

        /// <summary>
        /// Data de Publicação da Legislacao
        /// </summary>
        public string DataPublicacao { get; set; }

        /// <summary>
        /// Arquivos da Legislacao
        /// </summary>
        public List<LegislacaoArquivoVm> LegislacaoArquivo { get; set; }
    }
}