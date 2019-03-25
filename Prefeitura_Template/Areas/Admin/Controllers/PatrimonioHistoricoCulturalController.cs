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
    public class PatrimonioHistoricoCulturalController : Controller
    {
        private int currentCodArea = 24;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<PatrimonioHistoricoCultural> PatrimonioHistoricoCulturalList = db.PatrimonioHistoricoCultural.Include(x => x.PatrimonioHistoricoCulturalCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = PatrimonioHistoricoCulturalList.OrderBy(x => x.PatrimonioHistoricoCulturalCategoria.Descricao).Select(x => x.PatrimonioHistoricoCulturalCategoria.Descricao).ToList().Distinct();
            return View(PatrimonioHistoricoCulturalList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            PatrimonioHistoricoCultural PatrimonioHistoricoCultural = db.PatrimonioHistoricoCultural.Include(x => x.PatrimonioHistoricoCulturalCategoria).Where(x => x.Id == id).FirstOrDefault();
            if (PatrimonioHistoricoCultural == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(PatrimonioHistoricoCultural);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.PatrimonioHistoricoCulturalCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult Create(PatrimonioHistoricoCultural model, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioPatrimonio());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        ModelState.AddModelError("Imagem", model.Imagem);
                        return View(model);
                    }
                }

                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, model.Id.ToString());
                return RedirectToAction("Details", "PatrimonioHistoricoCultural", new { id = model.Id, retorno = "Patrimônio cadastrado com sucesso!" });
            }

            ViewBag.Categorias = new SelectList(db.PatrimonioHistoricoCulturalCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.PatrimonioHistoricoCulturalCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            PatrimonioHistoricoCultural PatrimonioHistoricoCultural = db.PatrimonioHistoricoCultural.Where(x => x.Id == id).FirstOrDefault();
            if (PatrimonioHistoricoCultural == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            ViewBag.Categorias = new SelectList(db.PatrimonioHistoricoCulturalCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", PatrimonioHistoricoCultural.PatrimonioHistoricoCulturalCategoriaId);

            return View(PatrimonioHistoricoCultural);
        }

        [HttpPost]
        public ActionResult Edit(PatrimonioHistoricoCultural model, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioPatrimonio());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        ModelState.AddModelError("Imagem", model.Imagem);
                        return View(model);
                    }
                }

                model.DataAtualizacao = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "PatrimonioHistoricoCultural", new { id = model.Id, retorno = "Patrimônio alterado com sucesso!" });
            }

            ViewBag.Categorias = new SelectList(db.PatrimonioHistoricoCulturalCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.PatrimonioHistoricoCulturalCategoriaId);

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
            var PatrimonioHistoricoCultural = db.PatrimonioHistoricoCultural.Find(id);
            PatrimonioHistoricoCultural.Status = (int)StatusPadrao.Excluido;
            db.Entry(PatrimonioHistoricoCultural).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, PatrimonioHistoricoCultural.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Patrimônio excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var PatrimonioHistoricoCultural = db.PatrimonioHistoricoCultural.Find(id);
            PatrimonioHistoricoCultural.Status = (int)StatusPadrao.Inativo;
            db.Entry(PatrimonioHistoricoCultural).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, PatrimonioHistoricoCultural.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Patrimônio bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var PatrimonioHistoricoCultural = db.PatrimonioHistoricoCultural.Find(id);
            PatrimonioHistoricoCultural.Status = (int)StatusPadrao.Ativo;
            db.Entry(PatrimonioHistoricoCultural).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, PatrimonioHistoricoCultural.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Patrimônio desbloqueado com sucesso!" });
        }
    }
}