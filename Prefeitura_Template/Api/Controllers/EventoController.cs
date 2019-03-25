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
    /// API Referente a "Eventos"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class EventoController : ApiController
    {
        /// <summary>
        /// Retorna a listagem de items
        /// </summary>
        /// <param name="PageNumber">Número da página</param>
        /// <param name="PageSize">Quantidade de itens na página</param>
        /// <param name="Palavra">Palavra Filtro</param>
        /// <param name="Data">Data Filtro (1-Ultima Semana, 2-Ultimo Mes, 3-Ultimo Ano)</param>
        /// <param name="CategoriaId">Id da Categoria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Eventos")]
        [ResponseType(typeof(List<EventoVinculadoVm>))]
        public IHttpActionResult Get(int PageNumber = 1 , int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            using (var db = new ApplicationDbContext())
            {
                DateTime Date = new DateTime();

                if (Data == 1)
                {
                    Date = DateTime.Now.AddDays(-7);
                }
                else if (Data == 2)
                {
                    Date = DateTime.Now.AddMonths(-1);
                }
                else if (Data == 3)
                {
                    Date = DateTime.Now.AddYears(-1);
                }

                List<Evento> EventosList = db.Evento.Include(x => x.EventoCategoria)
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                              x.DataHorarioEvento >= DateTime.Now &&
                                                             (string.IsNullOrEmpty(Palavra) || 
                                                              (x.Titulo.Contains(Palavra) ||
                                                              x.SubTitulo.Contains(Palavra))) &&
                                                              (CategoriaId == 0 || x.EventoCategoriaId == CategoriaId) &&
                                                              (Data == 0 || DbFunctions.TruncateTime(x.DataHorarioEvento) >= DbFunctions.TruncateTime(Date)))
                                                       .ToList();

                List<EventoVinculadoVm> Retorno = Mapper.Map<List<Evento>, List<EventoVinculadoVm>>(EventosList.ToPagedList(PageNumber, PageSize).ToList());

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna o Evento conforme o slug
        /// </summary>
        /// <param name="Slug">Slug do Servico</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Evento")]
        [ResponseType(typeof(EventoVm))]
        public IHttpActionResult Get(string Slug = "")
        {
            using (var db = new ApplicationDbContext())
            {
                Evento Evento = db.Evento.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                x.Slug == Slug)
                                        .Select(c => new
                                        {
                                            c,
                                            EventoGaleria = c.EventoGaleria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                            EventoCategoria = c.EventoCategoria
                                        })
                                        .ToList().Select(p => p.c).FirstOrDefault();

                if (Evento == null || Evento.Id == 0)
                {
                    return BadRequest("Evento não encontrado");
                }

                EventoVm Retorno = new EventoVm();

                Mapper.Map(Evento, Retorno);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasEventos")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<EventoCategoria> EventoCategoriaList = db.EventoCategoria
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<EventoCategoria>, List<GenericVm>>(EventoCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}