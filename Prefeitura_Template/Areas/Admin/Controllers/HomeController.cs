using Prefeitura_Template.ActionFilter;
using System.Web.Mvc;
using Prefeitura_Template.Models;
using Prefeitura_Template.Areas.Admin.Utils;

namespace Prefeitura_Template.Areas.Admin.Controllers
{
    [AutenticacaoAttribute]
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AcessoRestrito()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult ListEnum(string enumName)
        {
            var type = typeof(ApplicationDbContext).Assembly.GetType(enumName);
            if (type == null)
            {
                type = GetType().Assembly.GetType(enumName);
            }
            return Json(EnumExtensions.GetItems(type));
        }
    }
}