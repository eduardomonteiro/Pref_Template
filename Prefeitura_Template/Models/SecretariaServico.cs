using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;


namespace Prefeitura_Template.Models
{
    [Table("SecretariaServico")]
    public class SecretariaServico : EntidadePadrao
    {
        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Secretaria")]
        public int SecretariaId { get; set; }

        [Required(ErrorMessage = "{0}: Campo Obrigatório")]
        [Display(Name = "Serviço")]
        public int ServicoId { get; set; }

        [NotMapped]
        public string ServicoNome
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    return db.Servico.Where(x => x.Id == ServicoId).Select(x => x.Nome).FirstOrDefault();
                }
            }
        }

        [NotMapped]
        public string SecretariaNome
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    var Item = db.Secretaria.Include(y => y.SecretariaNomePrefixo).Where(x => x.Id == SecretariaId).FirstOrDefault();
                    return Item.NomeComPrefixo;
                }
            }
        }

        public int Ordem { get; set; }
    }
}