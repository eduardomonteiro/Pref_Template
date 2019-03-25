using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class NoticiaListaVm
    {
        /// <summary>
        /// Título da noticia
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// SubTítulo da noticia
        /// </summary>
        public string SubTitulo { get; set; }

        /// <summary>
        /// Slug da noticia
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Data e hora da publicação da noticia
        /// </summary>
        public string DataPublicacao { get; set; }

        /// <summary>
        /// Texto da noticia
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// Categoria da noticia
        /// </summary>
        public GenericVm NoticiaCategoria { get; set; }

        /// <summary>
        /// Galeria da noticia
        /// </summary>
        public List<NoticiaGaleriaVm> NoticiaGaleria { get; set; }

        /// <summary>
        /// Link do Video
        /// </summary>
        public string LinkVideo { get; set; }

        /// <summary>
        /// Arquivo da Imagem da Noticia
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem da Noticia
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }

        /// <summary>
        /// Noticia destaque? True = Sim , False = Não
        /// </summary>
        public bool Destaque { get; set; }

        /// <summary>
        /// Tags da noticia
        /// </summary>
        public List<TagVm> Tag { get; set; }
    }
}