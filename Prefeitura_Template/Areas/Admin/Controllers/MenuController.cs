using System.Linq;
using System.Web.Mvc;
using Prefeitura_Template.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Prefeitura_Template.Areas.Admin.Enums;
using System.Collections.Generic;

namespace Prefeitura_Template.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        public ActionResult Menu()
        {
            using (var db = new ApplicationDbContext())
            {
                int UserId = User.Identity.GetUserId<int>();
                if (UserId != 0)
                {
                    Usuario Usuario = db.Usuario.Where(x => x.Id == UserId).FirstOrDefault();
                    ViewBag.thm = Usuario.Tema;
                    var AcessosPermitidos = db.Perfil_Area.Where(x => x.PerfilId == Usuario.PerfilId && x.Leitura)
                                                            .Select(x => x.AreaId)
                                                            .ToList();

                    var areas = db.Area.Where(a => a.Status == (int)StatusPadrao.Ativo && AcessosPermitidos.Contains(a.Id))
                                       .OrderBy(a => a.Ordem)
                                       .ThenBy(a => a.Descricao)
                                       .ToList();

                    return PartialView(areas);
                }

                List<Area> Retorno = new List<Area>();
                return PartialView(Retorno);
            }
        }

        public void GetTema()
        {
            using (var db = new ApplicationDbContext())
            {
                int UserId = User.Identity.GetUserId<int>();
                Usuario Usuario = db.Usuario.Where(x => x.Id == UserId).FirstOrDefault();
                Session.Add("thm", Usuario.Tema);
            }
        }


        [HttpPost]
        public ActionResult AlteraThm(string thm, string returnUrl)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var usuario = db.Usuario.Where(u => u.Email == User.Identity.Name).AsQueryable().FirstOrDefault();
                    if (usuario != null)
                    {
                        usuario.Tema = thm;
                        db.Entry(usuario).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["thm"] = thm;
                    }
                }
                return RedirectToLocal(returnUrl);
            }
            catch
            {
                return View();
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}