using System.ComponentModel.DataAnnotations;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de retorno
    /// </summary>
    public class NewsLetterVm
    {
        /// <summary>
        /// E-mail
        /// </summary>
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(100, ErrorMessage = "{0}: Limite de 100 caracteres!")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(300, ErrorMessage = "{0}: Limite de 300 caracteres!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        /// <summary>
        /// Sexo
        /// </summary>
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(50, ErrorMessage = "{0}: Limite de 50 caracteres!")]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }
    }
}