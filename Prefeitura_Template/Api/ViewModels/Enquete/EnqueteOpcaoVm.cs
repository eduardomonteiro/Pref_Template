namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Opcoes
    /// </summary>
    public class EnqueteOpcaoVm
    {
        /// <summary>
        /// Id da Opcao
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Opção
        /// </summary>
        public string Opcao { get; set; }

        /// <summary>
        /// Quantidade de resposta com essa opção
        /// </summary>
        public int QuantidadeResposta { get; set; }

        /// <summary>
        /// Porcentagem de resposta com essa opção
        /// </summary>
        public double PorcentagemResposta { get; set; }
    }
}