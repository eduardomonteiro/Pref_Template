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
    public class GaleriaFotosController : Controller
    {
        private int currentCodArea = 13;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<GaleriaFoto> GaleriaFotoList = db.GaleriaFoto.Include(x => x.GaleriaFotoCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = GaleriaFotoList.OrderBy(x => x.GaleriaFotoCategoria.Descricao).Select(x => x.GaleriaFotoCategoria.Descricao).ToList().Distinct();
            return View(GaleriaFotoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            GaleriaFoto GaleriaFoto = db.GaleriaFoto.Where(x => x.Id == id)
                                                    .Select(c => new
                                                    {
                                                        c,
                                                        GaleriaFotoGaleria = c.GaleriaFotoGaleria.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                        GaleriaFotoCategoria = c.GaleriaFotoCategoria
                                                    })
                                                    .ToList().Select(p => p.c).FirstOrDefault();
            if (GaleriaFoto == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

            return View(GaleriaFoto);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.GaleriaFotoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult Create(GaleriaFoto model)
        {
            if (ModelState.IsValid)
            {
                model.DataCadastro = DateTime.Now;
                model.DataPublicacao = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                model.Slug = Utils.Utils.GerarSlug(model.Titulo);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.GaleriaFoto.Where(x => x.Slug == model.Slug).FirstOrDefault();

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
                return RedirectToAction("Details", "GaleriaFotos", new { id = model.Id, retorno = "Galeria de Fotos cadastrado com sucesso!" });
            }

            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            GaleriaFoto GaleriaFoto = db.GaleriaFoto.Where(x => x.Id == id).FirstOrDefault();

            if (GaleriaFoto == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            ViewBag.Categorias = new SelectList(db.GaleriaFotoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", GaleriaFoto.GaleriaFotoCategoriaId);

            return View(GaleriaFoto);
        }

        [HttpPost]
        public ActionResult Edit(GaleriaFoto model)
        {
            if (ModelState.IsValid)
            {
                model.DataAtualizacao = DateTime.Now;
                model.Slug = Utils.Utils.GerarSlug(model.Titulo);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.GaleriaFoto.Where(x => x.Slug == model.Slug && x.Id != model.Id).FirstOrDefault();

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
                return RedirectToAction("Details", "GaleriaFotos", new { id = model.Id, retorno = "Galeria de Fotos alterado com sucesso!" });
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
            var GaleriaFoto = db.GaleriaFoto.Find(id);
            GaleriaFoto.Status = (int)StatusPadrao.Excluido;
            db.Entry(GaleriaFoto).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, GaleriaFoto.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Galeria de Fotos excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var GaleriaFoto = db.GaleriaFoto.Find(id);
            GaleriaFoto.Status = (int)StatusPadrao.Inativo;
            db.Entry(GaleriaFoto).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, GaleriaFoto.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Galeria de Fotos bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var GaleriaFoto = db.GaleriaFoto.Find(id);
            GaleriaFoto.Status = (int)StatusPadrao.Ativo;
            db.Entry(GaleriaFoto).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, GaleriaFoto.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Galeria de Fotos desbloqueado com sucesso!" });
        }

        [HttpPost]
        public ActionResult UploadFiles(string Legenda, string Credito, int GaleriaFotoId, HttpPostedFileBase Arquivo, int Id = 0, string Tags = "")
        {
            if (Id == 0)
            {
                if (Arquivo != null)
                {
                    GaleriaFotoGaleria model = new GaleriaFotoGaleria();

                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioGaleriaImagem());
                    string[] extensions = { ".jpg", ".jpeg", ".gif", ".png" };

                    model.Imagem = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        return RedirectToAction("Details", "GaleriaFotos", new { id = GaleriaFotoId, retorno = model.Imagem });
                    }
                    else
                    {
                        model.Legenda = Legenda;
                        model.Credito = Credito;
                        model.GaleriaFotoId = GaleriaFotoId;
                        model.DataCadastro = DateTime.Now;
                        model.Status = 1;

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

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "Imagem " + model.Id.ToString());
                        return RedirectToAction("Details", "GaleriaFotos", new { id = GaleriaFotoId, retorno = "Imagem salva com sucesso!" });
                    }
                }
            }
            else
            {
                GaleriaFotoGaleria model = db.GaleriaFotoGaleria.Where(x => x.Id == Id).FirstOrDefault();

                if (model != null)
                {
                    if (Arquivo != null)
                    {
                        string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioGaleriaImagem());
                        string[] extensions = { ".jpg", ".jpeg", ".gif", ".png" };

                        model.Imagem = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                        if (model.Imagem.Contains("Erro:"))
                        {
                            return RedirectToAction("Details", "GaleriaFotos", new { id = GaleriaFotoId, retorno = model.Imagem });
                        }
                    }

                    model.Credito = Credito;
                    model.Legenda = Legenda;
                    model.DataAtualizacao = DateTime.Now;

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

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "Imagem " + model.Id.ToString());
                    return RedirectToAction("Details", "GaleriaFotos", new { id = GaleriaFotoId, retorno = "Imagem alterada com sucesso!" });
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
            var GaleriaFotoGaleria = db.GaleriaFotoGaleria.Find(id);
            return Json(GaleriaFotoGaleria, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArquivo(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var GaleriaFotoGaleria = db.GaleriaFotoGaleria.Find(id);
            GaleriaFotoGaleria.Status = (int)StatusPadrao.Excluido;
            db.Entry(GaleriaFotoGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "Imagem " + GaleriaFotoGaleria.Id.ToString());
            return RedirectToAction("Details", "GaleriaFotos", new { id = GaleriaFotoGaleria.GaleriaFotoId, retorno = "Imagem excluída com sucesso!" });
        }

        public ActionResult BlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var GaleriaFotoGaleria = db.GaleriaFotoGaleria.Find(id);
            GaleriaFotoGaleria.Status = (int)StatusPadrao.Inativo;
            db.Entry(GaleriaFotoGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "Imagem " + GaleriaFotoGaleria.Id.ToString());
            return RedirectToAction("Details", "GaleriaFotos", new { id = GaleriaFotoGaleria.GaleriaFotoId, retorno = "Imagem bloqueada com sucesso!" });
        }

        public ActionResult UnBlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var GaleriaFotoGaleria = db.GaleriaFotoGaleria.Find(id);
            GaleriaFotoGaleria.Status = (int)StatusPadrao.Ativo;
            db.Entry(GaleriaFotoGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "Imagem " + GaleriaFotoGaleria.Id.ToString());
            return RedirectToAction("Details", "GaleriaFotos", new { id = GaleriaFotoGaleria.GaleriaFotoId, retorno = "Imagem desbloqueada com sucesso!" });
        }
    }
}