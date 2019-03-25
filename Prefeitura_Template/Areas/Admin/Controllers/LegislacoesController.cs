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
    public class LegislacoesController : Controller
    {
        private int currentCodArea = 22;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Legislacao> LegislacaoList = db.Legislacao.Include(x => x.LegislacaoCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = LegislacaoList.OrderBy(x => x.LegislacaoCategoria.Descricao).Select(x => x.LegislacaoCategoria.Descricao).ToList().Distinct();
            return View(LegislacaoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Legislacao Legislacao = db.Legislacao.Where(x => x.Id == id).Select(c => new
                                                                        {
                                                                            c,
                                                                            LegislacaoArquivo = c.LegislacaoArquivo.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                                            LegislacaoCategoria = c.LegislacaoCategoria
                                                                        })
                                                                        .ToList().Select(p => p.c).FirstOrDefault();
            if (Legislacao == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(Legislacao);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.LegislacaoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Legislacao model)
        {
            if (ModelState.IsValid)
            {
                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                model.DataPublicacao = DateTime.Now;
                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, model.Id.ToString());
                return RedirectToAction("Details", "Legislacoes", new { id = model.Id, retorno = "Legislação cadastrado com sucesso!" });
            }

            ViewBag.Categorias = new SelectList(db.LegislacaoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.LegislacaoCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Legislacao Legislacao = db.Legislacao.Where(x => x.Id == id).FirstOrDefault();
            if (Legislacao == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            ViewBag.Categorias = new SelectList(db.LegislacaoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Legislacao.LegislacaoCategoriaId);

            return View(Legislacao);
        }

        [HttpPost]
        public ActionResult Edit(Legislacao model)
        {
            if (ModelState.IsValid)
            {
                model.DataAtualizacao = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "Legislacoes", new { id = model.Id, retorno = "Legislação alterado com sucesso!" });
            }

            ViewBag.Categorias = new SelectList(db.LegislacaoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.LegislacaoCategoriaId);
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
            var Legislacao = db.Legislacao.Find(id);
            Legislacao.Status = (int)StatusPadrao.Excluido;
            db.Entry(Legislacao).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Legislacao.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Legislacao excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Legislacao = db.Legislacao.Find(id);
            Legislacao.Status = (int)StatusPadrao.Inativo;
            db.Entry(Legislacao).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Legislacao.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Legislacao bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Legislacao = db.Legislacao.Find(id);
            Legislacao.Status = (int)StatusPadrao.Ativo;
            db.Entry(Legislacao).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Legislacao.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Legislação desbloqueado com sucesso!" });
        }

        [HttpPost]
        public ActionResult UploadFiles(string ArquivoNome, int LegislacaoId, HttpPostedFileBase Arquivo, int Id = 0)
        {
            if (Id == 0)
            {
                if (Arquivo != null)
                {
                    LegislacaoArquivo model = new LegislacaoArquivo();

                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioLegislacao());
                    string[] extensions = { ".pdf" };

                    model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Arquivo.Contains("Erro:"))
                    {
                        return RedirectToAction("Details", "Legislacoes", new { id = LegislacaoId, retorno = model.Arquivo });
                    }
                    else
                    {
                        model.ArquivoNome = ArquivoNome;
                        model.LegislacaoId = LegislacaoId;
                        model.DataCadastro = DateTime.Now;
                        model.Status = 1;
                        model.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);

                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "Arquivo " + model.Id.ToString());
                        return RedirectToAction("Details", "Legislacoes", new { id = LegislacaoId, retorno = "Arquivo salvo com sucesso!" });
                    }
                }
            }
            else
            {
                LegislacaoArquivo model = db.LegislacaoArquivo.Where(x => x.Id == Id).FirstOrDefault();

                if (model != null)
                {
                    if (Arquivo != null)
                    {
                        string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioLegislacao());
                        string[] extensions = { ".pdf" };

                        model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                        if (model.Arquivo.Contains("Erro:"))
                        {
                            return RedirectToAction("Details", "Legislacoes", new { id = LegislacaoId, retorno = model.Arquivo });
                        }
                        model.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);
                    }

                    model.ArquivoNome = ArquivoNome;
                    model.DataAtualizacao = DateTime.Now;
                   

                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "Arquivo " + model.Id.ToString());
                    return RedirectToAction("Details", "Legislacoes", new { id = LegislacaoId, retorno = "Arquivo alterado com sucesso!" });
                }
            }

            return Json("Selecione um arquivo");
        }

        public JsonResult EditArquivo(int id)
        {
            if (id == 0)
            {
                return Json("id Obrigatorio", JsonRequestBehavior.AllowGet);
            }
            var LegislacaoArquivo = db.LegislacaoArquivo.Find(id);
            return Json(LegislacaoArquivo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArquivo(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var LegislacaoArquivo = db.LegislacaoArquivo.Find(id);
            LegislacaoArquivo.Status = (int)StatusPadrao.Excluido;
            db.Entry(LegislacaoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "Arquivo " + LegislacaoArquivo.Id.ToString());
            return RedirectToAction("Details", "Legislacoes", new { id = LegislacaoArquivo.LegislacaoId, retorno = "Arquivo excluído com sucesso!" });
        }

        public ActionResult BlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var LegislacaoArquivo = db.LegislacaoArquivo.Find(id);
            LegislacaoArquivo.Status = (int)StatusPadrao.Inativo;
            db.Entry(LegislacaoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "Arquivo " + LegislacaoArquivo.Id.ToString());
            return RedirectToAction("Details", "Legislacoes", new { id = LegislacaoArquivo.LegislacaoId, retorno = "Arquivo bloqueado com sucesso!" });
        }

        public ActionResult UnBlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var LegislacaoArquivo = db.LegislacaoArquivo.Find(id);
            LegislacaoArquivo.Status = (int)StatusPadrao.Ativo;
            db.Entry(LegislacaoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "Arquivo " + LegislacaoArquivo.Id.ToString());
            return RedirectToAction("Details", "Legislacoes", new { id = LegislacaoArquivo.LegislacaoId, retorno = "Arquivo desbloqueado com sucesso!" });
        }
    }
}