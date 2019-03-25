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
    public class NewsLetterController : Controller
    {
        private int currentCodArea = 23;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            List<NewsLetter> NewsLetterList = db.NewsLetter.Where(x => x.Status != (int)StatusPadrao.Excluido).ToList();
            return View(NewsLetterList);
        }

        public void ExportExc(List<long> inter)
        {
            var items = db.NewsLetter.Where(x => inter.Contains(x.Id)).ToList();

            List<NewsLetterViewModel> News = items.Select(x => new NewsLetterViewModel
                                                    {
                                                        DataCadastro = x.DataCadastro,
                                                        Nome = x.Nome,
                                                        Email = x.Email,
                                                        Status = x.NomeStatus,
                                                        Sexo = x.Sexo
                                                    }).ToList();
            Utils.Utils.ExportToExcel(News, "NewsLetter");
        }
    }
}