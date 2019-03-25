using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("EnqueteOpcao")]
    public class EnqueteOpcao : EntidadePadrao
    {
        [StringLength(500, ErrorMessage = "{0}: Limite de 500 caracteres!")]
        public string Opcao { get; set; }

        [Display(Name = "Enquete")]
        public int EnqueteId { get; set; }

        public int Ordem { get; set; }

        public virtual ICollection<EnqueteResposta> EnqueteResposta { get; set; }

        [NotMapped]
        public int QuantidadeResposta
        {
            get
            {
                using(var db = new ApplicationDbContext())
                {
                    return db.EnqueteResposta.Where(x => x.EnqueteOpcaoId == Id).Count();
                }
            }
        }

        [NotMapped]
        public double PorcentagemResposta
        {
            get
            {
                using (var db = new ApplicationDbContext())
                {
                    List<int> OpcoesIds = db.EnqueteOpcao.Where(x => x.EnqueteId == EnqueteId).Select(x => x.Id).ToList();
                    double TotalResposta = db.EnqueteResposta.Where(x => OpcoesIds.Contains(x.EnqueteOpcaoId)).Count();

                    if(QuantidadeResposta == 0 || TotalResposta == 0)
                    {
                        return 0;
                    }

                    double Porcentagem = Math.Round(((QuantidadeResposta * 100) / TotalResposta),2);
                    return Porcentagem;
                }
            }
        }
    }
}