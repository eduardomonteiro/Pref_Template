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
    public class EnquetesController : Controller
    {
        private int currentCodArea = 20;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Enquete> EnqueteList = db.Enquete.Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            return View(EnqueteList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Enquete Enquete = db.Enquete.Include(x => x.EnqueteOpcao).Where(x => x.Id == id).FirstOrDefault();
            if (Enquete == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            

            return View(Enquete);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Enquete model, List<string> Opcoes)
        {
            if (ModelState.IsValid)
            {
                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();

                int Ordem = 0;
                foreach (var r in Opcoes)
                {
                    EnqueteOpcao Opcao = new EnqueteOpcao();
                    Opcao.DataCadastro = DateTime.Now;
                    Opcao.EnqueteId = model.Id;
                    Opcao.Opcao = r;
                    Opcao.Ordem = Ordem;
                    db.Entry(Opcao).State = EntityState.Added;
                    Ordem++;
                }
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, model.Id.ToString());
                return RedirectToAction("Details", "Enquetes", new { id = model.Id, retorno = "Enquete cadastrada com sucesso!" });
            }

            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Enquete Enquete = db.Enquete.Include(x => x.EnqueteOpcao).Where(x => x.Id == id).FirstOrDefault();

            if (Enquete == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            return View(Enquete);
        }

        [HttpPost]
        public ActionResult Edit(Enquete model, List<string> Opcoes)
        {
            if (ModelState.IsValid)
            {
                model.DataAtualizacao = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                int Ordem = 0;
                foreach (var r in Opcoes)
                {
                    EnqueteOpcao OpcaoAntiga = db.EnqueteOpcao.Where(x => x.EnqueteId == model.Id && x.Ordem == Ordem).FirstOrDefault();
                    OpcaoAntiga.Opcao = r;
                    db.Entry(OpcaoAntiga).State = EntityState.Modified;
                    Ordem++;
                }

                try
                { 
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "Enquetes", new { id = model.Id, retorno = "Enquete alterada com sucesso!" });
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
            var Enquete = db.Enquete.Find(id);
            Enquete.Status = (int)StatusPadrao.Excluido;
            db.Entry(Enquete).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Enquete.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Enquete excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Enquete = db.Enquete.Find(id);
            Enquete.Status = (int)StatusPadrao.Inativo;
            db.Entry(Enquete).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Enquete.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Enquete bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Enquete = db.Enquete.Find(id);
            Enquete.Status = (int)StatusPadrao.Ativo;
            db.Entry(Enquete).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Enquete.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Enquete desbloqueado com sucesso!" });
        }
    }
}