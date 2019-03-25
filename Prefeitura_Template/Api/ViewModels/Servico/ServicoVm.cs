using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class ServicoVm
    {
        /// <summary>
        /// Serviços que são destaques
        /// </summary>
        public List<ServicoListaVm> ServicoDestaque { get; set; }

        /// <summary>
        /// Serviços que nao sao destaques
        /// </summary>
        public List<ServicoListaVm> Servico { get; set; }
    }
}