using Prefeitura_Template.Api.ViewModels;
using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using AutoMapper;
using System;
using PagedList;
using WebApi.OutputCache.V2;

namespace Prefeitura_Template.Api.Controllers
{
    /// <summary>
    /// API Referente a "Bairros"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class BairroController : ApiController
    {
        /// <summary>
        /// Retorna a listagem de Bairros
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Bairros")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<Bairro> BairroList = db.Bairro.Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                    .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<Bairro>, List<GenericVm>>(BairroList);

                return Ok(Retorno);
            }
        }
    }
}