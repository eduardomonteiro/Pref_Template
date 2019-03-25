using AutoMapper;
using PagedList;
using Prefeitura_Template.Api.ViewModels;
using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.OutputCache.V2;

namespace Prefeitura_Template.Api.Controllers
{
    /// <summary>
    /// API Referente a "Enquetes"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class EnqueteController : ApiController
    {
        /// <summary>
        /// Retorna a listagem de items
        /// </summary>
        /// <param name="PageNumber">Número da página</param>
        /// <param name="PageSize">Quantidade de itens na página</param>
        /// <param name="Palavra">Palavra Filtro</param>
        /// <param name="DataInicio">Data Inicial</param>
        /// <param name="DataFim">Data Final</param>
        /// <param name="Status">Status (1-Ativos, 2-Encerrados)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Enquetes")]
        [ResponseType(typeof(List<EnqueteVm>))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20, string Palavra = "", string DataInicio = "", string DataFim = "", int Status = 0)
        {
            using (var db = new ApplicationDbContext())
            {
                DateTime DateInicio = new DateTime();
                if(!DateTime.TryParse(DataInicio, out DateInicio))
                {
                    DataInicio = "";
                }

                DateTime DateFim = new DateTime();
                if (!DateTime.TryParse(DataFim, out DateFim))
                {
                    DataFim = "";
                }

                List<Enquete> EnqueteList = db.Enquete.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                             x.DataInicial < DateTime.Now &&
                                                             (string.IsNullOrEmpty(Palavra) || (x.Pergunta.Contains(Palavra))) &&
                                                              //((Status == 0) || (Status == 1 && x.Encerrado == false) || (Status == 2 && x.Encerrado == true)) &&
                                                              (string.IsNullOrEmpty(DataInicio) || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(DateInicio)) &&
                                                              (string.IsNullOrEmpty(DataFim) || DbFunctions.TruncateTime(x.DataPublicacao) <= DbFunctions.TruncateTime(DateFim)))
                                                        .Select(c => new
                                                        {
                                                            c,
                                                            EnqueteOpcao = c.EnqueteOpcao.Where(x => x.Status == (int)StatusPadrao.Ativo).OrderBy(x => x.Ordem)
                                                        })
                                                       .ToList().Select(p => p.c).ToList();

                if(Status == 1)
                {
                    EnqueteList = EnqueteList.Where(x => x.Encerrado == false).ToList();
                }
                else if(Status==2)
                {
                    EnqueteList = EnqueteList.Where(x => x.Encerrado == true).ToList();
                }

                List<EnqueteVm> Retorno = Mapper.Map<List<Enquete>, List<EnqueteVm>>(EnqueteList.ToPagedList(PageNumber, PageSize).ToList());

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Responder Enquete
        /// </summary>
        /// <param name="EnqueteOpcaoId">Id da opcao da enquete</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Api/RespoderEnquete")]
        [ResponseType(typeof(string))]
        public IHttpActionResult ResponderEnquete(int EnqueteOpcaoId = 0)
        {
            if(EnqueteOpcaoId == 0)
            {
                return BadRequest("EnqueteOpcaoId Obrigatório");
            }

            using (var db = new ApplicationDbContext())
            {
                EnqueteResposta Resposta = new EnqueteResposta();
                Resposta.EnqueteOpcaoId = EnqueteOpcaoId;
                Resposta.DataResposta = DateTime.Now;
                db.Entry(Resposta).State = EntityState.Added;
                db.SaveChanges();
                return Ok("Resposta cadastrada com sucesso");
            }
        }
    }
}