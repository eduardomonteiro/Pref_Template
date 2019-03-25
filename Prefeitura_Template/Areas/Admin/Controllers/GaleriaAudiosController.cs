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
    public class GaleriaAudiosController : Controller
    {
        private int currentCodArea = 14;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<GaleriaAudio> GaleriaAudioList = db.GaleriaAudio.Include(x => x.GaleriaAudioCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = GaleriaAudioList.OrderBy(x => x.GaleriaAudioCategoria.Descricao).Select(x => x.GaleriaAudioCategoria.Descricao).ToList().Distinct();
            return View(GaleriaAudioList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            GaleriaAudio GaleriaAudio = db.GaleriaAudio.Include(x => x.GaleriaAudioCategoria).Where(x => x.Id == id).FirstOrDefault();
            if (GaleriaAudio == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(GaleriaAudio);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.GaleriaAudioCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            return View();
        }

        [HttpPost]
        public ActionResult Create(GaleriaAudio model, HttpPostedFileBase Audio, string Tags)
        {
            if (ModelState.IsValid)
            {
                if (Audio != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioAudio());
                    string[] extensions = { ".mp3", ".wma", ".aac", ".ogg", ".ac3", ".wav" };

                    model.Audio = Utils.Utils.UploadImagem(Audio, Path, extensions);

                    if (model.Audio.Contains("Erro:"))
                    {
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Tag = Tags;
                        ViewBag.Categorias = new SelectList(db.GaleriaAudioCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.GaleriaAudioCategoriaId);
                        ModelState.AddModelError("Audio", model.Audio);
                        return View(model);
                    }
                }

                model.DataCadastro = DateTime.Now;
                model.DataPublicacao = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
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
                return RedirectToAction("Details", "GaleriaAudios", new { id = model.Id, retorno = "Galeria de Áudio cadastrado com sucesso!" });
            }

            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Tag = Tags;
            ViewBag.Categorias = new SelectList(db.GaleriaAudioCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.GaleriaAudioCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            GaleriaAudio GaleriaAudio = db.GaleriaAudio.Where(x => x.Id == id).FirstOrDefault();
            if (GaleriaAudio == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Categorias = new SelectList(db.GaleriaAudioCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", GaleriaAudio.GaleriaAudioCategoriaId);
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Tag = String.Join(",", GaleriaAudio.Tag.Select(x => x.Descricao).ToArray());

            return View(GaleriaAudio);
        }

        [HttpPost]
        public ActionResult Edit(GaleriaAudio model, HttpPostedFileBase Audio, string Tags)
        {
            if (ModelState.IsValid)
            {
                if (Audio != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioAudio());
                    string[] extensions = { ".mp3", ".wma", ".aac", ".ogg", ".ac3", ".wav" };

                    model.Audio = Utils.Utils.UploadImagem(Audio, Path, extensions);

                    if (model.Audio.Contains("Erro:"))
                    {
                        ViewBag.Tag = Tags;
                        ViewBag.Categorias = new SelectList(db.GaleriaAudioCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.GaleriaAudioCategoriaId);
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ModelState.AddModelError("Audio", model.Audio);
                        return View(model);
                    }
                }

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

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "GaleriaAudios", new { id = model.Id, retorno = "Galeria de Áudio alterado com sucesso!" });
            }

            ViewBag.Tag = Tags;
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Categorias = new SelectList(db.GaleriaAudioCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.GaleriaAudioCategoriaId);
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
            var GaleriaAudio = db.GaleriaAudio.Find(id);
            GaleriaAudio.Status = (int)StatusPadrao.Excluido;
            db.Entry(GaleriaAudio).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, GaleriaAudio.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Galeria de Áudio excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var GaleriaAudio = db.GaleriaAudio.Find(id);
            GaleriaAudio.Status = (int)StatusPadrao.Inativo;
            db.Entry(GaleriaAudio).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, GaleriaAudio.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Galeria de Áudio bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var GaleriaAudio = db.GaleriaAudio.Find(id);
            GaleriaAudio.Status = (int)StatusPadrao.Ativo;
            db.Entry(GaleriaAudio).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, GaleriaAudio.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Galeria de Áudio desbloqueado com sucesso!" });
        }
    }
}