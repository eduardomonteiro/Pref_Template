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
    public class InformativosController : Controller
    {
        private int currentCodArea = 12;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Informativo> InformativoList = db.Informativo.Include(x => x.InformativoCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = InformativoList.OrderBy(x => x.InformativoCategoria.Descricao).Select(x => x.InformativoCategoria.Descricao).ToList().Distinct();
            return View(InformativoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Informativo Informativo = db.Informativo.Where(x => x.Id == id).Select(c => new
                                                                            {
                                                                                c,
                                                                                InformativoGaleria = c.InformativoGaleria.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                                                InformativoCategoria = c.InformativoCategoria
                                                                            })
                                                                           .ToList().Select(p => p.c).FirstOrDefault();
            if (Informativo == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;
            return View(Informativo);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.InformativoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Informativo model, string Tags, HttpPostedFileBase Arquivo, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Arquivo != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioInformativo());
                    string[] extensions = { ".pdf", ".doc", ".docx", ".odt", ".rtf", ".xls", ".xlsx" };

                    model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Arquivo.Contains("Erro:"))
                    {
                        ModelState.AddModelError("Arquivo", model.Arquivo);

                        ViewBag.Categorias = new SelectList(db.DocumentoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.InformativoCategoriaId);
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

                        return View(model);
                    }
                }

                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioInformativo());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        ViewBag.Categorias = new SelectList(db.DocumentoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.InformativoCategoriaId);
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

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
                    var SlugExistente = db.Informativo.Where(x => x.Slug == model.Slug).FirstOrDefault();

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
                return RedirectToAction("Details", "Informativos", new { id = model.Id, retorno = "Informativo cadastrado com sucesso!" });
            }
            ViewBag.Tag = Tags;
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Categorias = new SelectList(db.InformativoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.InformativoCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Informativo Informativo = db.Informativo.Where(x => x.Id == id).FirstOrDefault();
            if (Informativo == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Categorias = new SelectList(db.InformativoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Informativo.InformativoCategoriaId);
            ViewBag.Tag = String.Join(",", Informativo.Tag.Select(x => x.Descricao).ToArray());
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

            return View(Informativo);
        }

        [HttpPost]
        public ActionResult Edit(Informativo model, string Tags, HttpPostedFileBase Arquivo, HttpPostedFileBase Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Arquivo != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioInformativo());
                    string[] extensions = { ".pdf", ".doc", ".docx", ".odt", ".rtf", ".xls", ".xlsx" };

                    model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Arquivo.Contains("Erro:"))
                    {
                        ModelState.AddModelError("Arquivo", model.Arquivo);

                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Tag = Tags;
                        ViewBag.Categorias = new SelectList(db.InformativoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.InformativoCategoriaId);

                        return View(model);
                    }
                }

                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioInformativo());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Tag = Tags;
                        ViewBag.Categorias = new SelectList(db.InformativoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.InformativoCategoriaId);

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
                    var SlugExistente = db.Informativo.Where(x => x.Slug == model.Slug && x.Id != model.Id).FirstOrDefault();

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
                return RedirectToAction("Details", "Informativos", new { id = model.Id, retorno = "Informativo alterado com sucesso!" });
            }

            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Tag = Tags;
            ViewBag.Categorias = new SelectList(db.InformativoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.InformativoCategoriaId);
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
            var Informativo = db.Informativo.Find(id);
            Informativo.Status = (int)StatusPadrao.Excluido;
            db.Entry(Informativo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Informativo.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Informativo excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Informativo = db.Informativo.Find(id);
            Informativo.Status = (int)StatusPadrao.Inativo;
            db.Entry(Informativo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Informativo.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Informativo bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Informativo = db.Informativo.Find(id);
            Informativo.Status = (int)StatusPadrao.Ativo;
            db.Entry(Informativo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Informativo.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Informativo desbloqueado com sucesso!" });
        }

        [HttpPost]
        public ActionResult UploadFiles(string Legenda, string Credito, int InformativoId, HttpPostedFileBase Arquivo, int Id = 0)
        {
            if (Id == 0)
            {
                if (Arquivo != null)
                {
                    InformativoGaleria model = new InformativoGaleria();

                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioInformativo());
                    string[] extensions = { ".jpg", ".jpeg", ".gif", ".png" };

                    model.Midia = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Midia.Contains("Erro:"))
                    {
                        return RedirectToAction("Details", "Informativos", new { id = InformativoId, retorno = model.Midia });
                    }
                    else
                    {
                        model.Legenda = Legenda;
                        model.Credito = Credito;
                        model.InformativoId = InformativoId;
                        model.DataCadastro = DateTime.Now;
                        model.Status = 1;

                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "Mídia " + model.Id.ToString());
                        return RedirectToAction("Details", "Informativos", new { id = InformativoId, retorno = "Mídia salva com sucesso!" });
                    }
                }
            }
            else
            {
                InformativoGaleria model = db.InformativoGaleria.Where(x => x.Id == Id).FirstOrDefault();

                if (model != null)
                {
                    if (Arquivo != null)
                    {
                        string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioInformativo());
                        string[] extensions = { ".jpg", ".jpeg", ".gif", ".png" };

                        model.Midia = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                        if (model.Midia.Contains("Erro:"))
                        {
                            return RedirectToAction("Details", "Informativos", new { id = InformativoId, retorno = model.Midia });
                        }
                    }

                    model.Credito = Credito;
                    model.Legenda = Legenda;
                    model.DataAtualizacao = DateTime.Now;

                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "Mídia " + model.Id.ToString());
                    return RedirectToAction("Details", "Informativos", new { id = InformativoId, retorno = "Mídia alterado com sucesso!" });
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
            var InformativoGaleria = db.InformativoGaleria.Find(id);
            return Json(InformativoGaleria, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArquivo(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var InformativoGaleria = db.InformativoGaleria.Find(id);
            InformativoGaleria.Status = (int)StatusPadrao.Excluido;
            db.Entry(InformativoGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "Mídia " + InformativoGaleria.Id.ToString());
            return RedirectToAction("Details", "Informativos", new { id = InformativoGaleria.InformativoId, retorno = "Mídia excluída com sucesso!" });
        }

        public ActionResult BlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var InformativoGaleria = db.InformativoGaleria.Find(id);
            InformativoGaleria.Status = (int)StatusPadrao.Inativo;
            db.Entry(InformativoGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "Mídia " + InformativoGaleria.Id.ToString());
            return RedirectToAction("Details", "Informativos", new { id = InformativoGaleria.InformativoId, retorno = "Mídia bloqueada com sucesso!" });
        }

        public ActionResult UnBlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var InformativoGaleria = db.InformativoGaleria.Find(id);
            InformativoGaleria.Status = (int)StatusPadrao.Ativo;
            db.Entry(InformativoGaleria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "Mídia " + InformativoGaleria.Id.ToString());
            return RedirectToAction("Details", "Informativos", new { id = InformativoGaleria.InformativoId, retorno = "Mídia desbloqueada com sucesso!" });
        }

        public JsonResult DeleteArquivoEdit(int Id)
        {
            try
            {
                Informativo RegistroExistente = db.Informativo.Where(x => x.Id == Id).FirstOrDefault();
                RegistroExistente.Arquivo = "";
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