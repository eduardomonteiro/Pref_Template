namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class InformativoGaleriaVm
    {
        /// <summary>
        /// Arquivo
        /// </summary>
        public string Midia { get; set; }

        /// <summary>
        /// Caminho Completo do arquivo
        /// </summary>
        public string CaminhoLogicoMidia { get; set; }

        /// <summary>
        /// Credito do arquivo
        /// </summary>
        public string Credito { get; set; }

        /// <summary>
        /// Legenda do arquivo
        /// </summary>
        public string Legenda { get; set; }
    }
}