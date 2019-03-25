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
    public class LicitacoesController : Controller
    {
        private int currentCodArea = 18;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Licitacao> LicitacaoList = db.Licitacao.Include(x => x.LicitacaoModalidade).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Modalidades = LicitacaoList.OrderBy(x => x.LicitacaoModalidade.Descricao).Select(x => x.LicitacaoModalidade.Descricao).ToList().Distinct();
            return View(LicitacaoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Licitacao Licitacao = db.Licitacao.Where(x => x.Id == id)
                                                .Select(c => new
                                                {
                                                    c,
                                                    LicitacaoArquivo = c.LicitacaoArquivo.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                    LicitacaoModalidade = c.LicitacaoModalidade,
                                                    StatusPublicacao = c.StatusPublicacao
                                                })
                                                .ToList().Select(p => p.c).FirstOrDefault();

            if (Licitacao == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(Licitacao);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Modalidades = new SelectList(db.LicitacaoModalidade.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.Status = new SelectList(db.StatusPublicacao.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Licitacao model)
        {
            if (ModelState.IsValid)
            {
                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                model.DataPublicacao = DateTime.Now;
                model.Slug = Utils.Utils.GerarSlug(model.Titulo);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Licitacao.Where(x => x.Slug == model.Slug).FirstOrDefault();

                    if (SlugExistente == null || SlugExistente.Id == 0)
                    {
                        SlugVazia = true;
                    }
                    else
                    {
                        if (j == 1)
                        {
                            model.Slug = model.Slug + "_";
                        }
                        else
                        {
                            model.Slug.Remove(model.Slug.Length - 1);
                        }
                        model.Slug = model.Slug + j.ToString();
                    }
                }

                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, model.Id.ToString());
                return RedirectToAction("Details", "Licitacoes", new { id = model.Id, retorno = "Licitação cadastrado com sucesso!" });
            }

            ViewBag.Modalidades = new SelectList(db.LicitacaoModalidade.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.LicitacaoModalidadeId);
            ViewBag.Status = new SelectList(db.StatusPublicacao.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.StatusPublicacaoId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Licitacao Licitacao = db.Licitacao.Where(x => x.Id == id).FirstOrDefault();
            if (Licitacao == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            ViewBag.Modalidades = new SelectList(db.LicitacaoModalidade.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Licitacao.LicitacaoModalidadeId);
            ViewBag.Status = new SelectList(db.StatusPublicacao.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Licitacao.StatusPublicacaoId);

            return View(Licitacao);
        }

        [HttpPost]
        public ActionResult Edit(Licitacao model)
        {
            if (ModelState.IsValid)
            {
                model.DataAtualizacao = DateTime.Now;
                model.Slug = Utils.Utils.GerarSlug(model.Titulo);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Licitacao.Where(x => x.Slug == model.Slug && x.Id != model.Id).FirstOrDefault();

                    if (SlugExistente == null || SlugExistente.Id == 0)
                    {
                        SlugVazia = true;
                    }
                    else
                    {
                        if (j == 1)
                        {
                            model.Slug = model.Slug + "_";
                        }
                        else
                        {
                            model.Slug.Remove(model.Slug.Length - 1);
                        }
                        model.Slug = model.Slug + j.ToString();
                    }
                }

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "Licitacoes", new { id = model.Id, retorno = "Licitação alterado com sucesso!" });
            }

            ViewBag.Modalidades = new SelectList(db.LicitacaoModalidade.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.LicitacaoModalidadeId);
            ViewBag.Status = new SelectList(db.StatusPublicacao.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.StatusPublicacaoId);
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
            var Licitacao = db.Licitacao.Find(id);
            Licitacao.Status = (int)StatusPadrao.Excluido;
            db.Entry(Licitacao).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Licitacao.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Licitação excluída com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Licitacao = db.Licitacao.Find(id);
            Licitacao.Status = (int)StatusPadrao.Inativo;
            db.Entry(Licitacao).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Licitacao.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Licitação bloqueada com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Licitacao = db.Licitacao.Find(id);
            Licitacao.Status = (int)StatusPadrao.Ativo;
            db.Entry(Licitacao).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Licitacao.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Licitação desbloqueada com sucesso!" });
        }

        [HttpPost]
        public ActionResult UploadFiles(string ArquivoNome, int LicitacaoId, HttpPostedFileBase Arquivo, int Id = 0)
        {
            if (Id == 0)
            {
                if (Arquivo != null)
                {
                    LicitacaoArquivo model = new LicitacaoArquivo();

                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioLicitacao());
                    string[] extensions = { ".pdf", ".doc", ".docx", ".odt", ".rtf", ".xls", ".xlsx" };

                    model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Arquivo.Contains("Erro:"))
                    {
                        return RedirectToAction("Details", "Licitacoes", new { id = LicitacaoId, retorno = model.Arquivo });
                    }
                    else
                    {
                        model.ArquivoNome = ArquivoNome;
                        model.LicitacaoId = LicitacaoId;
                        model.DataCadastro = DateTime.Now;
                        model.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);
                        model.Status = 1;

                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "Arquivo " + model.Id.ToString());
                        return RedirectToAction("Details", "Licitacoes", new { id = LicitacaoId, retorno = "Arquivo salvo com sucesso!" });
                    }
                }
            }
            else
            {
                LicitacaoArquivo model = db.LicitacaoArquivo.Where(x => x.Id == Id).FirstOrDefault();

                if (model != null)
                {
                    if (Arquivo != null)
                    {
                        string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioLicitacao());
                        string[] extensions = { ".pdf", ".doc", ".docx", ".odt", ".rtf", ".xls", ".xlsx" };

                        model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                        if (model.Arquivo.Contains("Erro:"))
                        {
                            return RedirectToAction("Details", "Licitacoes", new { id = LicitacaoId, retorno = model.Arquivo });
                        }
                        model.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);
                    }

                    model.ArquivoNome = ArquivoNome;
                    model.DataAtualizacao = DateTime.Now;
                    
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "Arquivo " + model.Id.ToString());
                    return RedirectToAction("Details", "Licitacoes", new { id = LicitacaoId, retorno = "Arquivo alterado com sucesso!" });
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
            var LicitacaoArquivo = db.LicitacaoArquivo.Find(id);
            return Json(LicitacaoArquivo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArquivo(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var LicitacaoArquivo = db.LicitacaoArquivo.Find(id);
            LicitacaoArquivo.Status = (int)StatusPadrao.Excluido;
            db.Entry(LicitacaoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "Arquivo " + LicitacaoArquivo.Id.ToString());
            return RedirectToAction("Details", "Licitacoes", new { id = LicitacaoArquivo.LicitacaoId, retorno = "Arquivo excluído com sucesso!" });
        }

        public ActionResult BlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var LicitacaoArquivo = db.LicitacaoArquivo.Find(id);
            LicitacaoArquivo.Status = (int)StatusPadrao.Inativo;
            db.Entry(LicitacaoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "Arquivo " + LicitacaoArquivo.Id.ToString());
            return RedirectToAction("Details", "Licitacoes", new { id = LicitacaoArquivo.LicitacaoId, retorno = "Arquivo bloqueado com sucesso!" });
        }

        public ActionResult UnBlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var LicitacaoArquivo = db.LicitacaoArquivo.Find(id);
            LicitacaoArquivo.Status = (int)StatusPadrao.Ativo;
            db.Entry(LicitacaoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "Arquivo " + LicitacaoArquivo.Id.ToString());
            return RedirectToAction("Details", "Licitacoes", new { id = LicitacaoArquivo.LicitacaoId, retorno = "Arquivo desbloqueado com sucesso!" });
        }
    }
}