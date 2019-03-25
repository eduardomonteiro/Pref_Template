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
    /// API Referente a "AaZ"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class AaZController : ApiController
    {
        /// <summary>
        /// Retorna a listagem de items
        /// </summary>
        /// <param name="PageNumber">Número da página</param>
        /// <param name="PageSize">Quantidade de itens na página</param>
        /// <param name="Palavra">Palavra Filtro</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AaZ")]
        [ResponseType(typeof(List<AaZVm>))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20, string Palavra = "")
        {
            using (var db = new ApplicationDbContext())
            {
                List<AaZ> AaZList = db.AaZ.Include(x => x.AaZCategoria)
                                          .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                    (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra))))
                                          .ToList();

                List<AaZVm> Retorno = Mapper.Map<List<AaZ>, List<AaZVm>>(AaZList.ToPagedList(PageNumber, PageSize).ToList());

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasAaZ")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<AaZCategoria> AaZCategoriaList = db.AaZCategoria
                                                        .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                        .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<AaZCategoria>, List<GenericVm>>(AaZCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}