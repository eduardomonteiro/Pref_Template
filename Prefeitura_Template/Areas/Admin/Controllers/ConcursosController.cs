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
    public class ConcursosController : Controller
    {
        private int currentCodArea = 18;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Concurso> ConcursoList = db.Concurso.Include(x => x.ConcursoModalidade).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Modalidades = ConcursoList.OrderBy(x => x.ConcursoModalidade.Descricao).Select(x => x.ConcursoModalidade.Descricao).ToList().Distinct();
            return View(ConcursoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Concurso Concurso = db.Concurso.Where(x => x.Id == id)
                                                .Select(c => new
                                                {
                                                    c,
                                                    ConcursoArquivo = c.ConcursoArquivo.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                    ConcursoModalidade = c.ConcursoModalidade,
                                                    StatusPublicacao = c.StatusPublicacao
                                                })
                                                .ToList().Select(p => p.c).FirstOrDefault();
            if (Concurso == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            

            return View(Concurso);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Modalidades = new SelectList(db.ConcursoModalidade.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.Status = new SelectList(db.StatusPublicacao.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Concurso model)
        {
            if (ModelState.IsValid)
            {
                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                model.Slug = Utils.Utils.GerarSlug(model.Titulo);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Concurso.Where(x => x.Slug == model.Slug).FirstOrDefault();

                    if(SlugExistente == null || SlugExistente.Id == 0)
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
                return RedirectToAction("Details", "Concursos", new { id = model.Id, retorno = "Concurso cadastrado com sucesso!" });
            }

            ViewBag.Modalidades = new SelectList(db.ConcursoModalidade.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao",model.ConcursoModalidadeId);
            ViewBag.Status = new SelectList(db.StatusPublicacao.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao",model.StatusPublicacaoId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Concurso Concurso = db.Concurso.Where(x => x.Id == id).FirstOrDefault();
            if (Concurso == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Modalidades = new SelectList(db.ConcursoModalidade.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Concurso.ConcursoModalidadeId);
            ViewBag.Status = new SelectList(db.StatusPublicacao.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Concurso.StatusPublicacaoId);

            return View(Concurso);
        }

        [HttpPost]
        public ActionResult Edit(Concurso model)
        {
            if (ModelState.IsValid)
            {
                model.DataAtualizacao = DateTime.Now;
                model.Slug = Utils.Utils.GerarSlug(model.Titulo);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Concurso.Where(x => x.Slug == model.Slug && x.Id != model.Id).FirstOrDefault();

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
                return RedirectToAction("Details", "Concursos", new { id = model.Id, retorno = "Concurso alterado com sucesso!" });
            }

            ViewBag.Modalidades = new SelectList(db.ConcursoModalidade.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.ConcursoModalidadeId);
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
            var Concurso = db.Concurso.Find(id);
            Concurso.Status = (int)StatusPadrao.Excluido;
            db.Entry(Concurso).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Concurso.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Concurso excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Concurso = db.Concurso.Find(id);
            Concurso.Status = (int)StatusPadrao.Inativo;
            db.Entry(Concurso).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Concurso.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Concurso bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Concurso = db.Concurso.Find(id);
            Concurso.Status = (int)StatusPadrao.Ativo;
            db.Entry(Concurso).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Concurso.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Concurso desbloqueado com sucesso!" });
        }

        [HttpPost]
        public ActionResult UploadFiles(string ArquivoNome, int ConcursoId, HttpPostedFileBase Arquivo, int Id = 0)
        {
            if (Id == 0)
            {
                if (Arquivo != null)
                {
                    ConcursoArquivo model = new ConcursoArquivo();

                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioConcurso());
                    string[] extensions = { ".pdf", ".doc", ".docx", ".odt", ".rtf", ".xls", ".xlsx" };

                    model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Arquivo.Contains("Erro:"))
                    {
                        return RedirectToAction("Details", "Concursos", new { id = ConcursoId, retorno = model.Arquivo });
                    }
                    else
                    {
                        model.ArquivoNome = ArquivoNome;
                        model.ConcursoId = ConcursoId;
                        model.DataCadastro = DateTime.Now;
                        model.Status = 1;
                        model.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);

                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "Arquivo " + model.Id.ToString());
                        return RedirectToAction("Details", "Concursos", new { id = ConcursoId, retorno = "Arquivo salvo com sucesso!" });
                    }
                }
            }
            else
            {
                ConcursoArquivo model = db.ConcursoArquivo.Where(x => x.Id == Id).FirstOrDefault();

                if (model != null)
                {
                    if (Arquivo != null)
                    {
                        string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioConcurso());
                        string[] extensions = { ".pdf", ".doc", ".docx", ".odt", ".rtf", ".xls", ".xlsx" };

                        model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                        if (model.Arquivo.Contains("Erro:"))
                        {
                            return RedirectToAction("Details", "Concursos", new { id = ConcursoId, retorno = model.Arquivo });
                        }
                        model.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);
                    }

                    model.ArquivoNome = ArquivoNome;
                    model.DataAtualizacao = DateTime.Now;
                    

                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "Arquivo " + model.Id.ToString());
                    return RedirectToAction("Details", "Concursos", new { id = ConcursoId, retorno = "Arquivo alterado com sucesso!" });
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
            var ConcursoArquivo = db.ConcursoArquivo.Find(id);
            return Json(ConcursoArquivo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArquivo(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ConcursoArquivo = db.ConcursoArquivo.Find(id);
            ConcursoArquivo.Status = (int)StatusPadrao.Excluido;
            db.Entry(ConcursoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "Arquivo " + ConcursoArquivo.Id.ToString());
            return RedirectToAction("Details", "Concursos", new { id = ConcursoArquivo.ConcursoId, retorno = "Arquivo excluído com sucesso!" });
        }

        public ActionResult BlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ConcursoArquivo = db.ConcursoArquivo.Find(id);
            ConcursoArquivo.Status = (int)StatusPadrao.Inativo;
            db.Entry(ConcursoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "Arquivo " + ConcursoArquivo.Id.ToString());
            return RedirectToAction("Details", "Concursos", new { id = ConcursoArquivo.ConcursoId, retorno = "Arquivo bloqueado com sucesso!" });
        }

        public ActionResult UnBlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ConcursoArquivo = db.ConcursoArquivo.Find(id);
            ConcursoArquivo.Status = (int)StatusPadrao.Ativo;
            db.Entry(ConcursoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "Arquivo " + ConcursoArquivo.Id.ToString());
            return RedirectToAction("Details", "Concursos", new { id = ConcursoArquivo.ConcursoId, retorno = "Arquivo desbloqueado com sucesso!" });
        }
    }
}