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
    /// API Referente a "A Cidade"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class CidadeController : ApiController
    {
        /// <summary>
        /// Informações da Cidade
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Cidade")]
        [ResponseType(typeof(CidadeVm))]
        public IHttpActionResult Get()
        {
            using (var db = new ApplicationDbContext())
            {
                Cidade Cidade = db.Cidade.FirstOrDefault();
                CidadeVm Retorno = new CidadeVm();
                Mapper.Map(Cidade, Retorno);

                List<Timeline> TimelineList = db.Timeline.Where(x => x.Status == (int)StatusPadrao.Ativo).OrderBy(x => x.Ordem).ToList();

                Retorno.Timeline = Mapper.Map<List<Timeline>, List<TimelineVm>>(TimelineList.ToList());

                return Ok(Retorno);
            }
        }
    }
}