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
    public class ExPrefeitosController : Controller
    {
        private int currentCodArea = 3;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<ExPrefeito> ExPrefeitoList = db.ExPrefeito.Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Pagina = db.Cidade.Select(x => x.ExPrefeito).FirstOrDefault();
            return View(ExPrefeitoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ExPrefeito ExPrefeito = db.ExPrefeito.Where(x => x.Id == id).FirstOrDefault();
            if (ExPrefeito == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;
            return View(ExPrefeito);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            return View();
        }

        [HttpPost]
        public ActionResult Create(ExPrefeito model, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioExPrefeitos());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if(model.Imagem.Contains("Erro:"))
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
                return RedirectToAction("Details", "ExPrefeitos", new { id = model.Id, retorno = "Ex-Prefeito cadastrado com sucesso!" });
            }

            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            ExPrefeito ExPrefeito = db.ExPrefeito.Where(x => x.Id == id).FirstOrDefault();
            if (ExPrefeito == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            return View(ExPrefeito);
        }

        [HttpPost]
        public ActionResult Edit(ExPrefeito model, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioExPrefeitos());
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
                return RedirectToAction("Details", "ExPrefeitos", new { id = model.Id, retorno = "Ex-Prefeito alterado com sucesso!" });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditaPage(string DescricaoExPrefeitos)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            Cidade RegistroExistente = db.Cidade.FirstOrDefault();
            RegistroExistente.ExPrefeito = DescricaoExPrefeitos;
            RegistroExistente.DataAtualizacao = DateTime.Now;
            db.Entry(RegistroExistente).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId<int>()), currentCodArea, TipoAcao.Edicao, "Cidade");
            return RedirectToAction("Index", "ExPrefeitos", new { retorno = "Registro Alterado com sucesso!" });
        }

        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ExPrefeito = db.ExPrefeito.Find(id);
            ExPrefeito.Status = (int)StatusPadrao.Excluido;
            db.Entry(ExPrefeito).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, ExPrefeito.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Ex-Prefeito excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ExPrefeito = db.ExPrefeito.Find(id);
            ExPrefeito.Status = (int)StatusPadrao.Inativo;
            db.Entry(ExPrefeito).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, ExPrefeito.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Ex-Prefeito bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ExPrefeito = db.ExPrefeito.Find(id);
            ExPrefeito.Status = (int)StatusPadrao.Ativo;
            db.Entry(ExPrefeito).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, ExPrefeito.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Ex-Prefeito desbloqueado com sucesso!" });
        }
    }
}