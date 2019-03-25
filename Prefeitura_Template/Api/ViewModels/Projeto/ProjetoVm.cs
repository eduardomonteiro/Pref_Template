using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class ProjetoVm
    {
        /// <summary>
        /// Titulo do Projeto
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// SubTitulo do Projeto
        /// </summary>
        public string SubTitulo { get; set; }

        /// <summary>
        /// Descriçao do Projeto
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Slug do Projeto
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Secretaria do Projeto
        /// </summary>
        public SecretariaVinculadaVm Secretaria { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Projeto
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Projeto
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }

        /// <summary>
        /// Link do Video
        /// </summary>
        public string LinkVideo { get; set; }

        /// <summary>
        /// Link Externo do Projeto
        /// </summary>
        public string LinkExterno { get; set; }

        ///// <summary>
        ///// Nome de exibição do Projeto
        ///// </summary>
        //public string ArquivoNome { get; set; }

        ///// <summary>
        ///// Caminho Completo do Arquivo de Video do Projeto
        ///// </summary>
        //public string Arquivo { get; set; }

        ///// <summary>
        ///// Caminho Completo do Arquivo do Projeto
        ///// </summary>
        //public string CaminhoLogicoArquivo { get; set; }

        ///// <summary>
        ///// Arquivos
        ///// </summary>
        public List<ProjetoArquivoVm> ProjetoArquivo { get; set; }

        /// <summary>
        /// Listagem das Noticias
        /// </summary>
        public virtual List<NoticiaVinculadaVm> Noticias { get; set; }

        /// <summary>
        /// Listagem dos Eventos
        /// </summary>
        public virtual List<EventoVinculadoVm> Eventos { get; set; }

        /// <summary>
        /// Categoria do Projeto
        /// </summary>
        public GenericVm ProjetoCategoria { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public List<TagVm> Tag { get; set; }
    }
}