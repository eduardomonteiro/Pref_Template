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
    public class ProjetosController : Controller
    {
        private int currentCodArea = 9;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Projeto> ProjetoList = db.Projeto.Include(x => x.ProjetoCategoria).Include(x => x.Secretaria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = ProjetoList.OrderBy(x => x.ProjetoCategoria.Descricao).Select(x => x.ProjetoCategoria.Descricao).ToList().Distinct();
            ViewBag.Secretarias = ProjetoList.OrderBy(x => x.Secretaria.Nome).Select(x => x.Secretaria.Nome).ToList().Distinct();
            return View(ProjetoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Projeto Projeto = db.Projeto.Where(x => x.Id == id).Select(c => new
                                                                {
                                                                    c,
                                                                    ProjetoArquivo = c.ProjetoArquivo.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                                    ProjetoCategoria = c.ProjetoCategoria,
                                                                    Secretaria = c.Secretaria
                                                                })
                                                                .ToList().Select(p => p.c).FirstOrDefault();
            if (Projeto == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;
            return View(Projeto);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.ProjetoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.Secretarias = new SelectList(db.Secretaria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Nome");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Projeto model, HttpPostedFileBase Imagem, string Tags)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioProjeto());
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
                model.Slug = Utils.Utils.GerarSlug(model.Titulo);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Projeto.Where(x => x.Slug == model.Slug).FirstOrDefault();

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

                if (!string.IsNullOrEmpty(Tags))
                {
                    string[] tags = Tags.Split(new char[] { ',' });

                    foreach (var item in tags)
                    {
                        Tag Tag = new Tag();
                        Tag.DataCadastro = DateTime.Now;
                        Tag.Status = (int)StatusPadrao.Ativo;
                        Tag.AreaId = currentCodArea;
                        Tag.Descricao = item;
                        Tag.Slug = Utils.Utils.GerarSlug(item);
                        Tag.RegistroId = model.Id;

                        db.Entry(Tag).State = EntityState.Added;
                    }
                }
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, model.Id.ToString());
                return RedirectToAction("Details", "Projetos", new { id = model.Id, retorno = "Projeto cadastrado com sucesso!" });
            }

            ViewBag.Tag = Tags;
            ViewBag.Secretarias = new SelectList(db.Secretaria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Nome", model.SecretariaId);
            ViewBag.Categorias = new SelectList(db.ProjetoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.ProjetoCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Projeto Projeto = db.Projeto.Where(x => x.Id == id).FirstOrDefault();

            if (Projeto == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Secretarias = new SelectList(db.Secretaria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Nome", Projeto.SecretariaId);
            ViewBag.Tag = String.Join(",", Projeto.Tag.Select(x => x.Descricao).ToArray());
            ViewBag.Categorias = new SelectList(db.ProjetoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Projeto.ProjetoCategoriaId);

            return View(Projeto);
        }

        [HttpPost]
        public ActionResult Edit(Projeto model, HttpPostedFileBase Imagem, string Tags)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioProjeto());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        ModelState.AddModelError("Imagem", model.Imagem);
                        return View(model);
                    }
                }

                model.Slug = Utils.Utils.GerarSlug(model.Titulo);
                model.DataAtualizacao = DateTime.Now;

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Projeto.Where(x => x.Slug == model.Slug && x.Id != model.Id).FirstOrDefault();

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

                List<Tag> TagsExistentes = db.Tag.Where(x => x.RegistroId == model.Id && x.AreaId == currentCodArea).ToList();

                if (TagsExistentes != null && TagsExistentes.Count > 0)
                {
                    foreach (Tag DeleteTag in TagsExistentes)
                    {
                        db.Entry(DeleteTag).State = EntityState.Deleted;
                    }
                }

                if (!string.IsNullOrEmpty(Tags))
                {
                    string[] tags = Tags.Split(new char[] { ',' });

                    foreach (var item in tags)
                    {
                        Tag Tag = new Tag();
                        Tag.DataCadastro = DateTime.Now;
                        Tag.Status = (int)StatusPadrao.Ativo;
                        Tag.AreaId = currentCodArea;
                        Tag.Descricao = item;
                        Tag.Slug = Utils.Utils.GerarSlug(item);
                        Tag.RegistroId = model.Id;

                        db.Entry(Tag).State = EntityState.Added;

                    }
                }

                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "Projetos", new { id = model.Id, retorno = "Projeto alterado com sucesso!" });
            }

            ViewBag.Tag = Tags;
            ViewBag.Secretarias = new SelectList(db.Secretaria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Nome", model.SecretariaId);
            ViewBag.Categorias = new SelectList(db.ProjetoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.ProjetoCategoriaId);
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
            var Projeto = db.Projeto.Find(id);
            Projeto.Status = (int)StatusPadrao.Excluido;
            db.Entry(Projeto).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Projeto.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Projeto excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Projeto = db.Projeto.Find(id);
            Projeto.Status = (int)StatusPadrao.Inativo;
            db.Entry(Projeto).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Projeto.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Projeto bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Projeto = db.Projeto.Find(id);
            Projeto.Status = (int)StatusPadrao.Ativo;
            db.Entry(Projeto).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Projeto.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Projeto desbloqueado com sucesso!" });
        }

        [HttpPost]
        public ActionResult UploadFiles(string ArquivoNome, int ProjetoId, HttpPostedFileBase Arquivo, int Id = 0)
        {
            if (Id == 0)
            {
                if (Arquivo != null)
                {
                    ProjetoArquivo model = new ProjetoArquivo();

                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioProjeto());
                    string[] extensions = { ".pdf" };

                    model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Arquivo.Contains("Erro:"))
                    {
                        return RedirectToAction("Details", "Projetos", new { id = ProjetoId, retorno = model.Arquivo });
                    }
                    else
                    {
                        model.ArquivoNome = ArquivoNome;
                        model.ProjetoId = ProjetoId;
                        model.DataCadastro = DateTime.Now;
                        model.Status = 1;
                        model.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);

                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "Arquivo " + model.Id.ToString());
                        return RedirectToAction("Details", "Projetos", new { id = ProjetoId, retorno = "Arquivo salvo com sucesso!" });
                    }
                }
            }
            else
            {
                ProjetoArquivo model = db.ProjetoArquivo.Where(x => x.Id == Id).FirstOrDefault();

                if (model != null)
                {
                    if (Arquivo != null)
                    {
                        string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioProjeto());
                        string[] extensions = { ".pdf" };

                        model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                        if (model.Arquivo.Contains("Erro:"))
                        {
                            return RedirectToAction("Details", "Projetos", new { id = ProjetoId, retorno = model.Arquivo });
                        }
                        model.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);
                    }

                    model.ArquivoNome = ArquivoNome;
                    model.DataAtualizacao = DateTime.Now;


                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "Arquivo " + model.Id.ToString());
                    return RedirectToAction("Details", "Projetos", new { id = ProjetoId, retorno = "Arquivo alterado com sucesso!" });
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
            var ProjetoArquivo = db.ProjetoArquivo.Find(id);
            return Json(ProjetoArquivo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArquivo(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ProjetoArquivo = db.ProjetoArquivo.Find(id);
            ProjetoArquivo.Status = (int)StatusPadrao.Excluido;
            db.Entry(ProjetoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "Arquivo " + ProjetoArquivo.Id.ToString());
            return RedirectToAction("Details", "Projetos", new { id = ProjetoArquivo.ProjetoId, retorno = "Arquivo excluído com sucesso!" });
        }

        public ActionResult BlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ProjetoArquivo = db.ProjetoArquivo.Find(id);
            ProjetoArquivo.Status = (int)StatusPadrao.Inativo;
            db.Entry(ProjetoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "Arquivo " + ProjetoArquivo.Id.ToString());
            return RedirectToAction("Details", "Projetos", new { id = ProjetoArquivo.ProjetoId, retorno = "Arquivo bloqueado com sucesso!" });
        }

        public ActionResult UnBlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ProjetoArquivo = db.ProjetoArquivo.Find(id);
            ProjetoArquivo.Status = (int)StatusPadrao.Ativo;
            db.Entry(ProjetoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "Arquivo " + ProjetoArquivo.Id.ToString());
            return RedirectToAction("Details", "Projetos", new { id = ProjetoArquivo.ProjetoId, retorno = "Arquivo desbloqueado com sucesso!" });
        }

        public JsonResult DeleteImagem(int Id)
        {
            try
            {
                Projeto RegistroExistente = db.Projeto.Where(x => x.Id == Id).FirstOrDefault();
                RegistroExistente.Imagem = "";
                RegistroExistente.DataAtualizacao = DateTime.Now;
                db.Entry(RegistroExistente).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new
                {
                    Sucesso = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Sucesso = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}