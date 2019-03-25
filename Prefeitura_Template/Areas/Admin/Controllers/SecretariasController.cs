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
    public class SecretariasController : Controller
    {
        private int currentCodArea = 11;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;

            List<Secretaria> SecretariaList = new List<Secretaria>();

            int usuarioID = int.Parse(User.Identity.GetUserId());
            Usuario usuario = db.Usuario.Where(x => x.Id == usuarioID).Select(c => new
                                                                        {
                                                                            c,
                                                                            UsuarioSecretaria = c.UsuarioSecretaria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                                        }).ToList().Select(p => p.c).FirstOrDefault();
            if (usuario.UsuarioSecretaria != null && usuario.UsuarioSecretaria.Count > 0)
            {
                List<int> SecretarisaIds = usuario.UsuarioSecretaria.Select(x => x.SecretariaId).ToList();
                if (SecretarisaIds != null && SecretarisaIds.Count > 0)
                {
                    SecretariaList = db.Secretaria.Include(x => x.SecretariaCategoria).Include(x => x.SecretariaNomePrefixo).Where(x => SecretarisaIds.Contains(x.Id) && x.Status != (int)StatusPadrao.Excluido).ToList();
                }
                else
                {
                    SecretariaList = db.Secretaria.Include(x => x.SecretariaCategoria).Include(x => x.SecretariaNomePrefixo).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
                }
            }
            else
            {
                SecretariaList = db.Secretaria.Include(x => x.SecretariaCategoria).Include(x => x.SecretariaNomePrefixo).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            }

            ViewBag.Categorias = SecretariaList.OrderBy(x => x.SecretariaCategoria.Descricao).Select(x => x.SecretariaCategoria.Descricao).ToList().Distinct();
            return View(SecretariaList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Secretaria Secretaria = db.Secretaria.Where(x => x.Id == id).Select(c => new
                                                                        {
                                                                            c,
                                                                            SecretariaServico = c.SecretariaServico.Where(x => x.Status != (int)StatusPadrao.Excluido).OrderBy(x => x.Ordem),
                                                                            SecretariaCategoria = c.SecretariaCategoria,
                                                                            SecretariaNomePrefixo = c.SecretariaNomePrefixo
                                                                        })
                                                                        .ToList().Select(p => p.c).FirstOrDefault();
            if (Secretaria == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            int usuarioID = int.Parse(User.Identity.GetUserId());
            Usuario usuario = db.Usuario.Where(x => x.Id == usuarioID).Select(c => new
                                                                        {
                                                                            c,
                                                                            UsuarioSecretaria = c.UsuarioSecretaria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                                        }).ToList().Select(p => p.c).FirstOrDefault();

            if (usuario.UsuarioSecretaria != null && usuario.UsuarioSecretaria.Count > 0)
            {
                List<int> SecretarisaIds = usuario.UsuarioSecretaria.Select(x => x.SecretariaId).ToList();
                if (SecretarisaIds != null && SecretarisaIds.Count > 0)
                {
                    if (!SecretarisaIds.Contains(Secretaria.Id))
                    {
                        return RedirectToAction("AcessoRestrito", "Home");
                    }
                }
            }

            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;
            ViewBag.Servicos = new SelectList(db.Servico.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Nome");

            return View(Secretaria);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.SecretariaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.Prefixos = new SelectList(db.SecretariaNomePrefixo.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Secretaria model, string Tags, HttpPostedFileBase Icone, HttpPostedFileBase ImagemResponsavel, HttpPostedFileBase ImagemLocal)
        {
            if (ModelState.IsValid)
            {
                string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioSecretaria());
                string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                if (Icone != null)
                {
                    model.Icone = Utils.Utils.UploadImagem(Icone, Path, extensions);

                    if (model.Icone.Contains("Erro:"))
                    {
                        ViewBag.Tag = Tags;
                        ViewBag.Prefixos = new SelectList(db.SecretariaNomePrefixo.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaNomePrefixoId);
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Categorias = new SelectList(db.SecretariaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaCategoriaId);
                        ModelState.AddModelError("Icone", model.Icone);
                        return View(model);
                    }
                }

                if (ImagemResponsavel != null)
                {
                    model.ImagemResponsavel = Utils.Utils.UploadImagem(ImagemResponsavel, Path, extensions);

                    if (model.ImagemResponsavel.Contains("Erro:"))
                    {
                        ViewBag.Tag = Tags;
                        ViewBag.Prefixos = new SelectList(db.SecretariaNomePrefixo.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaNomePrefixoId);
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Categorias = new SelectList(db.SecretariaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaCategoriaId);
                        ModelState.AddModelError("ImagemResponsavel", model.ImagemResponsavel);
                        return View(model);
                    }
                }

                if (ImagemLocal != null)
                {
                    model.ImagemLocal = Utils.Utils.UploadImagem(ImagemLocal, Path, extensions);

                    if (model.ImagemLocal.Contains("Erro:"))
                    {
                        ViewBag.Tag = Tags;
                        ViewBag.Prefixos = new SelectList(db.SecretariaNomePrefixo.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaNomePrefixoId);
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Categorias = new SelectList(db.SecretariaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaCategoriaId);
                        ModelState.AddModelError("ImagemLocal", model.ImagemLocal);
                        return View(model);
                    }
                }

                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;

                string Prefixo = db.SecretariaNomePrefixo.Where(x => x.Id == model.SecretariaNomePrefixoId).Select(x => x.Descricao).FirstOrDefault();

                model.Slug = Utils.Utils.GerarSlug(Prefixo + " " + model.Nome);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Secretaria.Where(x => x.Slug == model.Slug).FirstOrDefault();

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
                return RedirectToAction("Details", "Secretarias", new { id = model.Id, retorno = "Secretaria cadastrado com sucesso!" });
            }
            ViewBag.Tag = Tags;
            ViewBag.Prefixos = new SelectList(db.SecretariaNomePrefixo.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaNomePrefixoId);
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Categorias = new SelectList(db.SecretariaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Secretaria Secretaria = db.Secretaria.Where(x => x.Id == id).FirstOrDefault();

            if (Secretaria == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            int usuarioID = int.Parse(User.Identity.GetUserId());
            Usuario usuario = db.Usuario.Where(x => x.Id == usuarioID).Select(c => new
                                                                        {
                                                                            c,
                                                                            UsuarioSecretaria = c.UsuarioSecretaria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                                        }).ToList().Select(p => p.c).FirstOrDefault();

            if (usuario.UsuarioSecretaria != null && usuario.UsuarioSecretaria.Count > 0)
            {
                List<int> SecretarisaIds = usuario.UsuarioSecretaria.Select(x => x.SecretariaId).ToList();
                if (SecretarisaIds != null && SecretarisaIds.Count > 0)
                {
                    if (!SecretarisaIds.Contains(Secretaria.Id))
                    {
                        return RedirectToAction("AcessoRestrito", "Home");
                    }
                }
            }

            ViewBag.Prefixos = new SelectList(db.SecretariaNomePrefixo.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Secretaria.SecretariaNomePrefixoId);
            ViewBag.Categorias = new SelectList(db.SecretariaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Secretaria.SecretariaCategoriaId);
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Tag = String.Join(",", Secretaria.Tag.Select(x => x.Descricao).ToArray());

            return View(Secretaria);
        }

        [HttpPost]
        public ActionResult Edit(Secretaria model, string Tags, HttpPostedFileBase Icone, HttpPostedFileBase ImagemResponsavel, HttpPostedFileBase ImagemLocal)
        {
            if (ModelState.IsValid)
            {
                string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioSecretaria());
                string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                if (Icone != null)
                {
                    model.Icone = Utils.Utils.UploadImagem(Icone, Path, extensions);

                    if (model.Icone.Contains("Erro:"))
                    {
                        ViewBag.Tag = Tags;
                        ViewBag.Prefixos = new SelectList(db.SecretariaNomePrefixo.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaNomePrefixoId);
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Categorias = new SelectList(db.SecretariaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaCategoriaId);
                        ModelState.AddModelError("Icone", model.Icone);
                        return View(model);
                    }
                }

                if (ImagemResponsavel != null)
                {
                    model.ImagemResponsavel = Utils.Utils.UploadImagem(ImagemResponsavel, Path, extensions);

                    if (model.ImagemResponsavel.Contains("Erro:"))
                    {
                        ViewBag.Tag = Tags;
                        ViewBag.Prefixos = new SelectList(db.SecretariaNomePrefixo.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaNomePrefixoId);
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Categorias = new SelectList(db.SecretariaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaCategoriaId);
                        ModelState.AddModelError("ImagemResponsavel", model.ImagemResponsavel);
                        return View(model);
                    }
                }

                if (ImagemLocal != null)
                {
                    model.ImagemLocal = Utils.Utils.UploadImagem(ImagemLocal, Path, extensions);

                    if (model.ImagemLocal.Contains("Erro:"))
                    {
                        ViewBag.Tag = Tags;
                        ViewBag.Prefixos = new SelectList(db.SecretariaNomePrefixo.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaNomePrefixoId);
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
                        ViewBag.Categorias = new SelectList(db.SecretariaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaCategoriaId);
                        ModelState.AddModelError("ImagemLocal", model.ImagemLocal);
                        return View(model);
                    }
                }

                model.DataAtualizacao = DateTime.Now;

                string Prefixo = db.SecretariaNomePrefixo.Where(x => x.Id == model.SecretariaNomePrefixoId).Select(x => x.Descricao).FirstOrDefault();

                model.Slug = Utils.Utils.GerarSlug(Prefixo + " " + model.Nome);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Secretaria.Where(x => x.Slug == model.Slug && x.Id != model.Id).FirstOrDefault();

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
                return RedirectToAction("Details", "Secretarias", new { id = model.Id, retorno = "Secretaria alterado com sucesso!" });
            }
            ViewBag.Tag = Tags;
            ViewBag.Prefixos = new SelectList(db.SecretariaNomePrefixo.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaNomePrefixoId);
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            ViewBag.Categorias = new SelectList(db.SecretariaCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.SecretariaCategoriaId);
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
            var Secretaria = db.Secretaria.Find(id);
            Secretaria.Status = (int)StatusPadrao.Excluido;
            db.Entry(Secretaria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Secretaria.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Secretaria excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Secretaria = db.Secretaria.Find(id);
            Secretaria.Status = (int)StatusPadrao.Inativo;
            db.Entry(Secretaria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Secretaria.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Secretaria bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Secretaria = db.Secretaria.Find(id);
            Secretaria.Status = (int)StatusPadrao.Ativo;
            db.Entry(Secretaria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Secretaria.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Secretaria desbloqueado com sucesso!" });
        }

        [HttpPost]
        public ActionResult CreateSecretariaServico(SecretariaServico SecretariaServico)
        {
            if (ModelState.IsValid)
            {
                if (SecretariaServico.Id == 0)
                {
                    SecretariaServico.DataCadastro = DateTime.Now;
                    SecretariaServico.Status = (int)StatusPadrao.Ativo;
                    db.Entry(SecretariaServico).State = EntityState.Added;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "SecretariaServico " + SecretariaServico.Id.ToString());
                    return RedirectToAction("Details", "Secretarias", new { id = SecretariaServico.SecretariaId, retorno = "Registro salvo com sucesso!" });
                }
                else
                {
                    SecretariaServico model = db.SecretariaServico.Where(x => x.Id == SecretariaServico.Id).FirstOrDefault();

                    if (model != null)
                    {
                        model.DataAtualizacao = DateTime.Now;
                        model.ServicoId = SecretariaServico.ServicoId;
                        model.Ordem = SecretariaServico.Ordem;

                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "SecretariaServico " + model.Id.ToString());
                        return RedirectToAction("Details", "Secretarias", new { id = SecretariaServico.SecretariaId, retorno = "Registro alterado com sucesso!" });
                    }
                }
            }
            return Json("Informações inválidas");
        }

        public JsonResult EditSecretariaServico(int id)
        {
            if (id == 0)
            {
                return Json("id Obrigatorio", JsonRequestBehavior.AllowGet);
            }
            var SecretariaServico = db.SecretariaServico.Find(id);
            return Json(SecretariaServico, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSecretariaServico(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var SecretariaServico = db.SecretariaServico.Find(id);
            SecretariaServico.Status = (int)StatusPadrao.Excluido;
            db.Entry(SecretariaServico).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "SecretariaServico " + SecretariaServico.Id.ToString());
            return RedirectToAction("Details", "Secretarias", new { id = SecretariaServico.SecretariaId, retorno = "Registro excluído com sucesso!" });
        }

        public ActionResult BlockSecretariaServico(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var SecretariaServico = db.SecretariaServico.Find(id);
            SecretariaServico.Status = (int)StatusPadrao.Inativo;
            db.Entry(SecretariaServico).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "SecretariaServico " + SecretariaServico.Id.ToString());
            return RedirectToAction("Details", "Secretarias", new { id = SecretariaServico.SecretariaId, retorno = "Registro bloqueada com sucesso!" });
        }

        public ActionResult UnBlockSecretariaServico(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var SecretariaServico = db.SecretariaServico.Find(id);
            SecretariaServico.Status = (int)StatusPadrao.Ativo;
            db.Entry(SecretariaServico).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "SecretariaServico " + SecretariaServico.Id.ToString());
            return RedirectToAction("Details", "Secretarias", new { id = SecretariaServico.SecretariaId, retorno = "Registro desbloqueado com sucesso!" });
        }

        public JsonResult DeleteImagemLocal(int Id)
        {
            try
            {
                Secretaria RegistroExistente = db.Secretaria.Where(x => x.Id == Id).FirstOrDefault();
                RegistroExistente.ImagemLocal = "";
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