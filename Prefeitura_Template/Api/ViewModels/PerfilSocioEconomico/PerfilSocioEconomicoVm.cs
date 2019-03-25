using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class PerfilSocioEconomicoVm
    {
        /// <summary>
        /// Titulo do Perfil
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descricao do Perfil
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Perfil
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Perfil
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }

        /// <summary>
        /// Categoria do Perfil
        /// </summary>
        public virtual GenericVm PerfilSocioEconomicoCategoria { get; set; }
    }
}