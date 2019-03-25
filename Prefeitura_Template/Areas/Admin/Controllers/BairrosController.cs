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
    public class BairrosController : Controller
    {
        private int currentCodArea = 19;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Bairro> BairroList = db.Bairro.Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            return View(BairroList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Bairro Bairro = db.Bairro.Where(x => x.Id == id).FirstOrDefault();
            if (Bairro == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            

            return View(Bairro);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Bairro model)
        {
            if (ModelState.IsValid)
            {
                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, model.Id.ToString());
                return RedirectToAction("Details", "Bairros", new { id = model.Id, retorno = "Bairro cadastrado com sucesso!" });
            }

            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Bairro Bairro = db.Bairro.Where(x => x.Id == id).FirstOrDefault();
            if (Bairro == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            return View(Bairro);
        }

        [HttpPost]
        public ActionResult Edit(Bairro model)
        {
            if (ModelState.IsValid)
            {
                model.DataAtualizacao = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "Bairros", new { id = model.Id, retorno = "Bairro alterado com sucesso!" });
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
            var Bairro = db.Bairro.Find(id);
            Bairro.Status = (int)StatusPadrao.Excluido;
            db.Entry(Bairro).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Bairro.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Bairro excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Bairro = db.Bairro.Find(id);
            Bairro.Status = (int)StatusPadrao.Inativo;
            db.Entry(Bairro).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Bairro.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Bairro bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Bairro = db.Bairro.Find(id);
            Bairro.Status = (int)StatusPadrao.Ativo;
            db.Entry(Bairro).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Bairro.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Bairro desbloqueado com sucesso!" });
        }
    }
}