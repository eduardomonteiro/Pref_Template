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
    public class PerguntasFrequentesController : Controller
    {
        private int currentCodArea = 26;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<PerguntasFrequentes> PerguntasFrequentesList = db.PerguntasFrequentes.Include(x => x.PerguntasFrequentesCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = PerguntasFrequentesList.OrderBy(x => x.PerguntasFrequentesCategoria.Descricao).Select(x => x.PerguntasFrequentesCategoria.Descricao).ToList().Distinct();
            return View(PerguntasFrequentesList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            PerguntasFrequentes PerguntasFrequentes = db.PerguntasFrequentes.Include(x => x.PerguntasFrequentesCategoria).Where(x => x.Id == id).FirstOrDefault();
            if (PerguntasFrequentes == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(PerguntasFrequentes);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.PerguntasFrequentesCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult Create(PerguntasFrequentes model)
        {
            if (ModelState.IsValid)
            {
                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, model.Id.ToString());
                return RedirectToAction("Details", "PerguntasFrequentes", new { id = model.Id, retorno = "Pergunta Frequente cadastrado com sucesso!" });
            }

            ViewBag.Categorias = new SelectList(db.PerguntasFrequentesCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao",model.PerguntasFrequentesCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            PerguntasFrequentes PerguntasFrequentes = db.PerguntasFrequentes.Where(x => x.Id == id).FirstOrDefault();

            if (PerguntasFrequentes == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Categorias = new SelectList(db.PerguntasFrequentesCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", PerguntasFrequentes.PerguntasFrequentesCategoriaId);

            return View(PerguntasFrequentes);
        }

        [HttpPost]
        public ActionResult Edit(PerguntasFrequentes model)
        {
            if (ModelState.IsValid)
            {
                model.DataAtualizacao = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "PerguntasFrequentes", new { id = model.Id, retorno = "Pergunta Frequente alterado com sucesso!" });
            }

            ViewBag.Categorias = new SelectList(db.PerguntasFrequentesCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.PerguntasFrequentesCategoriaId);
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
            var PerguntasFrequentes = db.PerguntasFrequentes.Find(id);
            PerguntasFrequentes.Status = (int)StatusPadrao.Excluido;
            db.Entry(PerguntasFrequentes).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, PerguntasFrequentes.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Pergunta Frequente excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var PerguntasFrequentes = db.PerguntasFrequentes.Find(id);
            PerguntasFrequentes.Status = (int)StatusPadrao.Inativo;
            db.Entry(PerguntasFrequentes).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, PerguntasFrequentes.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Pergunta Frequente bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var PerguntasFrequentes = db.PerguntasFrequentes.Find(id);
            PerguntasFrequentes.Status = (int)StatusPadrao.Ativo;
            db.Entry(PerguntasFrequentes).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, PerguntasFrequentes.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Pergunta Frequente desbloqueado com sucesso!" });
        }
    }
}