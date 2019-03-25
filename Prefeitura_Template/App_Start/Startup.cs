using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Prefeitura_Template.Models;
using Prefeitura_Template;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData.Extensions;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Prefeitura_Template.General;
using Prefeitura_Template.Areas.HelpPage;
using Microsoft.Practices.Unity.Mvc;
using Unity.Mvc5;

[assembly: OwinStartupAttribute(typeof(Startup))]
namespace Prefeitura_Template
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            Register(config);
            ConfigureAuth(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public static void Register(HttpConfiguration config)
        {
            config.AddODataQueryFilter();

            var container = new UnityContainer();

            config.DependencyResolver = new UnityResolver(container);

            // Web API configuration and services            
            config.Filters.Add(new InvalidOperationExceptionFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.SetDocumentationProvider(new XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/bin/Api.xml")));
            container.RegisterType<Prefeitura_Template.Areas.Admin.Controllers.UsuariosController>(new InjectionConstructor());

            config.Routes.MapHttpRoute("DefaultApiWithId", "Api/{controller}/{id}", new { id = RouteParameter.Optional }, new { id = @"\d+" });
            config.Routes.MapHttpRoute("DefaultApiWithActionWithOutParam", "Api/{controller}/{action}", new { param = RouteParameter.Optional });
            config.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}/{param}", new { param = RouteParameter.Optional });
            config.Routes.MapHttpRoute("DefaultApiWithActionUnderline   ", "Api/{controller}_{action}");
            config.Routes.MapHttpRoute("DefaultApiGet", "Api/{controller}", new { action = "Get" }, new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute("DefaultApiPost", "Api/{controller}", new { action = "Post" }, new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Post) });

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Admin/Usuarios/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator
                .OnValidateIdentity<ApplicationUserManager, ApplicationUser, int>(
                    validateInterval: TimeSpan.FromMinutes(30),
                    regenerateIdentityCallback: (manager, user) =>
                        user.GenerateUserIdentityAsync(manager),
                    getUserIdCallback: (id) => (id.GetUserId<int>()))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}