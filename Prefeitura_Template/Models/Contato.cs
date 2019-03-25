using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Areas.Admin.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Prefeitura_Template.Models
{
    [Table("Contato")]
    public class Contato : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(300, ErrorMessage = "{0}: Limite de 300 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(300, ErrorMessage = "{0}: Limite de 300 caracteres!")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "{0}: Limite de 20 caracteres!")]
        public string CPF { get; set; }

        [StringLength(20, ErrorMessage = "{0}: Limite de 20 caracteres!")]
        public string Telefone { get; set; }

        [StringLength(20, ErrorMessage = "{0}: Limite de 20 caracteres!")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Bairro")]
        public int BairroId { get; set; }

        public virtual Bairro Bairro { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Tipo")]
        public int ContatoTipoId { get; set; }

        public virtual ContatoTipo ContatoTipo { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        public string Mensagem { get; set; }

        [Display(Name = "Status")]
        public int StatusFaleConosco { get; set; }

        public string Resposta { get; set; }

        public int RespostaUsuarioId { get; set; }

        [NotMapped]
        public Usuario UsuarioReposta
        {
            get
            {
                if (RespostaUsuarioId != null && RespostaUsuarioId != 0)
                {
                    using (var db = new ApplicationDbContext())
                    {
                        return db.Usuario.Where(x => x.Id == RespostaUsuarioId).FirstOrDefault();
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public DateTime ? DataResposta { get; set; }

        [NotMapped]
        public string NomeStatusFaleConosco { get { return EnumExtensions.GetEnumDisplayName(typeof(StatusFaleConosco), StatusFaleConosco); } }
    }
}