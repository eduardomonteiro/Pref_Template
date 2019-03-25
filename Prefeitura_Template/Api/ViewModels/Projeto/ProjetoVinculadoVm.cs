using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto do Projeto
    /// </summary>
    public class ProjetoVinculadoVm
    {
        /// <summary>
        /// Slug do Projeto
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Nome do Projeto
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Subtitulo do Projeto
        /// </summary>
        public string SubTitulo { get; set; }

        /// <summary>
        /// Imagem do Projeto
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo da Imagem do Projeto
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }

        /// <summary>
        /// Tags que fizeram o vínculo
        /// </summary>
        public List<TagVm> Tag { get; set; }

        /// <summary>
        /// Categoria do Projeto
        /// </summary>
        public GenericVm ProjetoCategoria { get; set; }
    }
}