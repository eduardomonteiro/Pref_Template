using Microsoft.AspNet.Identity;
using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Areas.Admin.Utils;
using Prefeitura_Template.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Prefeitura_Template.Areas.Admin.Controllers
{
    [Authorize]
    public class AaZController : Controller
    {
        private int currentCodArea = 6;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<AaZ> AaZList = db.AaZ.Include(x => x.AaZCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = AaZList.OrderBy(x => x.AaZCategoria.Descricao).Select(x => x.AaZCategoria.Descricao).ToList().Distinct();
            return View(AaZList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            AaZ AaZ = db.AaZ.Include(x => x.AaZCategoria).Where(x => x.Id == id).FirstOrDefault();
            if (AaZ == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(AaZ);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.AaZCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult Create(AaZ model)
        {
            if (ModelState.IsValid)
            {
                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, model.Id.ToString());
                return RedirectToAction("Details", "AaZ", new { id = model.Id, retorno = "Registro cadastrado com sucesso!" });
            }

            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            AaZ AaZ = db.AaZ.Where(x => x.Id == id).FirstOrDefault();

            if (AaZ == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            ViewBag.Categorias = new SelectList(db.AaZCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", AaZ.AaZCategoriaId);

            return View(AaZ);
        }

        [HttpPost]
        public ActionResult Edit(AaZ model)
        {
            if (ModelState.IsValid)
            {
                model.DataAtualizacao = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "AaZ", new { id = model.Id, retorno = "Registro alterado com sucesso!" });
            }

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
            var AaZ = db.AaZ.Find(id);
            AaZ.Status = (int)StatusPadrao.Excluido;
            db.Entry(AaZ).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, AaZ.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Registro excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var AaZ = db.AaZ.Find(id);
            AaZ.Status = (int)StatusPadrao.Inativo;
            db.Entry(AaZ).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, AaZ.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Registro bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var AaZ = db.AaZ.Find(id);
            AaZ.Status = (int)StatusPadrao.Ativo;
            db.Entry(AaZ).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, AaZ.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Registro desbloqueado com sucesso!" });
        }
    }
}