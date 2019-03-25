using Microsoft.AspNet.Identity;
using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Areas.Admin.Utils;
using Prefeitura_Template.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prefeitura_Template.Areas.Admin.Controllers
{
    [Authorize]
    public class CidadeController : Controller
    {
        private int currentCodArea = 2;
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;
            Cidade Cidade = db.Cidade.FirstOrDefault();
            return View(Cidade);
        }

        public ActionResult Edit()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            Cidade Cidade = db.Cidade.FirstOrDefault();
            return View(Cidade);
        }

        [HttpPost]
        public ActionResult Edit(Cidade model, HttpPostedFileBase ImagemDescricao, HttpPostedFileBase ImagemBandeira, HttpPostedFileBase ImagemBrasao, HttpPostedFileBase ImagemInvista, HttpPostedFileBase AudioHino)
        {
            if (ModelState.IsValid)
            {
                Cidade RegistroExistente = db.Cidade.FirstOrDefault();
                RegistroExistente.Descricao = model.Descricao;
                RegistroExistente.DescricaoBandeira = model.DescricaoBandeira;
                RegistroExistente.DescricaoBrasao = model.DescricaoBrasao;
                RegistroExistente.DescricaoInvista = model.DescricaoInvista;
                RegistroExistente.DescricaoHino = model.DescricaoHino;
                RegistroExistente.DataFundacao = model.DataFundacao;
                RegistroExistente.AtualPrefeito = model.AtualPrefeito;
                RegistroExistente.Populacao = model.Populacao;
                RegistroExistente.Clima = model.Clima;
                RegistroExistente.Area = model.Area;
                RegistroExistente.Densidade = model.Densidade;
                RegistroExistente.Altitude = model.Altitude;
                RegistroExistente.Telefone = model.Telefone;
                RegistroExistente.Email = model.Email;

                string Path = System.Web.HttpContext.Current.Server.MapPath(Utils.Utils.RetornaDiretorioCidade());
                string[] extensions = { ".jpg", ".jpeg", ".png", ".gif" };

                if (ImagemDescricao != null)
                {
                    RegistroExistente.ImagemDescricao = Utils.Utils.UploadImagem(ImagemDescricao, Path, extensions);

                    if (RegistroExistente.ImagemDescricao.Contains("Erro:"))
                    {
                        ModelState.AddModelError("ImagemDescricao", RegistroExistente.ImagemDescricao);
                        return View(model);
                    }
                }

                if (ImagemBandeira != null)
                {
                    RegistroExistente.ImagemBandeira = Utils.Utils.UploadImagem(ImagemBandeira, Path, extensions);

                    if (RegistroExistente.ImagemBandeira.Contains("Erro:"))
                    {
                        ModelState.AddModelError("ImagemBandeira", RegistroExistente.ImagemBandeira);
                        return View(model);
                    }
                }

                if (ImagemBrasao != null)
                {
                    RegistroExistente.ImagemBrasao = Utils.Utils.UploadImagem(ImagemBrasao, Path, extensions);

                    if (RegistroExistente.ImagemBrasao.Contains("Erro:"))
                    {
                        ModelState.AddModelError("ImagemBrasao", RegistroExistente.ImagemBrasao);
                        return View(model);
                    }
                }

                if (ImagemInvista != null)
                {
                    RegistroExistente.ImagemInvista = Utils.Utils.UploadImagem(ImagemInvista, Path, extensions);

                    if (RegistroExistente.ImagemInvista.Contains("Erro:"))
                    {
                        ModelState.AddModelError("ImagemInvista", RegistroExistente.ImagemInvista);
                        return View(model);
                    }
                }

                if (AudioHino != null)
                {
                    string[] extensionsaaudio = { ".mp3", ".wma", ".aac", ".ogg", ".ac3", ".wav" };
                    RegistroExistente.AudioHino = Utils.Utils.UploadImagem(AudioHino, Path, extensionsaaudio);

                    if (RegistroExistente.AudioHino.Contains("Erro:"))
                    {
                        ModelState.AddModelError("AudioHino", RegistroExistente.AudioHino);
                        return View(model);
                    }
                }

                RegistroExistente.DataAtualizacao = DateTime.Now;
                db.Entry(RegistroExistente).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId<int>()), currentCodArea, TipoAcao.Edicao, "");
                return RedirectToAction("Index", "Cidade", new { retorno = "Registro Alterado com sucesso!" });
            }

            var usuario = db.Usuario.Find(model.Id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            return View(usuario);
        }

        public JsonResult DeleteImagemDescricao(int Id)
        {
            try
            {
                Cidade RegistroExistente = db.Cidade.Where(x => x.Id == Id).FirstOrDefault();
                RegistroExistente.ImagemDescricao = "";
                RegistroExistente.DataAtualizacao = DateTime.Now;
                db.Entry(RegistroExistente).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new
                {
                    Sucesso = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new
                {
                    Sucesso = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteImagemBandeira(int Id)
        {
            try
            {
                Cidade RegistroExistente = db.Cidade.Where(x => x.Id == Id).FirstOrDefault();
                RegistroExistente.ImagemBandeira = "";
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

        public JsonResult DeleteImagemBrasao(int Id)
        {
            try
            {
                Cidade RegistroExistente = db.Cidade.Where(x => x.Id == Id).FirstOrDefault();
                RegistroExistente.ImagemBrasao = "";
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

        public JsonResult DeleteImagemInvista(int Id)
        {
            try
            {
                Cidade RegistroExistente = db.Cidade.Where(x => x.Id == Id).FirstOrDefault();
                RegistroExistente.ImagemInvista = "";
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

        public JsonResult DeleteAudioHino(int Id)
        {
            try
            {
                Cidade RegistroExistente = db.Cidade.Where(x => x.Id == Id).FirstOrDefault();
                RegistroExistente.AudioHino = "";
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