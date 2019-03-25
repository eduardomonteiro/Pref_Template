using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Areas.Admin.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "O campo E-mail é inválido.")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "O campo Provider é obrigatório.")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "O campo Código é obrigatório.")]
        [Display(Name = "Código")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Lembrar esse navegador?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "O campo E-mail é inválido.")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "O campo E-mail é inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [DataType(DataType.Password, ErrorMessage = "O campo Senha é inválido.")]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "O campo E-mail é inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Perfil é obrigatório.")]
        [Display(Name = "Perfil")]
        public int PerfilId { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessage = "O campo Senha é inválido.")]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password, ErrorMessage = "O campo Senha é inválido.")]
        [Display(Name = "Confirme sua senha")]
        [Compare("Password", ErrorMessage = "As senhas precisam ser iguais.")]
        public string ConfirmPassword { get; set; }
    }

    public class EditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "O campo E-mail é inválido.")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "O campo E-mail é inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessage = "O campo Senha é inválido.")]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password, ErrorMessage = "O campo Senha é inválido.")]
        [Display(Name = "Confirme sua senha")]
        [Compare("Password", ErrorMessage = "As senhas precisam ser iguais.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "O campo E-mail é inválido.")]
        public string Email { get; set; }
    }
}