using System;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class BuscaVm
    {
        /// <summary>
        /// Titulo do Registro
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descrição do Registro
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Slug do Registro
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Data da publicação do Registro
        /// </summary>
        public DateTime DataPublicacao { get; set; }

        /// <summary>
        /// Id da Area
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// Nome da Área
        /// </summary>
        public string AreaDescricao { get; set; }
    }
}