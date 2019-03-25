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
    /// API Referente a "Licitações"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class LicitacaoController : ApiController
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
        [Route("Api/Licitacoes")]
        [ResponseType(typeof(List<LicitacaoVm>))]
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

                List<Licitacao> LicitacaoList = db.Licitacao.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                                     (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra))) &&
                                                                     (StatusId == 0 || x.StatusPublicacaoId == StatusId) &&
                                                                     (string.IsNullOrEmpty(DataInicio) || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(DateInicio)) &&
                                                                     (string.IsNullOrEmpty(DataFim) || DbFunctions.TruncateTime(x.DataPublicacao) <= DbFunctions.TruncateTime(DateFim)))
                                                               .Select(c => new
                                                               {
                                                                   c,
                                                                   LicitacaoArquivo = c.LicitacaoArquivo.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                                   LicitacaoModalidade = c.LicitacaoModalidade,
                                                                   StatusPublicacao = c.StatusPublicacao
                                                               })
                                                               .ToList().Select(p => p.c).ToList();

                List<LicitacaoVm> Retorno = Mapper.Map<List<Licitacao>, List<LicitacaoVm>>(LicitacaoList.ToPagedList(PageNumber, PageSize).ToList());

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a Licitacao conforme o slug
        /// </summary>
        /// <param name="Slug">Slug da Licitacao</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Licitacao")]
        [ResponseType(typeof(LicitacaoVm))]
        public IHttpActionResult Get(string Slug = "")
        {
            using (var db = new ApplicationDbContext())
            {
                Licitacao Licitacao = db.Licitacao.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                            x.Slug == Slug)
                                                    .Select(c => new
                                                    {
                                                        c,
                                                        LicitacaoArquivo = c.LicitacaoArquivo.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                        LicitacaoModalidade = c.LicitacaoModalidade,
                                                        StatusPublicacao = c.StatusPublicacao
                                                    })
                                                    .ToList().Select(p => p.c).FirstOrDefault();

                if (Licitacao == null || Licitacao.Id == 0)
                {
                    return BadRequest("Licitação não encontrado");
                }

                LicitacaoVm Retorno = new LicitacaoVm();

                Mapper.Map(Licitacao, Retorno);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/StatusLicitacoes")]
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
        [Route("Api/ModalidadesLicitacoes")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<LicitacaoModalidade> LicitacaoModalidadeList = db.LicitacaoModalidade
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<LicitacaoModalidade>, List<GenericVm>>(LicitacaoModalidadeList);

                return Ok(Retorno);
            }
        }
    }
}