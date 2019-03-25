using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Prefeitura_Template.Models
{
    [Table("UsuarioSecretaria")]
    public class UsuarioSecretaria : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Secretaria")]
        public int SecretariaId { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Usuário")]
        public int UsuarioId { get; set; }

        [NotMapped]
        public string SecretariaNome
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    return db.Secretaria.Where(x => x.Id == SecretariaId).Select(x => x.Nome).FirstOrDefault();
                }
            }
        }

        [NotMapped]
        public string Prefixo
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    return db.Secretaria.Where(x => x.Id == SecretariaId).Select(x => x.SecretariaNomePrefixo.Descricao).FirstOrDefault();
                }
            }
        }
    }
}