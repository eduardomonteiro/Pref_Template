using AutoMapper;
using Microsoft.AspNet.Identity;
using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Areas.Admin.Models;
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
    public class DocumentosController : Controller
    {
        private int currentCodArea = 10;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Documento> DocumentoList = db.Documento.Include(x => x.DocumentoCategoria).Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Categorias = DocumentoList.OrderBy(x => x.DocumentoCategoria.Descricao).Select(x => x.DocumentoCategoria.Descricao).ToList().Distinct();
            return View(DocumentoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Documento Documento = db.Documento.Include(x => x.DocumentoCategoria).Include(x => x.DocumentoArquivo).Where(x => x.Id == id).FirstOrDefault();
            if (Documento == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(Documento);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.DocumentoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();
            return View();
        }

        [HttpPost]
        public ActionResult Create(DocumentoViewModel model, HttpPostedFileBase Arquivo)
        {
            if (ModelState.IsValid)
            {
                Documento Documento = new Documento();
                Mapper.Map(model, Documento);

                if (Arquivo != null)
                {
                    DocumentoArquivo DocumentoArquivo = new DocumentoArquivo();
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioDocumento());
                    string[] extensions = { ".pdf"};

                    DocumentoArquivo.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (DocumentoArquivo.Arquivo.Contains("Erro:"))
                    {
                        ModelState.AddModelError("Arquivo", DocumentoArquivo.Arquivo);

                        ViewBag.Categorias = new SelectList(db.DocumentoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.DocumentoCategoriaId);
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

                        return View(model);
                    }

                    DocumentoArquivo.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);
                    DocumentoArquivo.ArquivoNome = model.ArquivoNome;
                    DocumentoArquivo.DataCadastro = DateTime.Now;
                    DocumentoArquivo.Status = (int)StatusPadrao.Ativo;
                    Documento.DocumentoArquivo.Add(DocumentoArquivo);
                }

                Documento.DataCadastro = DateTime.Now;
                Documento.Status = (int)StatusPadrao.Ativo;
                Documento.DataPublicacao = DateTime.Now;
                db.Entry(Documento).State = EntityState.Added;
                db.SaveChanges();

                if (!string.IsNullOrEmpty(model.Tag))
                {
                    string[] tags = model.Tag.Split(new char[] { ',' });

                    foreach (var item in tags)
                    {
                        Tag Tag = new Tag();
                        Tag.DataCadastro = DateTime.Now;
                        Tag.Status = (int)StatusPadrao.Ativo;
                        Tag.AreaId = currentCodArea;
                        Tag.Descricao = item;
                        Tag.Slug = Utils.Utils.GerarSlug(item);
                        Tag.RegistroId = Documento.Id;

                        db.Entry(Tag).State = EntityState.Added;
                        db.SaveChanges();
                    }
                }

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, Documento.Id.ToString());
                return RedirectToAction("Details", "Documentos", new { id = Documento.Id, retorno = "Documento cadastrado com sucesso!" });
            }

            ViewBag.Categorias = new SelectList(db.DocumentoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.DocumentoCategoriaId);
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Documento Documento = db.Documento.Include(x => x.DocumentoArquivo).Where(x => x.Id == id).FirstOrDefault();

            if (Documento == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Categorias = new SelectList(db.DocumentoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Documento.DocumentoCategoriaId);
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

            DocumentoViewModel model = new DocumentoViewModel();
            Mapper.Map(Documento, model);

            model.ArquivoNome = Documento.DocumentoArquivoAtual.ArquivoNome;
            model.Arquivo = Documento.DocumentoArquivoAtual.Arquivo;
            model.CaminhoLogicoArquivo = Documento.DocumentoArquivoAtual.CaminhoLogicoArquivo;
            model.Tag = String.Join(",", Documento.Tag.Select(x => x.Descricao).ToArray());

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DocumentoViewModel model, HttpPostedFileBase Arquivo)
        {
            if (ModelState.IsValid)
            {
                Documento Documento = new Documento();
                Mapper.Map(model, Documento);

                if (Arquivo != null)
                {
                    DocumentoArquivo DocumentoArquivo = new DocumentoArquivo();
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioDocumento());
                    string[] extensions = { ".pdf" };

                    DocumentoArquivo.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (DocumentoArquivo.Arquivo.Contains("Erro:"))
                    {
                        ModelState.AddModelError("Arquivo", DocumentoArquivo.Arquivo);

                        ViewBag.Categorias = new SelectList(db.DocumentoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
                        ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

                        return View(model);
                    }

                    DocumentoArquivo.DocumentoId = model.Id;
                    DocumentoArquivo.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);
                    DocumentoArquivo.ArquivoNome = model.ArquivoNome;
                    DocumentoArquivo.DataCadastro = DateTime.Now;
                    DocumentoArquivo.Status = (int)StatusPadrao.Ativo;

                    List<DocumentoArquivo> DocumentoArquivoExistentes = db.DocumentoArquivo.Where(x => x.DocumentoId == Documento.Id && x.Status == (int)StatusPadrao.Ativo).ToList();

                    if (DocumentoArquivoExistentes != null && DocumentoArquivoExistentes.Count > 0)
                    {
                        foreach (DocumentoArquivo DeleteDocumentoArquivo in DocumentoArquivoExistentes)
                        {
                            DeleteDocumentoArquivo.Status = (int)StatusPadrao.Excluido;
                            DeleteDocumentoArquivo.DataAtualizacao = DateTime.Now;
                            db.Entry(DeleteDocumentoArquivo).State = EntityState.Modified;
                        }
                    }

                    db.Entry(DocumentoArquivo).State = EntityState.Added;
                }

                Documento.DataCadastro = DateTime.Now;
                
                db.Entry(Documento).State = EntityState.Modified;

                List<Tag> TagsExistentes = db.Tag.Where(x => x.RegistroId == Documento.Id && x.AreaId == currentCodArea).ToList();
                
                if(TagsExistentes != null && TagsExistentes.Count > 0)
                {
                    foreach (Tag DeleteTag in TagsExistentes)
                    {
                        db.Entry(DeleteTag).State = EntityState.Deleted;
                    }
                }

                if (!string.IsNullOrEmpty(model.Tag))
                {
                    string[] tags = model.Tag.Split(new char[] { ',' });

                    foreach (var item in tags)
                    {
                        Tag Tag = new Tag();
                        Tag.DataCadastro = DateTime.Now;
                        Tag.Status = (int)StatusPadrao.Ativo;
                        Tag.AreaId = currentCodArea;
                        Tag.Descricao = item;
                        Tag.Slug = Utils.Utils.GerarSlug(item);
                        Tag.RegistroId = Documento.Id;

                        db.Entry(Tag).State = EntityState.Added;
                        
                    }
                }

                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, Documento.Id.ToString());
                return RedirectToAction("Details", "Documentos", new { id = Documento.Id, retorno = "Documento alterado com sucesso!" });
            }

            ViewBag.Categorias = new SelectList(db.DocumentoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.AutoCompleteTags = db.Tag.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(x => x.Descricao).Distinct().ToArray();

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
            var Documento = db.Documento.Find(id);
            Documento.Status = (int)StatusPadrao.Excluido;
            db.Entry(Documento).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Documento.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Documento excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Documento = db.Documento.Find(id);
            Documento.Status = (int)StatusPadrao.Inativo;
            db.Entry(Documento).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Documento.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Documento bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Documento = db.Documento.Find(id);
            Documento.Status = (int)StatusPadrao.Ativo;
            db.Entry(Documento).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Documento.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Documento desbloqueado com sucesso!" });
        }
    }
}