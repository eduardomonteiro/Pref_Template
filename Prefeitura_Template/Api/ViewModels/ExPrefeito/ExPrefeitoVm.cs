using System.Collections.Generic;
using System.Web.Mvc;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class ExPrefeitoVm
    {
        /// <summary>
        /// HTML de descrição da página de Ex-Prefeitos
        /// </summary>
        [AllowHtml]
        public string ExPrefeito { get; set; }

        /// <summary>
        /// Listagem dos Ex-Prefeitos
        /// </summary>
        public List<ExPrefeitoListaVm> ListaExPrefeitos { get; set; }
    }
}