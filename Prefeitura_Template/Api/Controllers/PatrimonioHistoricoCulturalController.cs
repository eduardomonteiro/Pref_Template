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
    /// API Referente a "Patrimonio Historico Cultural"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class PatrimonioHistoricoCulturalController : ApiController
    {
        /// <summary>
        /// Retorna o HTML da página e a listagem de items
        /// </summary>
        /// <param name="CategoriaId">Id da Categoria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/PatrimonioHistoricoCultural")]
        [ResponseType(typeof(PatrimonioHistoricoCulturalVm))]
        public IHttpActionResult Get(int CategoriaId = 0)
        {
            using (var db = new ApplicationDbContext())
            {
                Cidade Cidade = db.Cidade.FirstOrDefault();
                List<PatrimonioHistoricoCultural> PatrimonioHistoricoCulturalList = db.PatrimonioHistoricoCultural
                                                                                        .Include(x => x.PatrimonioHistoricoCulturalCategoria)
                                                                                        .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                                                               (CategoriaId == 0 || x.PatrimonioHistoricoCulturalCategoriaId == CategoriaId))
                                                                                        .ToList();

                PatrimonioHistoricoCulturalVm Retorno = new PatrimonioHistoricoCulturalVm
                {
                    PatrimonioHistoricoCultural = Cidade.PatrimonioHistoricoCultural,
                    ListaPatrimonioHistoricoCultural = Mapper.Map<List<PatrimonioHistoricoCultural>, List<PatrimonioHistoricoCulturalListaVm>>(PatrimonioHistoricoCulturalList.ToList())
                };

                return Ok(Retorno);

            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasPatrimonio")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<PatrimonioHistoricoCulturalCategoria> PatrimonioHistoricoCulturalCategoriaList = db.PatrimonioHistoricoCulturalCategoria
                                                                                                        .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                                                        .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<PatrimonioHistoricoCulturalCategoria>, List<GenericVm>>(PatrimonioHistoricoCulturalCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}