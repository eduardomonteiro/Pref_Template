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
    public class FaleConoscoController : Controller
    {
        private int currentCodArea = 15;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Contato> ContatoList = db.Contato.Include(x => x.ContatoTipo).Where(x => x.ContatoTipo.ContatoCategoriaId == 1 && x.Status != (int)StatusPadrao.Excluido).ToList();
            ViewBag.Tipos = ContatoList.OrderBy(x => x.ContatoTipo.Descricao).Select(x => x.ContatoTipo.Descricao).ToList().Distinct();
            return View(ContatoList);
        }

        public ActionResult Details(int id = 0, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            Contato Contato = db.Contato.Include(x => x.ContatoTipo).Include(x => x.Bairro).Where(x => x.Id == id).FirstOrDefault();
            if (Contato == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }
            
            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;

            

            if (Contato.StatusFaleConosco == (int)StatusFaleConosco.Novo)
            {
                Contato.DataAtualizacao = DateTime.Now;
                Contato.StatusFaleConosco = (int)StatusFaleConosco.Analise;

                db.Entry(Contato).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            return View(Contato);
        }

        [HttpPost]
        public ActionResult Responder (int Id , string Resposta)
        {
            try
            {
                Contato FaleConosco = db.Contato.SingleOrDefault(c => c.Id == Id);

                String body = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/MailModels/FaleConoscoResposta.html"));

                body = body.Replace("##nome##", FaleConosco.Nome);
                body = body.Replace("##pergunta##", FaleConosco.Mensagem);
                body = body.Replace("##resposta##", Resposta);
                body = body.Replace("../", ("http://##host##/").Replace("##host##", Request.Url.Host));
                body = body.Replace("##url##", "http://" + Request.Url.Host + "/API/Admin");

                var resp = Utils.Utils.EnviaEmail(body, "Fale Conosco", FaleConosco.Email, null);

                if (resp != "OK")
                {
                    return RedirectToAction("Details", "FaleConosco", new { id = Id, retorno = "Houve um erro ao Responder!" });
                }

                FaleConosco.Resposta = Resposta;
                FaleConosco.DataResposta = DateTime.Now;
                FaleConosco.DataAtualizacao = DateTime.Now;
                FaleConosco.StatusFaleConosco = (int)StatusFaleConosco.Respondido;
                FaleConosco.RespostaUsuarioId = Convert.ToInt32(User.Identity.GetUserId());

                db.Entry(FaleConosco).State = EntityState.Modified;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, "Responder " + Id.ToString());
                return RedirectToAction("Details", "FaleConosco", new { id = Id, retorno = "Resposta enviada com sucesso!" });
            }
            catch (Exception e)
            {
                return RedirectToAction("Details", "FaleConosco", new { id = Id, retorno = "Houve um erro ao Responder!" });
            }
            
        }
    }
}