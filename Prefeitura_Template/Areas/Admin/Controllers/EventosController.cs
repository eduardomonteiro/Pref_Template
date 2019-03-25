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
    public class EventosController : Controller
    {
        private int currentCodArea = 7;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Evento> EventoList = db.Evento.Include(x => x.EventoCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = EventoList.OrderBy(x => x.EventoCategoria.Descricao).Select(x => x.EventoCategoria.Descricao).ToList().Distinct();
            return View(EventoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Evento Evento = db.Evento.Where(x => x.Id == id).Select(c => new
                                                            {
                                                                c,
                                                                EventoGaleria = c.EventoGaleria.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                                EventoCategoria = c.EventoCategoria
                                                            })
                                                            .ToList().Select(p => p.c).FirstOrDefault();
            if (Evento == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            


            return View(Evento);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.EventoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Evento model, string Tags, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioEvento());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        ViewBag.Tag = Tags;
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Categorias = new SelectList(db.EventoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.EventoCategoriaId);
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
                    var SlugExistente = db.Evento.Where(x => x.Slug == model.Slug).FirstOrDefault();

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
                return RedirectToAction("Details", "Eventos", new { id = model.Id, retorno = "Evento cadastrado com sucesso!" });
            }
            ViewBag.Tag = Tags;
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Categorias = new SelectList(db.EventoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.EventoCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Evento Evento = db.Evento.Where(x => x.Id == id).FirstOrDefault();
            if (Evento == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            ViewBag.Categorias = new SelectList(db.EventoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Evento.EventoCategoriaId);
            ViewBag.Tag = String.Join(",", Evento.Tag.Select(x => x.Descricao).ToArray());
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

            return View(Evento);
        }

        [HttpPost]
        public ActionResult Edit(Evento model, string Tags, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioEvento());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Tag = Tags;
                        ViewBag.Categorias = new SelectList(db.EventoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.EventoCategoriaId);

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
                    var SlugExistente = db.Evento.Where(x => x.Slug == model.Slug && x.Id != model.Id).FirstOrDefault();

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
                return RedirectToAction("Details", "Eventos", new { id = model.Id, retorno = "Evento alterado com sucesso!" });
            }

            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Tag = Tags;
            ViewBag.Categorias = new SelectList(db.EventoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.EventoCategoriaId);
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
            var Evento = db.Evento.Find(id);
            Evento.Status = (int)StatusPadrao.Excluido;
            db.Entry(Evento).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Evento.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Evento excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Evento = db.Evento.Find(id);
            Evento.Status = (int)StatusPadrao.Inativo;
            db.Entry(Evento).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Evento.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Evento bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Evento = db.Evento.Find(id);
            Evento.Status = (int)StatusPadrao.Ativo;
            db.Entry(Evento).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Evento.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Evento desbloqueado com sucesso!" });
        }

        [HttpPost]
        public ActionResult UploadFiles(string Legenda, string Credito, int EventoId, HttpPostedFileBase Arquivo, int Id = 0)
        {
            if (Id == 0)
            {
                if (Arquivo != null)
                {
                    EventoGaleria model = new EventoGaleria();

                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioEvento());
                    string[] extensions = { ".jpg", ".jpeg", ".gif", ".png"};

                    model.Midia = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Midia.Contains("Erro:"))
                    {
                        return RedirectToAction("Details", "Eventos", new { id = EventoId, retorno = model.Midia });
                    }
                    else
                    {
                        model.Legenda = Legenda;
                        model.Credito = Credito;
                        model.EventoId = EventoId;
                        model.DataCadastro = DateTime.Now;
                        model.Status = 1;

                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "Mídia " + model.Id.ToString());
                        return RedirectToAction("Details", "Eventos", new { id = EventoId, retorno = "Mídia salva com sucesso!" });
                    }
                }
            }
            else
            {
                EventoGaleria model = db.EventoGaleria.Where(x => x.Id == Id).FirstOrDefault();

                if (model != null)
                {
                    if (Arquivo != null)
                    {
                        string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioEvento());
                        string[] extensions = { ".jpg", ".jpeg", ".gif", ".png" };

                        model.Midia = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                        if (model.Midia.Contains("Erro:"))
                        {
                            return RedirectToAction("Details", "Eventos", new { id = EventoId, retorno = model.Midia });
                        }
                    }

                    model.Credito = Credito;
                    model.Legenda = Legenda;
                    model.DataAtualizacao = DateTime.Now;

                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "Mídia " + model.Id.ToString());
                    return RedirectToAction("Details", "Eventos", new { id = EventoId, retorno = "Mídia alterado com sucesso!" });
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
            var EventoGaleria = db.EventoGaleria.Find(id);
            return Json(EventoGaleria, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArquivo(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var EventoGaleria = db.EventoGaleria.Find(id);
            EventoGaleria.Status = (int)StatusPadrao.Excluido;
            db.Entry(EventoGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "Mídia " + EventoGaleria.Id.ToString());
            return RedirectToAction("Details", "Eventos", new { id = EventoGaleria.EventoId, retorno = "Mídia excluída com sucesso!" });
        }

        public ActionResult BlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var EventoGaleria = db.EventoGaleria.Find(id);
            EventoGaleria.Status = (int)StatusPadrao.Inativo;
            db.Entry(EventoGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "Mídia " + EventoGaleria.Id.ToString());
            return RedirectToAction("Details", "Eventos", new { id = EventoGaleria.EventoId, retorno = "Mídia bloqueada com sucesso!" });
        }

        public ActionResult UnBlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var EventoGaleria = db.EventoGaleria.Find(id);
            EventoGaleria.Status = (int)StatusPadrao.Ativo;
            db.Entry(EventoGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "Mídia " + EventoGaleria.Id.ToString());
            return RedirectToAction("Details", "Eventos", new { id = EventoGaleria.EventoId, retorno = "Mídia desbloqueada com sucesso!" });
        }
    }
}