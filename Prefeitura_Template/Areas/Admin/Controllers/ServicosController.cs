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
    public class ServicosController : Controller
    {
        private int currentCodArea = 27;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Servico> ServicoList = new List<Prefeitura_Template.Models.Servico>();

            int usuarioID = int.Parse(User.Identity.GetUserId());
            Usuario usuario = db.Usuario.Where(x => x.Id == usuarioID).Select(c => new
                                                                        {
                                                                            c,
                                                                            UsuarioSecretaria = c.UsuarioSecretaria.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                                        }).ToList().Select(p => p.c).FirstOrDefault();
            if (usuario.UsuarioSecretaria != null && usuario.UsuarioSecretaria.Count > 0)
            {
                List<int> SecretarisaIds = usuario.UsuarioSecretaria.Select(x => x.SecretariaId).ToList();

                if (SecretarisaIds != null && SecretarisaIds.Count > 0)
                {
                    List<Secretaria> Secretarias = db.Secretaria.Where(x => SecretarisaIds.Contains(x.Id)).Select(c => new
                    {
                        c,
                        SecretariaServico = c.SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo),
                    }).ToList().Select(p => p.c).ToList();

                    List<int> ServicosIds = new List<int>();
                    foreach (var item in Secretarias)
                    {
                        if (item.SecretariaServico != null)
                        {
                            foreach (var item2 in item.SecretariaServico)
                            {
                                if (item2.Status == (int)StatusPadrao.Ativo)
                                {
                                    ServicosIds.Add(item2.ServicoId);
                                }
                            }
                        }
                    }

                    var Servicos = db.Servico.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(c => new
                                                                                                {
                                                                                                    c,
                                                                                                    ServicoCategoria = c.ServicoCategoria,
                                                                                                    SecretariaServico = c.SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                                                })
                                                                                                .ToList().Select(p => p.c).ToList();

                    ServicoList = Servicos.Where(x => (ServicosIds.Contains(x.Id) || (x.SecretariaServico == null || !x.SecretariaServico.Any()))).ToList();
                }
                else
                {
                    ServicoList = db.Servico.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(c => new
                                                                                                {
                                                                                                    c,
                                                                                                    ServicoCategoria = c.ServicoCategoria,
                                                                                                    SecretariaServico = c.SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                                                })
                                                                                                .ToList().Select(p => p.c).ToList();
                }
            }
            else
            {
                ServicoList = db.Servico.Where(x => x.Status != (int)StatusPadrao.Excluido).Select(c => new
                                                                                            {
                                                                                                c,
                                                                                                ServicoCategoria = c.ServicoCategoria,
                                                                                                SecretariaServico = c.SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                                            })
                                                                                            .ToList().Select(p => p.c).ToList();
            }

            ViewBag.Categorias = ServicoList.OrderBy(x => x.ServicoCategoria.Descricao).Select(x => x.ServicoCategoria.Descricao).ToList().Distinct();
            return View(ServicoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);

            Servico Servico = db.Servico.Where(x => x.Id == id).Select(c => new
                                                                {
                                                                    c,
                                                                    ServicoArquivo = c.ServicoArquivo.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                                    ServicoPin = c.ServicoPin.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                                    ServicoCategoria = c.ServicoCategoria,
                                                                    SecretariaServico = c.SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                 })
                                                                .ToList().Select(p => p.c).FirstOrDefault();
            if (Servico == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            if (Servico.SecretariaServico != null && Servico.SecretariaServico.Count > 0)
            {
                int usuarioID = int.Parse(User.Identity.GetUserId());
                Usuario usuario = db.Usuario.Where(x => x.Id == usuarioID).Select(c => new
                {
                    c,
                    UsuarioSecretaria = c.UsuarioSecretaria.Where(x => x.Status != (int)StatusPadrao.Excluido),
                }).ToList().Select(p => p.c).FirstOrDefault();
                if (usuario.UsuarioSecretaria != null && usuario.UsuarioSecretaria.Count > 0)
                {
                    List<int> SecretarisaIds = usuario.UsuarioSecretaria.Select(x => x.SecretariaId).ToList();

                    if (SecretarisaIds != null && SecretarisaIds.Count > 0)
                    {
                        List<Secretaria> Secretarias = db.Secretaria.Where(x => SecretarisaIds.Contains(x.Id)).Select(c => new
                        {
                            c,
                            SecretariaServico = c.SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo),
                        }).ToList().Select(p => p.c).ToList();

                        List<int> ServicosIds = new List<int>();
                        foreach (var item in Secretarias)
                        {
                            if (item.SecretariaServico != null)
                            {
                                foreach (var item2 in item.SecretariaServico)
                                {
                                    if (item2.Status == (int)StatusPadrao.Ativo)
                                    {
                                        ServicosIds.Add(item2.ServicoId);
                                    }
                                }
                            }
                        }

                        List<int> Servicos = db.Servico.Include(x => x.ServicoCategoria).Where(x => ServicosIds.Contains(x.Id) && x.Status != (int)StatusPadrao.Excluido).Select(x => x.Id).ToList();

                        if (!Servicos.Contains(Servico.Id))
                        {
                            return RedirectToAction("AcessoRestrito", "Home");
                        }
                    }
                }
            }

            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            return View(Servico);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            ViewBag.Categorias = new SelectList(db.ServicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            ViewBag.Secretarias = new SelectList(db.Secretaria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Nome");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Servico model, HttpPostedFileBase Icone, HttpPostedFileBase Imagem, string Latitudes = "", string Longitudes = "")
        {
            //if (!string.IsNullOrEmpty(Latitudes))
            //    Latitudes = Latitudes.Remove(Latitudes.Length - 1);
            //if (!string.IsNullOrEmpty(Longitudes))
            //    Longitudes = Longitudes.Remove(Longitudes.Length - 1);

            if (ModelState.IsValid)
            {
                if (Icone != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioServico());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Icone = Utils.Utils.UploadImagem(Icone, Path, extensions);

                    if (model.Icone.Contains("Erro:"))
                    {
                        ViewBag.Latitudes = Latitudes;
                        ViewBag.Longitudes = Longitudes;
                        ViewBag.Categorias = new SelectList(db.ServicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.ServicoCategoriaId);
                        ModelState.AddModelError("Icone", model.Icone);
                        return View(model);
                    }
                }

                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioServico());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        //ViewBag.Latitudes = Latitudes;
                        //ViewBag.Longitudes = Longitudes;
                        ViewBag.Categorias = new SelectList(db.ServicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.ServicoCategoriaId);
                        ModelState.AddModelError("Imagem", model.Imagem);
                        return View(model);
                    }
                }

                model.DataCadastro = DateTime.Now;
                model.Status = (int)StatusPadrao.Ativo;
                model.Slug = Utils.Utils.GerarSlug(model.Nome);

                bool SlugVazia = false;
                int j = 1;
                while (SlugVazia == false)
                {
                    var SlugExistente = db.Servico.Where(x => x.Slug == model.Slug).FirstOrDefault();

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

                //if (!string.IsNullOrEmpty(Latitudes))
                //{
                //    string[] latitudes = Latitudes.Split(new char[] { '@' });
                //    string[] longitudes = Longitudes.Split(new char[] { '@' });

                //    for (int I = 0; I < latitudes.Count(); I ++)
                //    {
                //        ServicoPin Pin = new ServicoPin();
                //        Pin.DataCadastro = DateTime.Now;
                //        Pin.Status = (int)StatusPadrao.Ativo;
                //        Pin.ServicoId = model.Id;
                //        Pin.Latitude = latitudes[I];
                //        Pin.Longitude = longitudes[I];

                //        db.Entry(Pin).State = EntityState.Added;
                //    }
                //}

                //db.SaveChanges();

                int usuarioID = int.Parse(User.Identity.GetUserId());
                Usuario usuario = db.Usuario.Where(x => x.Id == usuarioID).Select(c => new
                {
                    c,
                    UsuarioSecretaria = c.UsuarioSecretaria.Where(x => x.Status != (int)StatusPadrao.Excluido),
                }).ToList().Select(p => p.c).FirstOrDefault();
                if (usuario.UsuarioSecretaria != null && usuario.UsuarioSecretaria.Count == 1)
                {
                    SecretariaServico SecretariaServico = new SecretariaServico();
                    SecretariaServico.DataCadastro = DateTime.Now;
                    SecretariaServico.Status = (int)StatusPadrao.Ativo;
                    SecretariaServico.SecretariaId = usuario.UsuarioSecretaria.Select(x => x.SecretariaId).FirstOrDefault();
                    SecretariaServico.ServicoId = model.Id;
                    SecretariaServico.Ordem = 0;
                    db.Entry(SecretariaServico).State = EntityState.Added;
                    db.SaveChanges();
                }

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, model.Id.ToString());
                return RedirectToAction("Details", "Servicos", new { id = model.Id, retorno = "Serviço cadastrado com sucesso!" });
            }

            //ViewBag.Latitudes = Latitudes;
            //ViewBag.Longitudes = Longitudes;
            ViewBag.Categorias = new SelectList(db.ServicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.ServicoCategoriaId);
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            Servico Servico = db.Servico.Where(x => x.Id == id).Select(c => new
                                                                {
                                                                    c,
                                                                    ServicoPin = c.ServicoPin.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                                    SecretariaServico = c.SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                })
                                                                .ToList().Select(p => p.c).FirstOrDefault();
            if (Servico == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            if (Servico.SecretariaServico != null && Servico.SecretariaServico.Count > 0)
            {
                int usuarioID = int.Parse(User.Identity.GetUserId());
                Usuario usuario = db.Usuario.Where(x => x.Id == usuarioID).Select(c => new
                {
                    c,
                    UsuarioSecretaria = c.UsuarioSecretaria.Where(x => x.Status != (int)StatusPadrao.Excluido),
                }).ToList().Select(p => p.c).FirstOrDefault();
                if (usuario.UsuarioSecretaria != null && usuario.UsuarioSecretaria.Count > 0)
                {
                    List<int> SecretarisaIds = usuario.UsuarioSecretaria.Select(x => x.SecretariaId).ToList();

                    if (SecretarisaIds != null && SecretarisaIds.Count > 0)
                    {
                        List<Secretaria> Secretarias = db.Secretaria.Where(x => SecretarisaIds.Contains(x.Id)).Select(c => new
                        {
                            c,
                            SecretariaServico = c.SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo),
                        }).ToList().Select(p => p.c).ToList();

                        List<int> ServicosIds = new List<int>();
                        foreach (var item in Secretarias)
                        {
                            if (item.SecretariaServico != null)
                            {
                                foreach (var item2 in item.SecretariaServico)
                                {
                                    if (item2.Status == (int)StatusPadrao.Ativo)
                                    {
                                        ServicosIds.Add(item2.ServicoId);
                                    }
                                }
                            }
                        }

                        List<int> Servicos = db.Servico.Include(x => x.ServicoCategoria).Where(x => ServicosIds.Contains(x.Id) && x.Status != (int)StatusPadrao.Excluido).Select(x => x.Id).ToList();

                        if (!Servicos.Contains(Servico.Id))
                        {
                            return RedirectToAction("AcessoRestrito", "Home");
                        }
                    }
                }
            }

            ViewBag.Categorias = new SelectList(db.ServicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", Servico.ServicoCategoriaId);
            //ViewBag.Latitudes = String.Join("@", Servico.ServicoPin.Select(x => x.Latitude).ToArray());
            //ViewBag.Longitudes = String.Join("@", Servico.ServicoPin.Select(x => x.Longitude).ToArray());

            return View(Servico);
        }

        [HttpPost]
        public ActionResult Edit(Servico model, HttpPostedFileBase Icone, HttpPostedFileBase Imagem, string Latitudes = "", string Longitudes = "")
        {
            //if (!string.IsNullOrEmpty(Latitudes))
            //    Latitudes = Latitudes.Remove(Latitudes.Length - 1);
            //if (!string.IsNullOrEmpty(Longitudes))
            //    Longitudes = Longitudes.Remove(Longitudes.Length - 1);

            if (ModelState.IsValid)
            {
                if (Icone != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioServico());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Icone = Utils.Utils.UploadImagem(Icone, Path, extensions);

                    if (model.Icone.Contains("Erro:"))
                    {
                        //ViewBag.Latitudes = Latitudes;
                        //ViewBag.Longitudes = Longitudes;
                        ModelState.AddModelError("Imagem", model.Icone);
                        return View(model);
                    }
                }

                if (Imagem != null)
                {
                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioServico());
                    string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    model.Imagem = Utils.Utils.UploadImagem(Imagem, Path, extensions);

                    if (model.Imagem.Contains("Erro:"))
                    {
                        //ViewBag.Latitudes = Latitudes;
                        //ViewBag.Longitudes = Longitudes;
                        ViewBag.Categorias = new SelectList(db.ServicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.ServicoCategoriaId);
                        ModelState.AddModelError("Imagem", model.Imagem);
                        return View(model);
                    }
                }

                model.Slug = Utils.Utils.GerarSlug(model.Nome);

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

                model.DataAtualizacao = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;

                //List<ServicoPin> PinsExistentes = db.ServicoPin.Where(x => x.ServicoId == model.Id).ToList();

                //if (PinsExistentes != null && PinsExistentes.Count > 0)
                //{
                //    foreach (ServicoPin DeletePin in PinsExistentes)
                //    {
                //        db.Entry(DeletePin).State = EntityState.Deleted;
                //    }
                //}

                //if (!string.IsNullOrEmpty(Latitudes))
                //{
                //    string[] latitudes = Latitudes.Split(new char[] { '@' });
                //    string[] longitudes = Longitudes.Split(new char[] { '@' });

                //    for (int I = 0; I < latitudes.Count(); I++)
                //    {
                //        ServicoPin Pin = new ServicoPin();
                //        Pin.DataCadastro = DateTime.Now;
                //        Pin.Status = (int)StatusPadrao.Ativo;
                //        Pin.ServicoId = model.Id;
                //        Pin.Latitude = latitudes[I];
                //        Pin.Longitude = longitudes[I];

                //        db.Entry(Pin).State = EntityState.Added;
                //    }
                //}

                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Edicao, model.Id.ToString());
                return RedirectToAction("Details", "Servicos", new { id = model.Id, retorno = "Serviço alterado com sucesso!" });
            }

            //ViewBag.Latitudes = Latitudes;
            //ViewBag.Longitudes = Longitudes;
            ViewBag.Categorias = new SelectList(db.ServicoCategoria.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", model.ServicoCategoriaId);
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
            var Servico = db.Servico.Find(id);
            Servico.Status = (int)StatusPadrao.Excluido;
            db.Entry(Servico).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, Servico.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Serviço excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Servico = db.Servico.Find(id);
            Servico.Status = (int)StatusPadrao.Inativo;
            db.Entry(Servico).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, Servico.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Serviço bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var Servico = db.Servico.Find(id);
            Servico.Status = (int)StatusPadrao.Ativo;
            db.Entry(Servico).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, Servico.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Serviço desbloqueado com sucesso!" });
        }

        [HttpPost]
        public ActionResult UploadFiles(string ArquivoNome, int ServicoId, HttpPostedFileBase Arquivo, int Id = 0)
        {
            if (Id == 0)
            {
                if (Arquivo != null)
                {
                    ServicoArquivo model = new ServicoArquivo();

                    string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioServico());
                    string[] extensions = { ".pdf" };

                    model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                    if (model.Arquivo.Contains("Erro:"))
                    {
                        return RedirectToAction("Details", "Servicos", new { id = ServicoId, retorno = model.Arquivo });
                    }
                    else
                    {
                        model.ArquivoNome = ArquivoNome;
                        model.ServicoId = ServicoId;
                        model.DataCadastro = DateTime.Now;
                        model.Status = 1;
                        model.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);

                        db.Entry(model).State = EntityState.Added;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "Arquivo " + model.Id.ToString());
                        return RedirectToAction("Details", "Servicos", new { id = ServicoId, retorno = "Arquivo salvo com sucesso!" });
                    }
                }
            }
            else
            {
                ServicoArquivo model = db.ServicoArquivo.Where(x => x.Id == Id).FirstOrDefault();

                if (model != null)
                {
                    if (Arquivo != null)
                    {
                        string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioServico());
                        string[] extensions = { ".pdf" };

                        model.Arquivo = Utils.Utils.UploadImagem(Arquivo, Path, extensions);

                        if (model.Arquivo.Contains("Erro:"))
                        {
                            return RedirectToAction("Details", "Servicos", new { id = ServicoId, retorno = model.Arquivo });
                        }
                        model.Tamanho = Utils.Utils.TamanhoEmMB(Arquivo.ContentLength);
                    }

                    model.ArquivoNome = ArquivoNome;
                    model.DataAtualizacao = DateTime.Now;


                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "Arquivo " + model.Id.ToString());
                    return RedirectToAction("Details", "Servicos", new { id = ServicoId, retorno = "Arquivo alterado com sucesso!" });
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
            var ServicoArquivo = db.ServicoArquivo.Find(id);
            return Json(ServicoArquivo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArquivo(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ServicoArquivo = db.ServicoArquivo.Find(id);
            ServicoArquivo.Status = (int)StatusPadrao.Excluido;
            db.Entry(ServicoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "Arquivo " + ServicoArquivo.Id.ToString());
            return RedirectToAction("Details", "Servicos", new { id = ServicoArquivo.ServicoId, retorno = "Arquivo excluído com sucesso!" });
        }

        public ActionResult BlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ServicoArquivo = db.ServicoArquivo.Find(id);
            ServicoArquivo.Status = (int)StatusPadrao.Inativo;
            db.Entry(ServicoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "Arquivo " + ServicoArquivo.Id.ToString());
            return RedirectToAction("Details", "Servicos", new { id = ServicoArquivo.ServicoId, retorno = "Arquivo bloqueado com sucesso!" });
        }

        public ActionResult UnBlockArquivo(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ServicoArquivo = db.ServicoArquivo.Find(id);
            ServicoArquivo.Status = (int)StatusPadrao.Ativo;
            db.Entry(ServicoArquivo).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "Arquivo " + ServicoArquivo.Id.ToString());
            return RedirectToAction("Details", "Servicos", new { id = ServicoArquivo.ServicoId, retorno = "Arquivo desbloqueado com sucesso!" });
        }

        [HttpPost]
        public ActionResult CreatePin(ServicoPin Pin)
        {
            if (ModelState.IsValid)
            {
                if (Pin.Id == 0)
                {
                    Pin.DataCadastro = DateTime.Now;
                    Pin.Status = (int)StatusPadrao.Ativo;
                    db.Entry(Pin).State = EntityState.Added;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "Pin " + Pin.Id.ToString());
                    return RedirectToAction("Details", "Servicos", new { id = Pin.ServicoId, retorno = "Pin salvo com sucesso!" });
                }
                else
                {
                    ServicoPin model = db.ServicoPin.Where(x => x.Id == Pin.Id).FirstOrDefault();

                    if (model != null)
                    {
                        model.DataAtualizacao = DateTime.Now;
                        model.Latitude = Pin.Latitude;
                        model.Longitude = Pin.Longitude;

                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "Pin " + model.Id.ToString());
                        return RedirectToAction("Details", "Servicos", new { id = Pin.ServicoId, retorno = "Pin alterado com sucesso!" });
                    }
                }
            }
            return Json("Informações inválidas");
        }

        public JsonResult EditPin(int id)
        {
            if (id == 0)
            {
                return Json("id Obrigatorio", JsonRequestBehavior.AllowGet);
            }
            var ServicoPin = db.ServicoPin.Find(id);
            return Json(ServicoPin, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePin(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ServicoPin = db.ServicoPin.Find(id);
            ServicoPin.Status = (int)StatusPadrao.Excluido;
            db.Entry(ServicoPin).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "Pin " + ServicoPin.Id.ToString());
            return RedirectToAction("Details", "Servicos", new { id = ServicoPin.ServicoId, retorno = "Pin excluído com sucesso!" });
        }

        public ActionResult BlockPin(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ServicoPin = db.ServicoPin.Find(id);
            ServicoPin.Status = (int)StatusPadrao.Inativo;
            db.Entry(ServicoPin).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "Pin " + ServicoPin.Id.ToString());
            return RedirectToAction("Details", "Servicos", new { id = ServicoPin.ServicoId, retorno = "Pin bloqueada com sucesso!" });
        }

        public ActionResult UnBlockPin(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var ServicoPin = db.ServicoPin.Find(id);
            ServicoPin.Status = (int)StatusPadrao.Ativo;
            db.Entry(ServicoPin).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "Pin " + ServicoPin.Id.ToString());
            return RedirectToAction("Details", "Servicos", new { id = ServicoPin.ServicoId, retorno = "Pin desbloqueado com sucesso!" });
        }

        public JsonResult DeleteImagem(int Id)
        {
            try
            {
                Servico RegistroExistente = db.Servico.Where(x => x.Id == Id).FirstOrDefault();
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