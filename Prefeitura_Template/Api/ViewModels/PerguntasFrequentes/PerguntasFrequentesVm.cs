namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class PerguntasFrequentesVm
    {
        /// <summary>
        /// Titulo da Pergunta Frequente
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descrição da Pergunta Frequente
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Categoria da Pergunta Frequente
        /// </summary>
        public GenericVm PerguntasFrequentesCategoria { get; set; }
    }
}