namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class ServicoArquivoVm
    {
        /// <summary>
        /// Arquivo
        /// </summary>
        public string Arquivo { get; set; }

        /// <summary>
        /// Caminho completo do arquivo
        /// </summary>
        public string CaminhoLogicoArquivo { get; set; }

        /// <summary>
        /// Nome de exibição do arquivo
        /// </summary>
        public string ArquivoNome { get; set; }

        /// <summary>
        /// Tamanho do arquivo
        /// </summary>
        public string Tamanho { get; set; }

        /// <summary>
        /// Formato do arquivo
        /// </summary>
        public string Formato { get; set; }
    }
}