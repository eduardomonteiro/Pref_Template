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
    public class NoticiasController : Controller
    {
        private int currentCodArea = 8;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Noticia> NoticiaList = db.Noticia.Include(x => x.NoticiaCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = NoticiaList.OrderBy(x => x.NoticiaCategoria.Descricao).Select(x => x.NoticiaCategoria.Descricao).ToList().Distinct();
            return View(NoticiaList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Noticia Noticia = db.Noticia.Where(x => x.Id == id).Select(c => new
                                                                {
                                                                    c,
                                                                    NoticiaGaleria = c.NoticiaGaleria.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                                    NoticiaCategoria = c.NoticiaCategoria
                                                                })
                                                                .ToList().Select(p => p.c).FirstOrDefault();
            if (Noticia == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(Noticia);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.NoticiaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Noticia model, string Tags, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioNoticia());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        ViewBag.Tag = Tags;
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Categorias = new SelectList(db.NoticiaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.NoticiaCategoriaId);

                        ModelState.AddModelError("Imagem", model.Imagem);
                        return View(model);
                    }
                }

                model.DataPublicacao = DateTime.Now;
                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                model.Slug = Utils.Utils.GerarSlug(model.Titulo);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Noticia.Where(x => x.Slug == model.Slug).FirstOrDefault();

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
                return RedirectToAction("Details", "Noticias", new { id = model.Id, retorno = "Noticia cadastrado com sucesso!" });
            }
            ViewBag.Tag = Tags;
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Categorias = new SelectList(db.NoticiaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.NoticiaCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Noticia Noticia = db.Noticia.Where(x => x.Id == id).FirstOrDefault();
            if (Noticia == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
           
            ViewBag.Categorias = new SelectList(db.NoticiaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Noticia.NoticiaCategoriaId);
            ViewBag.Tag = String.Join(",", Noticia.Tag.Select(x => x.Descricao).ToArray());
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

            return View(Noticia);
        }

        [HttpPost]
        public ActionResult Edit(Noticia model, string Tags, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioNoticia());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Tag = Tags;
                        ViewBag.Categorias = new SelectList(db.NoticiaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.NoticiaCategoriaId);

                        ModelState.AddModelError("Imagem", model.Imagem);
                        return View(model);
                    }
                }

                model.DataAtualizacao = DateTime.Now;
                model.Slug = Utils.Utils.GerarSlug(model.Titulo);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Noticia.Where(x => x.Slug == model.Slug && x.Id != model.Id).FirstOrDefault();

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
                return RedirectToAction("Details", "Noticias", new { id = model.Id, retorno = "Noticia alterado com sucesso!" });
            }

            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Tag = Tags;
            ViewBag.Categorias = new SelectList(db.NoticiaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.NoticiaCategoriaId);
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
            var Noticia = db.Noticia.Find(id);
            Noticia.Status = (int)StatusPadrao.Excluido;
            db.Entry(Noticia).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Noticia.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Noticia excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Noticia = db.Noticia.Find(id);
            Noticia.Status = (int)StatusPadrao.Inativo;
            db.Entry(Noticia).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Noticia.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Noticia bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Noticia = db.Noticia.Find(id);
            Noticia.Status = (int)StatusPadrao.Ativo;
            db.Entry(Noticia).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Noticia.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Noticia desbloqueado com sucesso!" });
        }

        [HttpPost]
        public ActionResult UploadFiles(string Legenda, string Credito, int NoticiaId, HttpPostedFileBase Arquivo, int Id = 0)
        {
            if (Id == 0)
            {
                if (Arquivo != null)
                {
                    NoticiaGaleria model = new NoticiaGaleria();

                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioNoticia());
                    string[] extensions = { ".jpg", ".jpeg", ".gif", ".png" };

                    model.Midia = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Midia.Contains("Erro:"))
                    {
                        return RedirectToAction("Details", "Noticias", new { id = NoticiaId, retorno = model.Midia });
                    }
                    else
                    {
                        model.Legenda = Legenda;
                        model.Credito = Credito;
                        model.NoticiaId = NoticiaId;
                        model.DataCadastro = DateTime.Now;
                        model.Status = 1;

                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "Mídia " + model.Id.ToString());
                        return RedirectToAction("Details", "Noticias", new { id = NoticiaId, retorno = "Mídia salva com sucesso!" });
                    }
                }
            }
            else
            {
                NoticiaGaleria model = db.NoticiaGaleria.Where(x => x.Id == Id).FirstOrDefault();

                if (model != null)
                {
                    if (Arquivo != null)
                    {
                        string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioNoticia());
                        string[] extensions = { ".jpg", ".jpeg", ".gif", ".png" };

                        model.Midia = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                        if (model.Midia.Contains("Erro:"))
                        {
                            return RedirectToAction("Details", "Noticias", new { id = NoticiaId, retorno = model.Midia });
                        }
                    }

                    model.Credito = Credito;
                    model.Legenda = Legenda;
                    model.DataAtualizacao = DateTime.Now;

                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "Mídia " + model.Id.ToString());
                    return RedirectToAction("Details", "Noticias", new { id = NoticiaId, retorno = "Mídia alterado com sucesso!" });
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
            var NoticiaGaleria = db.NoticiaGaleria.Find(id);
            return Json(NoticiaGaleria, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArquivo(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var NoticiaGaleria = db.NoticiaGaleria.Find(id);
            NoticiaGaleria.Status = (int)StatusPadrao.Excluido;
            db.Entry(NoticiaGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "Mídia " + NoticiaGaleria.Id.ToString());
            return RedirectToAction("Details", "Noticias", new { id = NoticiaGaleria.NoticiaId, retorno = "Mídia excluída com sucesso!" });
        }

        public ActionResult BlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var NoticiaGaleria = db.NoticiaGaleria.Find(id);
            NoticiaGaleria.Status = (int)StatusPadrao.Inativo;
            db.Entry(NoticiaGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "Mídia " + NoticiaGaleria.Id.ToString());
            return RedirectToAction("Details", "Noticias", new { id = NoticiaGaleria.NoticiaId, retorno = "Mídia bloqueada com sucesso!" });
        }

        public ActionResult UnBlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var NoticiaGaleria = db.NoticiaGaleria.Find(id);
            NoticiaGaleria.Status = (int)StatusPadrao.Ativo;
            db.Entry(NoticiaGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "Mídia " + NoticiaGaleria.Id.ToString());
            return RedirectToAction("Details", "Noticias", new { id = NoticiaGaleria.NoticiaId, retorno = "Mídia desbloqueada com sucesso!" });
        }
    }
}