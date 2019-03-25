using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prefeitura_Template.Models
{
    [Table("Log")]
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int AreaId { get; set; }

        public int Acao { get; set; }

        public string Detalhes { get; set; }

        public DateTime? DataAtualizacao { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Area Area { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}