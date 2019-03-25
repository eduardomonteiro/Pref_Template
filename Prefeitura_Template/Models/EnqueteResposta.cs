using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace Prefeitura_Template.Models
{
    [Table("EnqueteResposta")]
    public class EnqueteResposta
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Opção")]
        public int EnqueteOpcaoId { get; set; }

        [Display(Name = "Data da Resposta")]
        public DateTime DataResposta { get; set; }

        //[NotMapped]
        //public virtual Enquete Enquete
        //{
        //    get
        //    {
        //        using (var db = new ApplicationDbContext())
        //        {
        //            int EnqueteId = db.EnqueteOpcao.Where(x => x.Id == EnqueteOpcaoId).Select(x => x.EnqueteId).FirstOrDefault();
        //            return db.Enquete.Where(x => x.Id == EnqueteId).FirstOrDefault();
        //        }
        //    }
        //}
    }
}