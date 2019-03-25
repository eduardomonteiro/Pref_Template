using AutoMapper;
using Prefeitura_Template.Api.ViewModels;
using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.OutputCache.V2;

namespace Prefeitura_Template.Api.Controllers
{
    /// <summary>
    /// API Referente a "Ex-Prefeitos"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class ExPrefeitoController : ApiController
    {
        /// <summary>
        /// Retorna o HTML da página e a listagem de items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/ExPrefeitos")]
        [ResponseType(typeof(ExPrefeitoVm))]
        public IHttpActionResult Get()
        {
            using (var db = new ApplicationDbContext())
            {
                Cidade Cidade = db.Cidade.FirstOrDefault();
                List<ExPrefeito> ExpPrefeitoList = db.ExPrefeito.Where(x => x.Status == (int)StatusPadrao.Ativo).ToList();

                ExPrefeitoVm Retorno = new ExPrefeitoVm
                {
                    ExPrefeito = Cidade.ExPrefeito,
                    ListaExPrefeitos = Mapper.Map<List<ExPrefeito>, List<ExPrefeitoListaVm>>(ExpPrefeitoList)
                };
                return Ok(Retorno);
            }
        }
    }
}