using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

namespace Prefeitura_Template.Areas.Admin.Controllers
{
    public class LogsController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult LogsHistory(int AreaId, int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    ViewBag.logCriacao = db.Log.Include(l => l.Usuario)
                                                .Where(lo => lo.Detalhes == id.ToString() && 
                                                        lo.Acao == (int)TipoAcao.Adicao && 
                                                        lo.AreaId == AreaId)
                                                .AsQueryable()
                                                .FirstOrDefault();

                    ViewBag.logEdicao = db.Log.Include(l => l.Usuario)
                                                .Where(lo => lo.Detalhes == id.ToString() 
                                                        && lo.Acao == (int)TipoAcao.Edicao && 
                                                        lo.AreaId == AreaId)
                                                .OrderByDescending(l => l.DataCadastro)
                                                .Take(1)
                                                .AsQueryable()
                                                .SingleOrDefault();

                    ViewBag.logAtivacao = db.Log.Include(l => l.Usuario)
                                                .Where(lo => lo.Detalhes == id.ToString() && 
                                                        lo.AreaId == AreaId && 
                                                        lo.Acao == (int)TipoAcao.Ativar)
                                                .OrderByDescending(l => l.DataCadastro)
                                                .Take(1)
                                                .AsQueryable()
                                                .SingleOrDefault();

                    ViewBag.logDesativacao = db.Log.Include(l => l.Usuario)
                                                    .Where(lo => lo.Detalhes == id.ToString() && 
                                                            lo.AreaId == AreaId && 
                                                            lo.Acao == (int)TipoAcao.Desativar)
                                                    .OrderByDescending(l => l.DataCadastro)
                                                    .Take(1)
                                                    .AsQueryable()
                                                    .SingleOrDefault();
                }
                else
                {
                    ViewBag.logCriacao = db.Log.Include(l => l.Usuario)
                                                .Where(lo => lo.Acao == (int)TipoAcao.Adicao &&
                                                        lo.AreaId == AreaId)
                                                .AsQueryable()
                                                .FirstOrDefault();

                    ViewBag.logEdicao = db.Log.Include(l => l.Usuario)
                                                .Where(lo => lo.Acao == (int)TipoAcao.Edicao && 
                                                        lo.AreaId == AreaId)
                                                .OrderByDescending(l => l.DataCadastro)
                                                .Take(1)
                                                .AsQueryable()
                                                .SingleOrDefault();

                    ViewBag.logAtivacao = db.Log.Include(l => l.Usuario)
                                                .Where(lo => lo.AreaId == AreaId && 
                                                        lo.Acao == (int)TipoAcao.Ativar)
                                                .OrderByDescending(l => l.DataCadastro)
                                                .Take(1)
                                                .AsQueryable()
                                                .SingleOrDefault();

                    ViewBag.logDesativacao = db.Log.Include(l => l.Usuario)
                                                    .Where(lo => lo.AreaId == AreaId && 
                                                            lo.Acao == (int)TipoAcao.Desativar)
                                                    .OrderByDescending(l => l.DataCadastro)
                                                    .Take(1)
                                                    .AsQueryable()
                                                    .SingleOrDefault();

                }
                return PartialView();
            }
            catch
            {
                return PartialView();
            }
        }
    }
}