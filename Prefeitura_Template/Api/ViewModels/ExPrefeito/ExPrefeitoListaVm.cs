namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Lista de Ex-Prefeitos
    /// </summary>
    public class ExPrefeitoListaVm
    {
        /// <summary>
        /// Nome do Ex-Prefeito
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Data Inicial da Legislatura do Ex-Prefeito
        /// </summary>
        public string DataInicioLegislatura { get; set; }

        /// <summary>
        /// Data Final da Legislatura do Ex-Prefeito
        /// </summary>
        public string DataFimLegislatura { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Ex-Prefeito
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Ex-Prefeito
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }
    }
}