using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto do Informativo
    /// </summary>
    public class InformativoListaVm
    {
        /// <summary>
        /// Título do Informativo
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// SubTítulo do Informativo
        /// </summary>
        public string SubTitulo { get; set; }

        /// <summary>
        /// Slug do Informativo
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Data e hora da publicação do Informativo
        /// </summary>
        public string DataPublicacao { get; set; }

        /// <summary>
        /// Texto do Informativo
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// Informativo destaque? True = Sim , False = Não
        /// </summary>
        public bool Destaque { get; set; }

        /// <summary>
        /// Nome de exibição do informativo
        /// </summary>
        public string ArquivoNome { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo  do informativo
        /// </summary>
        public string Arquivo { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo do informativo
        /// </summary>
        public string CaminhoLogicoArquivo { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Informativo
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Informativo
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }

        /// <summary>
        /// Categoria do Informativo
        /// </summary>
        public GenericVm InformativoCategoria { get; set; }

        /// <summary>
        /// Galeria do Informativo
        /// </summary>
        public List<InformativoGaleriaVm> InformativoGaleria { get; set; }

        /// <summary>
        /// Link do Video
        /// </summary>
        public string LinkVideo { get; set; }

        /// <summary>
        /// Tags do Informativo
        /// </summary>
        public List<TagVm> Tag { get; set; }
    }
}