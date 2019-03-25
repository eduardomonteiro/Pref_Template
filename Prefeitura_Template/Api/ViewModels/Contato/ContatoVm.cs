using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class ContatoVm
    {
        /// <summary>
        /// Nome
        /// </summary>
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(300, ErrorMessage = "{0}: Limite de 300 caracteres!")]
        public string Nome { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(300, ErrorMessage = "{0}: Limite de 300 caracteres!")]
        public string Email { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "{0}: Limite de 20 caracteres!")]
        public string CPF { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        [StringLength(20, ErrorMessage = "{0}: Limite de 20 caracteres!")]
        public string Telefone { get; set; }

        /// <summary>
        /// Celular
        /// </summary>
        [StringLength(20, ErrorMessage = "{0}: Limite de 20 caracteres!")]
        public string Celular { get; set; }

        /// <summary>
        /// Id do Bairro
        /// </summary>
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        public int BairroId { get; set; }

        /// <summary>
        /// Id do tipo do Contato
        /// </summary>
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        public int ContatoTipoId { get; set; }

        /// <summary>
        /// Mensagem
        /// </summary>
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        public string Mensagem { get; set; }
    }
}