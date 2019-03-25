using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class NoticiaVm
    {
        /// <summary>
        /// Noticias que são destaques
        /// </summary>
        public List<NoticiaListaVm> NoticiaDestaque { get; set; }

        /// <summary>
        /// Noticias que nao sao destaques
        /// </summary>
        public List<NoticiaListaVm> Noticia { get; set; }
    }
}