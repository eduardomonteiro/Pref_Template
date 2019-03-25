using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Prefeitura_Template.Models;
using Prefeitura_Template.Areas.Admin.Utils;
using Prefeitura_Template.Areas.Admin.Enums;
using System.Net;
using System.Data.Entity;
using Prefeitura_Template.Areas.Admin.Models;
using System.Collections.Generic;
using Microsoft.Owin.Security.DataProtection;

namespace Prefeitura_Template.Areas.Admin.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int currentCodArea = 1;
        
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UsuariosController()
        {

        }

        public UsuariosController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(int? page, string search, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            int pageNumber = (page ?? 1);

            var Usuarios = db.Usuario.Include(x => x.Perfil).Where(x => x.Status != (int)StatusPadrao.Excluido)
                                     .OrderByDescending(q => q.DataAtualizacao)
                                     .ToList();

            ViewBag.Perfis = Usuarios.OrderBy(x => x.Perfil.Descricao).Select(x => x.Perfil.Descricao).ToList().Distinct();

            ViewBag.Retorno = retorno;
            Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId<int>()), currentCodArea, TipoAcao.Visualizacao, "");
            return View(Usuarios);
        }

        public ActionResult Create()
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), true, false, false, false);
            int UsarioLogadoId = User.Identity.GetUserId<int>();
            Usuario UsuarioLogado = db.Usuario.Where(x => x.Id == UsarioLogadoId).FirstOrDefault();
            if(UsuarioLogado.PerfilId == 1)
            {
                ViewBag.Perfis = new SelectList(db.Perfil.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            }
            else
            {
                ViewBag.Perfis = new SelectList(db.Perfil.Where(x => x.Id != 1 && x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Usuario user = new Usuario
                {
                    Email = model.Email,
                    Nome = model.Nome,
                    UserName = model.Email,
                    Status = (int)StatusPadrao.Ativo,
                    DataCadastro = DateTime.Now,
                    PerfilId = model.PerfilId,
                    Tema = "sknPadrao"
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var usuario = db.Usuario.Where(x => x.Email == model.Email).AsQueryable().FirstOrDefault();
                    Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId()), currentCodArea, TipoAcao.Adicao, usuario.Id.ToString());
                    return RedirectToAction("Details", "Usuarios", new { id = usuario.Id, retorno = "Usuário cadastrado com sucesso!" });
                }
                AddErrors(result);
            }

            int UsarioLogadoId = User.Identity.GetUserId<int>();
            Usuario UsuarioLogado = db.Usuario.Where(x => x.Id == UsarioLogadoId).FirstOrDefault();
            if (UsuarioLogado.PerfilId == 1)
            {
                ViewBag.Perfis = new SelectList(db.Perfil.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            }
            else
            {
                ViewBag.Perfis = new SelectList(db.Perfil.Where(x => x.Id != 1 && x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);

            var usu = db.Usuario.Find(id);

            if (usu == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            List<Perfil> Perfis = new List<Perfil>();

            int UsarioLogadoId = User.Identity.GetUserId<int>();
            Usuario UsuarioLogado = db.Usuario.Where(x => x.Id == UsarioLogadoId).FirstOrDefault();
            if (UsuarioLogado.PerfilId == 1)
            {
                ViewBag.Perfis = new SelectList(db.Perfil.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao" , usu.PerfilId);
            }
            else
            {
                ViewBag.Perfis = new SelectList(db.Perfil.Where(x => x.Id != 1 && x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao", usu.PerfilId);
            }

            if (usu == null)
            {
                return HttpNotFound();
            }

            var model = new Usuario()
            {
                Id = usu.Id,
                Nome = usu.Nome,
                Email = usu.Email,
                PerfilId = usu.PerfilId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario model)
        {
            if (ModelState.IsValid)
            {
                var usu = db.Usuario.Find(model.Id);
                if (usu == null)
                {
                    return HttpNotFound();
                }

                usu.Nome = model.Nome;
                usu.Email = model.Email;
                usu.UserName = model.Email;
                usu.PerfilId = model.PerfilId;
                usu.DataAtualizacao = DateTime.Now;

                db.Entry(usu).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId<int>()), currentCodArea, TipoAcao.Edicao, usu.Id.ToString());
                return RedirectToAction("Details", "Usuarios", new { usu.Id, retorno = "Usuário alterado com sucesso!" });
            }

            var usuario = db.Usuario.Find(model.Id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            int UsarioLogadoId = User.Identity.GetUserId<int>();
            Usuario UsuarioLogado = db.Usuario.Where(x => x.Id == UsarioLogadoId).FirstOrDefault();
            if (UsuarioLogado.PerfilId == 1)
            {
                ViewBag.Perfis = new SelectList(db.Perfil.Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            }
            else
            {
                ViewBag.Perfis = new SelectList(db.Perfil.Where(x => x.Id != 1 && x.Status == (int)StatusPadrao.Ativo), "Id", "Descricao");
            }

            return View(usuario);
        }

        [AllowAnonymous]
        public ActionResult ChangePassword(string email)
        {
            if (email != null)
            {
                ViewBag.Email = email;
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.Error = "E-mail não encontrado!";
                return RedirectToAction("Error", "Home");

            }

            var provider = new DpapiDataProtectionProvider("Prefeitura");
            UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, int>(provider.Create("UserToken"))
                as IUserTokenProvider<ApplicationUser, int>;

            model.Code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var code = model.Code.Replace(" ", "+");
            var passAdd = await UserManager.ResetPasswordAsync(user.Id, code, model.Password);
            if (passAdd.Succeeded)
            {
                return RedirectToAction("Details", "Usuarios", new { id = user.Id, retorno = "Senha alterada com sucesso!"});
            }
            else
            {
                AddErrors(passAdd);
            }

            ViewBag.Email = model.Email;
            return View(model);
        }

        public ActionResult Details(int id, string retorno = "")
        {
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, true, false, false);
            var usuario = db.Usuario.Where(x => x.Id == id).Select(c => new
                                                                                    {
                                                                                        c,
                                                                                        UsuarioSecretaria = c.UsuarioSecretaria.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                                                        Perfil = c.Perfil
                                                                                    }).ToList().Select(p => p.c).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index", new { retorno = "Registro inexistente" });
            }

            ViewBag.Retorno = retorno;
            ViewBag.codAreaAtual = currentCodArea;
            ViewBag.Secretarias = new SelectList(db.Secretaria.Include(y => y.SecretariaNomePrefixo).Where(x => x.Status == (int)StatusPadrao.Ativo), "Id", "NomeComPrefixo");

            Logs.salvarLog(Convert.ToInt32(User.Identity.GetUserId<int>()), currentCodArea, TipoAcao.Visualizacao, id.ToString());
            return View(usuario);
        }

        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var usu = db.Usuario.Find(id);
            usu.Email = "Deleted:" + usu.Email;
            usu.UserName = "Deleted:" + usu.Email;
            usu.Email = "Deleted:" + usu.Email;
            usu.Status = (int)StatusPadrao.Excluido;
            db.Entry(usu).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, usu.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Usuário excluído com sucesso!" });
        }

        public ActionResult BlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var usu = db.Usuario.Find(id);
            usu.Status = (int)StatusPadrao.Inativo;
            db.Entry(usu).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, usu.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Usuário bloqueado com sucesso!" });
        }

        public ActionResult UnBlockItem(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var usu = db.Usuario.Find(id);
            usu.Status = (int)StatusPadrao.Ativo;
            db.Entry(usu).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, usu.Id.ToString());
            return RedirectToAction("Index", new { retorno = "Usuário desbloqueado com sucesso!" });
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            if (result == SignInStatus.Success)
            {
                Usuario User = db.Usuario.Where(x => x.Email == model.Email).FirstOrDefault();
                if (User.Email != "admin@am4.com.br" && User.Status != (int)StatusPadrao.Ativo)
                {
                    ModelState.AddModelError("", "Usuário bloqueado");
                    return View(model);
                }
                Session.Add("UserName", User.Nome);
                Session.Add("thm", User.Tema);
                Logs.salvarLog(User.Id, currentCodArea, TipoAcao.Login, "");
                return RedirectToLocal(returnUrl);
            }
            else if (result == SignInStatus.LockedOut)
            {
                ModelState.AddModelError("", "Usuário bloqueadp");
                return View(model);
            }
            else if (result == SignInStatus.Failure)
            {
                ModelState.AddModelError("", "Login Inválido");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Falha ao tentar logar");
                return View(model);
            }
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Usuario.Where(x => x.Email == model.Email).FirstOrDefault();
                if (user != null)
                {
                    string Senha = "";
                    Random oRandom = new Random();
                    for (int i = 0; i <= 5; i++)
                    {
                        Senha += oRandom.Next(0, 9).ToString();
                    }

                    var provider = new DpapiDataProtectionProvider("Prefeitura");
                    UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, int>(provider.Create("UserToken"))
                                                         as IUserTokenProvider<ApplicationUser, int>;
                    string Code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var result = await UserManager.ResetPasswordAsync(user.Id, Code, Senha);
                    if (result.Errors.Any())
                    {
                        var errs = result.Errors.Where(q => q.ToLower().Contains("senha")).FirstOrDefault();
                        if (!string.IsNullOrEmpty(errs))
                        {
                            ModelState.AddModelError("Password", errs);
                        }
                    }
                    AddErrors(result);

                    String body = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/MailModels/RecuperacaoSenha.html"));

                    body = body.Replace("##nome##", user.Nome);
                    body = body.Replace("##senha##", Senha);
                    body = body.Replace("../", ("http://##host##/").Replace("##host##", Request.Url.Host));
                    body = body.Replace("##url##", "http://" + Request.Url.Host + "/API/Admin");

                    string retorno = Utils.Utils.EnviaEmail(body, "Esqueci minha senha", model.Email, null);

                    if (retorno == "OK")
                    {
                        return View("ForgotPasswordConfirmation");
                    }
                    else
                    {
                        ViewBag.Erro = retorno;
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.Erro = "Email não cadastrado";
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateUsuarioSecretaria(UsuarioSecretaria UsuarioSecretaria)
        {
            if (ModelState.IsValid)
            {
                if (UsuarioSecretaria.Id == 0)
                {
                    UsuarioSecretaria.DataCadastro = DateTime.Now;
                    UsuarioSecretaria.Status = (int)StatusPadrao.Ativo;
                    db.Entry(UsuarioSecretaria).State = EntityState.Added;
                    db.SaveChanges();

                    Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Adicao, "UsuarioSecretaria " + UsuarioSecretaria.Id.ToString());
                    return RedirectToAction("Details", "Usuarios", new { id = UsuarioSecretaria.UsuarioId, retorno = "Registro salvo com sucesso!" });
                }
                else
                {
                    UsuarioSecretaria model = db.UsuarioSecretaria.Where(x => x.Id == UsuarioSecretaria.Id).FirstOrDefault();

                    if (model != null)
                    {
                        model.DataAtualizacao = DateTime.Now;
                        model.SecretariaId = UsuarioSecretaria.SecretariaId;

                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();

                        Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Edicao, "UsuarioSecretaria " + model.Id.ToString());
                        return RedirectToAction("Details", "Usuarios", new { id = UsuarioSecretaria.UsuarioId, retorno = "Registro alterado com sucesso!" });
                    }
                }
            }
            return Json("Informações inválidas");
        }

        public JsonResult EditUsuarioSecretaria(int id)
        {
            if (id == 0)
            {
                return Json("id Obrigatorio", JsonRequestBehavior.AllowGet);
            }
            var UsuarioSecretaria = db.UsuarioSecretaria.Find(id);
            return Json(UsuarioSecretaria, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteUsuarioSecretaria(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, false, true);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var UsuarioSecretaria = db.UsuarioSecretaria.Find(id);
            UsuarioSecretaria.Status = (int)StatusPadrao.Excluido;
            db.Entry(UsuarioSecretaria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Exclusao, "UsuarioSecretaria " + UsuarioSecretaria.Id.ToString());
            return RedirectToAction("Details", "Usuarios", new { id = UsuarioSecretaria.UsuarioId, retorno = "Registro excluído com sucesso!" });
        }

        public ActionResult BlockUsuarioSecretaria(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var UsuarioSecretaria = db.UsuarioSecretaria.Find(id);
            UsuarioSecretaria.Status = (int)StatusPadrao.Inativo;
            db.Entry(UsuarioSecretaria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Desativar, "UsuarioSecretaria " + UsuarioSecretaria.Id.ToString());
            return RedirectToAction("Details", "Usuarios", new { id = UsuarioSecretaria.UsuarioId, retorno = "Registro bloqueada com sucesso!" });
        }

        public ActionResult UnBlockUsuarioSecretaria(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utils.Utils.VerificaPermissoesUsuario(currentCodArea, User.Identity.GetUserId(), false, false, true, false);
            if (HttpContext.Response.IsRequestBeingRedirected) { return View(); }
            var UsuarioSecretaria = db.UsuarioSecretaria.Find(id);
            UsuarioSecretaria.Status = (int)StatusPadrao.Ativo;
            db.Entry(UsuarioSecretaria).State = EntityState.Modified;
            db.SaveChanges();
            Logs.salvarLog(User.Identity.GetUserId<int>(), currentCodArea, TipoAcao.Ativar, "UsuarioSecretaria " + UsuarioSecretaria.Id.ToString());
            return RedirectToAction("Details", "Usuarios", new { id = UsuarioSecretaria.UsuarioId, retorno = "Registro desbloqueado com sucesso!" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}