namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class TimelineVm
    {
        /// <summary>
        /// Ano
        /// </summary>
        public string Ano { get; set; }

        /// <summary>
        /// Descrição
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Arquivo De Imagem
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem 
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }
    }
}