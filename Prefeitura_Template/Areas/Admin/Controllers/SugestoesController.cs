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
    public class SugestoesController : Controller
    {
        private int currentCodArea = 16;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            ViewBag.Retorno = retorno;
            List<Contato> ContatoList = db.Contato.Include(x => x.ContatoTipo).Where(x => x.ContatoTipo.ContatoCategoriaId == 2 && x.Status != (int)StatusPadrao.Excluido).ToList();
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

            return View(Contato);
        }
    }
}