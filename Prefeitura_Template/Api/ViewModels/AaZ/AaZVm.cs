using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de retorno
    /// </summary>
    public class AaZVm
    {
        /// <summary>
        /// Titulo
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Categoria
        /// </summary>
        public GenericVm AaZCategoria { get; set; }
    }
}