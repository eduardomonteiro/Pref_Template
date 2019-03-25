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
    /// API Referente a "Legislação"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class LegislacaoController : ApiController
    {
        /// <summary>
        /// Retorna a listagem de items
        /// </summary>
        /// <param name="PageNumber">Número da página</param>
        /// <param name="PageSize">Quantidade de itens na página</param>
        /// <param name="Palavra">Palavra Filtro</param>
        /// <param name="DataInicio">Data Inicial</param>
        /// <param name="DataFim">Data Final</param>
        /// <param name="StatusId">Id da Categoria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Legislacoes")]
        [ResponseType(typeof(List<LegislacaoVm>))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20, string Palavra = "", string DataInicio = "", string DataFim = "", int CategoriaId = 0)
        {
            using (var db = new ApplicationDbContext())
            {
                DateTime DateInicio = new DateTime();
                if (!DateTime.TryParse(DataInicio, out DateInicio))
                {
                    DataInicio = "";
                }

                DateTime DateFim = new DateTime();
                if (!DateTime.TryParse(DataFim, out DateFim))
                {
                    DataFim = "";
                }

                List<Legislacao> LegislacaoList = db.Legislacao.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                                     (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra))) &&
                                                                     (CategoriaId == 0 || x.LegislacaoCategoriaId == CategoriaId) &&
                                                                     (string.IsNullOrEmpty(DataInicio) || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(DateInicio)) &&
                                                                     (string.IsNullOrEmpty(DataFim) || DbFunctions.TruncateTime(x.DataPublicacao) <= DbFunctions.TruncateTime(DateFim)))
                                                               .Select(c => new
                                                               {
                                                                   c,
                                                                   LegislacaoArquivo = c.LegislacaoArquivo.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                                   LegislacaoCategoria = c.LegislacaoCategoria
                                                               })
                                                               .ToList().Select(p => p.c).ToList();

                List<LegislacaoVm> Retorno = Mapper.Map<List<Legislacao>, List<LegislacaoVm>>(LegislacaoList.ToPagedList(PageNumber, PageSize).ToList());

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasLegislacao")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<LegislacaoCategoria> LegislacaoCategoriaList = db.LegislacaoCategoria
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<LegislacaoCategoria>, List<GenericVm>>(LegislacaoCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}