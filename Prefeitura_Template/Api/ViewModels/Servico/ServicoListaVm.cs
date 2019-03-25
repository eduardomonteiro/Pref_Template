using System.Collections.Generic;
namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class ServicoListaVm
    {
        /// <summary>
        /// Nome do serviço
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Chamada do serviço
        /// </summary>
        public string Chamada { get; set; }

        /// <summary>
        /// Slug do serviço
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Categoria do servico
        /// </summary>
        public GenericVm ServicoCategoria { get; set; }

        /// <summary>
        /// Arquivo do icone
        /// </summary>
        public string Icone { get; set; }

        /// <summary>
        /// Serviço destaque? True = Sim , False = Não
        /// </summary>
        public bool Destaque { get; set; }

        /// <summary>
        /// Caminho completo do arquivo do icone
        /// </summary>
        public string CaminhoLogicoIcone { get; set; }

        /// <summary>
        /// Descricao do serviço
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Link Externo
        /// </summary>
        public string LinkExterno { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Serviço
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Serviço
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }

        /// <summary>
        /// Arquivos
        /// </summary>
        public List<ServicoArquivoVm> ServicoArquivo { get; set; }

        /// <summary>
        /// Pins
        /// </summary>
        public List<ServicoPinVm> ServicoPin { get; set; }
    }
}