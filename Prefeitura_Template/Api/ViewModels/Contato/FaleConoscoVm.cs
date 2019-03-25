using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class FaleConoscoVm
    {
        /// <summary>
        /// Telefone da Prefeitura
        /// </summary>
        public string PrefeituraTelefone { get; set; }

        /// <summary>
        /// Telefone da Prefeitura
        /// </summary>
        public string PrefeituraEmail { get; set; }

        /// <summary>
        /// Telefone do Gabinete
        /// </summary>
        public string GabineteTelefone { get; set; }

        /// <summary>
        /// E-mail do Gabinete
        /// </summary>
        public string GabineteEmail { get; set; }
    }
}