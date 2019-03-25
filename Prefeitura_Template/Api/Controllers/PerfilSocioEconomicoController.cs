using Prefeitura_Template.Api.ViewModels;
using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data;
using System.Data.Entity;
using AutoMapper;
using WebApi.OutputCache.V2;

namespace Prefeitura_Template.Api.Controllers
{
    /// <summary>
    /// API Referente a "Perfil Socio-Economico"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class PerfilSocioEconomicoController : ApiController
    {
        /// <summary>
        /// Retorna o HTML da página e a listagem de items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/PerfilSocioEconomico")]
        [ResponseType(typeof(List<PerfilSocioEconomicoVm>))]
        public IHttpActionResult Get()
        {
            using (var db = new ApplicationDbContext())
            {
                List<PerfilSocioEconomico> PerfilSocioEconomicoList = db.PerfilSocioEconomico
                                                                                        .Include(x => x.PerfilSocioEconomicoCategoria)
                                                                                        .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                                        .ToList();

                List<PerfilSocioEconomicoVm> Retorno = Mapper.Map<List<PerfilSocioEconomico>, List<PerfilSocioEconomicoVm>>(PerfilSocioEconomicoList.ToList());

                return Ok(Retorno);

            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasPerfilSocioEconomico")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<PerfilSocioEconomicoCategoria> PerfilSocioEconomicoCategoriaList = db.PerfilSocioEconomicoCategoria
                                                                                                        .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                                                        .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<PerfilSocioEconomicoCategoria>, List<GenericVm>>(PerfilSocioEconomicoCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}