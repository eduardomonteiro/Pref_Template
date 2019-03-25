using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto da Noticia Vinculada
    /// </summary>
    public class NoticiaVinculadaVm
    {
        /// <summary>
        /// Slug da Noticia
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Titulo da Noticia
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Sub-Titulo da Noticia
        /// </summary>
        public string SubTitulo { get; set; }

        /// <summary>
        /// Data e Horario da publicação
        /// </summary>
        public string DataPublicacao { get; set; }

        /// <summary>
        /// Noticia Destaque? True = Sim , False = Não
        /// </summary>
        public bool Destaque { get; set; }

        /// <summary>
        /// Arquivo da Imagem da Noticia
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem da Noticia
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }

        /// <summary>
        /// Tags que fizeram o vínculo
        /// </summary>
        public List<TagVm> Tag { get; set; }

        /// <summary>
        /// Categoria da Noticia
        /// </summary>
        public GenericVm NoticiaCategoria { get; set; }

        /// <summary>
        /// Galeria da noticia
        /// </summary>
        public List<NoticiaGaleriaVm> NoticiaGaleria { get; set; }
    }
}