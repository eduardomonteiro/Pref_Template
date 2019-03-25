using AutoMapper;
using PagedList;
using Prefeitura_Template.Api.ViewModels;
using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.OutputCache.V2;

namespace Prefeitura_Template.Api.Controllers
{
    /// <summary>
    /// API Referente a "Enquetes"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class NewsLetterController : ApiController
    {
        /// <summary>
        /// Responder Enquete
        /// </summary>
        /// <param name="News">Objeto da News</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Api/CadastrarNewsLetter")]
        [ResponseType(typeof(string))]
        public IHttpActionResult Cadastrar(NewsLetterVm News)
        {
            if (News == null)
            {
                return BadRequest("News Obrigatório");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var db = new ApplicationDbContext())
            {
                NewsLetter Model = new NewsLetter();

                Mapper.Map(News, Model);

                NewsLetter RegistroExistente = db.NewsLetter.Where(x => x.Email == News.Email && x.Status == (int)StatusPadrao.Ativo).FirstOrDefault();

                if (RegistroExistente != null)
                {
                    return BadRequest("E-mail já cadastrado anteriormente");
                }

                Model.DataCadastro = DateTime.Now;
                Model.Status = (int)StatusPadrao.Ativo;

                db.Entry(Model).State = EntityState.Added;
                db.SaveChanges();
                return Ok("News cadastrada com sucesso");
            }
        }
    }
}