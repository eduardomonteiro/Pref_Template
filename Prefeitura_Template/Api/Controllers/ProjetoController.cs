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
    /// API Referente a "Projetos"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class ProjetoController : ApiController
    {
        /// <summary>
        /// Retorna a listagem de items
        /// </summary>
        /// <param name="PageNumber">Número da página</param>
        /// <param name="PageSize">Quantidade de itens na página</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Projetos")]
        [ResponseType(typeof(List<ProjetoVinculadoVm>))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20)
        {
            using (var db = new ApplicationDbContext())
            {
                List<Projeto> ProjetosList = db.Projeto.Include(x => x.ProjetoCategoria).Include(x => x.Secretaria)
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                       .ToList();

                List<ProjetoVinculadoVm> Retorno = Mapper.Map<List<Projeto>, List<ProjetoVinculadoVm>>(ProjetosList.ToPagedList(PageNumber, PageSize).ToList());

                return Ok(Retorno);

            }
        }

        /// <summary>
        /// Retorna o Projeto conforme o slug
        /// </summary>
        /// <param name="Slug">Slug do Projeto</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Projeto")]
        [ResponseType(typeof(ProjetoVm))]
        public IHttpActionResult Get(string Slug = "")
        {
            using (var db = new ApplicationDbContext())
            {
                Projeto Projeto = db.Projeto.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                    x.Slug == Slug)
                                            .Select(c => new
                                            {
                                                c,
                                                ProjetoArquivo = c.ProjetoArquivo.Where(y => y.Status == (int)StatusPadrao.Ativo),
                                                ProjetoCategoria = c.ProjetoCategoria,
                                                Secretaria = c.Secretaria
                                            })
                                            .ToList().Select(p => p.c).FirstOrDefault();

                if (Projeto == null || Projeto.Id == 0)
                {
                    return BadRequest("Projeto não encontrado");
                }

                ProjetoVm Retorno = new ProjetoVm();

                Mapper.Map(Projeto, Retorno);

                List<string> ProjetoTags = Projeto.Tag.Select(x => x.Slug).ToList();

                List<Evento> Eventos = db.Evento.Include(x => x.EventoCategoria)
                                                .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                .ToList();
                List<int> EventosIds = Eventos.Select(x => x.Id).ToList();
                List<Tag> EventosTags = db.Tag.Where(x => x.AreaId == 7 &&
                                                     EventosIds.Contains(x.RegistroId) &&
                                                     ProjetoTags.Contains(x.Slug)
                                                     )
                                                     .ToList();
                List<int> EventosTagsIds = EventosTags.Select(y => y.RegistroId).ToList();
                Eventos = Eventos.Where(x => EventosTagsIds.Contains(x.Id)).Take(4).ToList();


                List<Noticia> Noticias = db.Noticia.Include(x => x.NoticiaCategoria)
                                                   .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                   .OrderByDescending(x => x.Destaque)
                                                   .ToList();
                List<int> NoticiasIds = Noticias.Select(x => x.Id).ToList();
                List<Tag> NoticiasTags = db.Tag.Where(x => x.AreaId == 8 &&
                                                      NoticiasIds.Contains(x.RegistroId) &&
                                                      ProjetoTags.Contains(x.Slug)
                                                      )
                                                      .ToList();
                List<int> NoticiasTagsIds = NoticiasTags.Select(y => y.RegistroId).ToList();
                Noticias = Noticias.Where(x => NoticiasTagsIds.Contains(x.Id)).Take(3).ToList();

                Retorno.Eventos = Mapper.Map<List<Evento>, List<EventoVinculadoVm>>(Eventos);

                Retorno.Noticias = Mapper.Map<List<Noticia>, List<NoticiaVinculadaVm>>(Noticias);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasProjetos")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<ProjetoCategoria> ProjetosCategoriaList = db.ProjetoCategoria
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<ProjetoCategoria>, List<GenericVm>>(ProjetosCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}