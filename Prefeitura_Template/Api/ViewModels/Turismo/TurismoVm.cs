using System.Collections.Generic;
using System.Web.Mvc;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class TurismoVm
    {
        /// <summary>
        /// HTML da página de "Turimos"
        /// </summary>
        [AllowHtml]
        public string Turismo { get; set; }

        /// <summary>
        /// Listagem de Pontos Turisticos
        /// </summary>
        public List<TurismoListaVm> ListaTurismo { get; set; }
    }
}