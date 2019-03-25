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
    public class PerfilSocioEconomicoController : Controller
    {
        private int currentCodArea = 25;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<PerfilSocioEconomico> PerfilSocioEconomicoList = db.PerfilSocioEconomico.Include(x => x.PerfilSocioEconomicoCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = PerfilSocioEconomicoList.OrderBy(x => x.PerfilSocioEconomicoCategoria.Descricao).Select(x => x.PerfilSocioEconomicoCategoria.Descricao).ToList().Distinct();
            return View(PerfilSocioEconomicoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            PerfilSocioEconomico PerfilSocioEconomico = db.PerfilSocioEconomico.Include(x => x.PerfilSocioEconomicoCategoria).Where(x => x.Id == id).FirstOrDefault();
            if (PerfilSocioEconomico == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(PerfilSocioEconomico);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.PerfilSocioEconomicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult Create(PerfilSocioEconomico model, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioPerfilSocioEconomico());
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
                return RedirectToAction("Details", "PerfilSocioEconomico", new { id = model.Id, retorno = "Perfil cadastrado com sucesso!" });
            }

            ViewBag.Categorias = new SelectList(db.PerfilSocioEconomicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.PerfilSocioEconomicoCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            PerfilSocioEconomico PerfilSocioEconomico = db.PerfilSocioEconomico.Where(x => x.Id == id).FirstOrDefault();

            if (PerfilSocioEconomico == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Categorias = new SelectList(db.PerfilSocioEconomicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", PerfilSocioEconomico.PerfilSocioEconomicoCategoriaId);

            return View(PerfilSocioEconomico);
        }

        [HttpPost]
        public ActionResult Edit(PerfilSocioEconomico model, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioPerfilSocioEconomico());
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
                return RedirectToAction("Details", "PerfilSocioEconomico", new { id = model.Id, retorno = "Perfil alterado com sucesso!" });
            }

            ViewBag.Categorias = new SelectList(db.PerfilSocioEconomicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.PerfilSocioEconomicoCategoriaId);

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
            var PerfilSocioEconomico = db.PerfilSocioEconomico.Find(id);
            PerfilSocioEconomico.Status = (int)StatusPadrao.Excluido;
            db.Entry(PerfilSocioEconomico).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, PerfilSocioEconomico.Id.ToString());
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
            var PerfilSocioEconomico = db.PerfilSocioEconomico.Find(id);
            PerfilSocioEconomico.Status = (int)StatusPadrao.Inativo;
            db.Entry(PerfilSocioEconomico).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, PerfilSocioEconomico.Id.ToString());
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
            var PerfilSocioEconomico = db.PerfilSocioEconomico.Find(id);
            PerfilSocioEconomico.Status = (int)StatusPadrao.Ativo;
            db.Entry(PerfilSocioEconomico).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, PerfilSocioEconomico.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Perfil desbloqueado com sucesso!" });
        }
    }
}