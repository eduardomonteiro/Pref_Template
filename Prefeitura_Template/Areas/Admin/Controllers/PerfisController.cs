using Microsoft.AspNet.Identity;
using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Areas.Admin.Utils;
using Prefeitura_Template.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Prefeitura_Template.Areas.Admin.Controllers
{
    [Authorize]
    public class PerfisController : Controller
    {
        private int currentCodArea = 4;
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;

            List<Perfil> PerfilList = new List<Perfil>();

            int UsarioLogadoId = User.Identity.GetUserId<int>();
            Usuario UsuarioLogado = db.Usuario.Where(x => x.Id == UsarioLogadoId).FirstOrDefault();
            if (UsuarioLogado.PerfilId == 1)
            {
                PerfilList = db.Perfil.Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            }
            else
            {
                PerfilList = db.Perfil.Where(x => x.Id != 1 && x.Status != (int)StatusPadrao.Excluido).ToList();
            }

            return View(PerfilList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Perfil Perfil = db.Perfil.Where(x => x.Id == id).FirstOrDefault();

            if (Perfil == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            if(id == 1)
            {
                int UsarioLogadoId = User.Identity.GetUserId<int>();
                Usuario UsuarioLogado = db.Usuario.Where(x => x.Id == UsarioLogadoId).FirstOrDefault();
                if (UsuarioLogado.PerfilId != 1)
                {
                    return RedirectToAction("Index", "Perfis");
                }
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(Perfil);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            var areas = db.Area.ToList();

            var pu = new List<Perfil_Area>();

            foreach (var area in areas)
            {
                pu.Add(new Perfil_Area()
                {
                    Area = area,
                    AreaId = area.Id,
                    Criacao = false,
                    Alteracao = false,
                    Exclusao = false,
                    Leitura = false,
                    PerfilId = 0
                });
            }

            Perfil Perfil = new Perfil();

            Perfil.Perfil_Area = pu.OrderBy(x => x.Area.Ordem).ToList();

            return View(Perfil);
        }

        [HttpPost]
        public ActionResult Create(Perfil model , List<Perfil_Area> pm)
        {
            if (ModelState.IsValid)
            {
                foreach (var permissao in pm)
                {
                    db.Entry(permissao).State = EntityState.Added;
                }

                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, model.Id.ToString());
                return RedirectToAction("Details", "Perfis", new { id = model.Id, retorno = "Perfil cadastrado com sucesso!" });
            }

            var areas = db.Area.ToList();

            var pu = new List<Perfil_Area>();

            foreach (var area in areas)
            {
                pu.Add(new Perfil_Area()
                {
                    Area = area,
                    AreaId = area.Id,
                    Criacao = pm.Where(x => x.AreaId == area.Id).Select(x => x.Criacao).FirstOrDefault(),
                    Alteracao = pm.Where(x => x.AreaId == area.Id).Select(x => x.Alteracao).FirstOrDefault(),
                    Exclusao = pm.Where(x => x.AreaId == area.Id).Select(x => x.Exclusao).FirstOrDefault(),
                    Leitura = pm.Where(x => x.AreaId == area.Id).Select(x => x.Leitura).FirstOrDefault(),
                    PerfilId = 0
                });
            }

            model.Perfil_Area = pu.OrderBy(x => x.Area.Ordem).ToList();
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Perfil Perfil = db.Perfil.Where(x => x.Id == id).FirstOrDefault();

            if (Perfil == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            if (id == 1)
            {
                int UsarioLogadoId = User.Identity.GetUserId<int>();
                Usuario UsuarioLogado = db.Usuario.Where(x => x.Id == UsarioLogadoId).FirstOrDefault();
                if (UsuarioLogado.PerfilId != 1)
                {
                    return RedirectToAction("Index", "Perfis");
                }
            }

            var areas = db.Area.ToList();

            var pu = db.Perfil_Area.Where(x => x.PerfilId == id).ToList();

            foreach (var a in areas)
            {
                if (!pu.Select(x => x.AreaId).Contains(a.Id))
                {
                    pu.Add(new Perfil_Area()
                    {
                        Area = a,
                        AreaId = a.Id,
                        Criacao = false,
                        Alteracao = false,
                        Exclusao = false,
                        Leitura = false,
                        PerfilId = id
                    });
                }
                else
                {
                    foreach (var p in pu)
                    {
                        if (p.AreaId == a.Id)
                        {
                            p.Area = a;
                        }
                    }
                }
            }
            
            Perfil.Perfil_Area = pu.OrderBy(x => x.Area.Ordem).ToList();

            return View(Perfil);
        }

        [HttpPost]
        public ActionResult Edit(Perfil model, List<Perfil_Area> pm)
        {
            if (ModelState.IsValid)
            {
                foreach (var permissao in pm)
                {
                    if (permissao.Id == 0)
                    {
                        db.Entry(permissao).State = EntityState.Added;
                    }
                    else
                    {
                        db.Entry(permissao).State = EntityState.Modified;
                    }
                }

                model.DataAtualizacao = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                db.Entry(model).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "Perfis", new { id = model.Id, retorno = "Perfil alterado com sucesso!" });
            }

            var areas = db.Area.ToList();

            var pu = new List<Perfil_Area>();

            foreach (var area in areas)
            {
                pu.Add(new Perfil_Area()
                {
                    Area = area,
                    AreaId = area.Id,
                    Criacao = pm.Where(x => x.AreaId == area.Id).Select(x => x.Criacao).FirstOrDefault(),
                    Alteracao = pm.Where(x => x.AreaId == area.Id).Select(x => x.Alteracao).FirstOrDefault(),
                    Exclusao = pm.Where(x => x.AreaId == area.Id).Select(x => x.Exclusao).FirstOrDefault(),
                    Leitura = pm.Where(x => x.AreaId == area.Id).Select(x => x.Leitura).FirstOrDefault(),
                    PerfilId = 0
                });
            }

            model.Perfil_Area = pu.OrderBy(x => x.Area.Ordem).ToList();

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Perfil = db.Perfil.Find(id);
            Perfil.Status = (int)StatusPadrao.Excluido;
            db.Entry(Perfil).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Perfil.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Perfil excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Perfil = db.Perfil.Find(id);
            Perfil.Status = (int)StatusPadrao.Inativo;
            db.Entry(Perfil).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Perfil.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Perfil bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Perfil = db.Perfil.Find(id);
            Perfil.Status = (int)StatusPadrao.Ativo;
            db.Entry(Perfil).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Perfil.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Perfil desbloqueado com sucesso!" });
        }
    }
}