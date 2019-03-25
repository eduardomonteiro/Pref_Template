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
    /// API Referente a "Concursos"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class ConcursoController : ApiController
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
        [Route("Api/Concursos")]
        [ResponseType(typeof(List<ConcursoVm>))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20, string Palavra = "", string DataInicio = "", string DataFim = "", int StatusId = 0)
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

                List<Concurso> ConcursoList = db.Concurso.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                             (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra))) &&
                                                             (StatusId == 0 || x.StatusPublicacaoId == StatusId) &&
                                                             (string.IsNullOrEmpty(DataInicio) || DbFunctions.TruncateTime(x.DataInicio) >= DbFunctions.TruncateTime(DateInicio)) &&
                                                             (string.IsNullOrEmpty(DataFim) || DbFunctions.TruncateTime(x.DataInicio) <= DbFunctions.TruncateTime(DateFim)))
                                                       .Select(c => new
                                                       {
                                                           c,
                                                           ConcursoArquivo = c.ConcursoArquivo.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                           ConcursoModalidade = c.ConcursoModalidade,
                                                           StatusPublicacao = c.StatusPublicacao
                                                       })
                                                       .ToList().Select(p => p.c).ToList();

                List<ConcursoVm> Retorno = Mapper.Map<List<Concurso>, List<ConcursoVm>>(ConcursoList.ToPagedList(PageNumber, PageSize).ToList());

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a Concurso conforme o slug
        /// </summary>
        /// <param name="Slug">Slug da Concurso</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Concurso")]
        [ResponseType(typeof(ConcursoVm))]
        public IHttpActionResult Get(string Slug = "")
        {
            using (var db = new ApplicationDbContext())
            {
                Concurso Concurso = db.Concurso.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                        x.Slug == Slug)
                                                .Select(c => new
                                                {
                                                    c,
                                                    ConcursoArquivo = c.ConcursoArquivo.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                    ConcursoModalidade = c.ConcursoModalidade,
                                                    StatusPublicacao = c.StatusPublicacao
                                                })
                                                .ToList().Select(p => p.c).FirstOrDefault();

                if (Concurso == null || Concurso.Id == 0)
                {
                    return BadRequest("Concurso não encontrado");
                }

                ConcursoVm Retorno = new ConcursoVm();

                Mapper.Map(Concurso, Retorno);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/StatusConcursos")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetStatus()
        {
            using (var db = new ApplicationDbContext())
            {
                List<StatusPublicacao> StatusPublicacaoList = db.StatusPublicacao
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<StatusPublicacao>, List<GenericVm>>(StatusPublicacaoList);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Modalidades
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/ModalidadesConcursos")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<ConcursoModalidade> ConcursoModalidadeList = db.ConcursoModalidade
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<ConcursoModalidade>, List<GenericVm>>(ConcursoModalidadeList);

                return Ok(Retorno);
            }
        }
    }
}