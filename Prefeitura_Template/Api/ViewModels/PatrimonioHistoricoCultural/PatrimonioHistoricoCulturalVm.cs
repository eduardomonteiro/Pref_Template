using System.Collections.Generic;
using System.Web.Mvc;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class PatrimonioHistoricoCulturalVm
    {
        /// <summary>
        /// HTML da página de "Patrimônio Histórico Cultural"
        /// </summary>
        [AllowHtml]
        public string PatrimonioHistoricoCultural { get; set; }

        /// <summary>
        /// Listagem dos Patrimonios
        /// </summary>
        public List<PatrimonioHistoricoCulturalListaVm> ListaPatrimonioHistoricoCultural { get; set; }
    }
}