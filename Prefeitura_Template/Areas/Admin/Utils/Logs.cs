using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prefeitura_Template.Areas.Admin.Utils
{
    class Logs
    {
        public static void salvarLog(int UsuarioId, int AreaId, TipoAcao tipo, string detalhes)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Log objLog = new Log();
                objLog.AreaId = AreaId;
                objLog.UsuarioId = UsuarioId;
                objLog.Acao = (int)tipo;
                objLog.DataCadastro = DateTime.Now;
                objLog.Detalhes = detalhes;
                db.Log.Add(objLog);
                db.SaveChanges();
            }
        }
    }

    public class LogsAllViewModel
    {
        public Log logViewModelCriacao { get; set; }
        public Log logViewModelEdicao { get; set; }
        public Log logViewModelAtivacao { get; set; }

    }
}
