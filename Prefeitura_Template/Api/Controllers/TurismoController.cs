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
    /// API Referente a "Pontos Turisticos"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class TurismoController : ApiController
    {
        /// <summary>
        /// Retorna o HTML da página e a listagem de items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Turismo")]
        [ResponseType(typeof(TurismoVm))]
        public IHttpActionResult Get()
        {
            using (var db = new ApplicationDbContext())
            {
                Cidade Cidade = db.Cidade.FirstOrDefault();
                List<Turismo> TurismoList = db.Turismo.Where(x => x.Status == (int)StatusPadrao.Ativo).ToList();

                TurismoVm Retorno = new TurismoVm
                {
                    Turismo = Cidade.Turismo,
                    ListaTurismo = Mapper.Map<List<Turismo>, List<TurismoListaVm>>(TurismoList)
                };

                return Ok(Retorno);

            }
        }

    }
}