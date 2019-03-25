using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto da Secretaria
    /// </summary>
    public class SecretariaVinculadaVm
    {
        /// <summary>
        /// Nome da Secretaria
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Prefixo da Secretaria
        /// </summary>
        public string Prefixo { get; set; }
        
        /// <summary>
        /// Slug da Secretaria
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Nome do Responsável
        /// </summary>
        public string NomeResponsavel { get; set; }

        /// <summary>
        /// Cargo do Responsável
        /// </summary>
        public string Cargo { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// Arquivo do icone
        /// </summary>
        public string Icone { get; set; }

        /// <summary>
        /// Caminho completo do arquivo do icone
        /// </summary>
        public string CaminhoLogicoIcone { get; set; }

        /// <summary>
        /// Categoria do Secretaria
        /// </summary>
        public GenericVm SecretariaCategoria { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public List<TagVm> Tag { get; set; }

        /// <summary>
        /// Pricipais Serviços
        /// </summary>
        public string PincipaisServicos { get; set; }
    }
}