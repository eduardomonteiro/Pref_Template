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
using Prefeitura_Template.Areas.Admin.Utils;
using System.Globalization;
using System.Text.RegularExpressions;
using WebApi.OutputCache.V2;

namespace Prefeitura_Template.Api.Controllers
{
    /// <summary>
    /// API Referente a "Noticia"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class NoticiaController : ApiController
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
        [Route("Api/Noticias")]
        [ResponseType(typeof(NoticiaVm))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            NoticiaVm Retorno = new NoticiaVm();
            Retorno.NoticiaDestaque = Destaques(Palavra, Data , CategoriaId);
            Retorno.Noticia = NaoDestaques(PageNumber, PageSize,Palavra, Data , CategoriaId);

            return Ok(Retorno);
        }

        /// <summary>
        /// Retorna a listagem de items de destaque
        /// </summary>
        /// <param name="Palavra">Palavra Filtro</param>
        /// <param name="Data">Data Filtro (1-Ultima Semana, 2-Ultimo Mes, 3-Ultimo Ano)</param>
        /// <param name="CategoriaId">Id da Categoria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/NoticiaDestaques")]
        [ResponseType(typeof(List<NoticiaListaVm>))]
        public IHttpActionResult GetDestaques(string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            return Ok(Destaques(Palavra, Data , CategoriaId));
        }

        /// <summary>
        /// Retorna a listagem de items nao destaques
        /// </summary>
        /// <param name="PageNumber">Número da página</param>
        /// <param name="PageSize">Quantidade de itens na página</param>
        /// <param name="Palavra">Palavra Filtro</param>
        /// <param name="Data">Data Filtro (1-Ultima Semana, 2-Ultimo Mes, 3-Ultimo Ano)</param>
        /// <param name="CategoriaId">Id da Categoria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/NoticiaNaoDestaques")]
        [ResponseType(typeof(List<NoticiaListaVm>))]
        public IHttpActionResult GetNaoDestaques(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            return Ok(NaoDestaques(PageNumber, PageSize, Palavra, Data, CategoriaId));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<NoticiaListaVm> Destaques(string Palavra = "", int Data = 0, int CategoriaId = 0)
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

                List<Noticia> NoticiaList = db.Noticia.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                              x.Destaque &&
                                                              x.DataPublicacao <= DateTime.Now &&
                                                              (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra) || 
                                                                                                 x.SubTitulo.Contains(Palavra))) &&
                                                              (CategoriaId == 0 || x.NoticiaCategoriaId == CategoriaId) &&
                                                              (Data == 0 || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(Date)))
                                                       .Select(c => new
                                                       {
                                                           c,
                                                           NoticiaGaleria = c.NoticiaGaleria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                           NoticiaCategoria = c.NoticiaCategoria
                                                       })
                                                       .ToList().Select(p => p.c).ToList();

                return Mapper.Map<List<Noticia>, List<NoticiaListaVm>>(NoticiaList.ToList());
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<NoticiaListaVm> NaoDestaques(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
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

                List<Noticia> NoticiaList = db.Noticia.Include(x => x.NoticiaCategoria)
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                             !x.Destaque &&
                                                             x.DataPublicacao <= DateTime.Now &&
                                                             (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra) ||
                                                                                                 x.SubTitulo.Contains(Palavra))) &&
                                                              (CategoriaId == 0 || x.NoticiaCategoriaId == CategoriaId) &&
                                                              (Data == 0 || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(Date)))
                                                       .Select(c => new
                                                       {
                                                           c,
                                                           NoticiaGaleria = c.NoticiaGaleria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                           NoticiaCategoria = c.NoticiaCategoria
                                                       })
                                                       .ToList().Select(p => p.c).ToList();

                return Mapper.Map<List<Noticia>, List<NoticiaListaVm>>(NoticiaList.ToPagedList(PageNumber, PageSize).ToList());
            }
        }

        /// <summary>
        /// Retorna a Noticia conforme o slug
        /// </summary>
        /// <param name="Slug">Slug da Noticia</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Noticia")]
        [ResponseType(typeof(NoticiaListaVm))]
        public IHttpActionResult Get(string Slug = "")
        {
            using (var db = new ApplicationDbContext())
            {
                Noticia Noticia = db.Noticia.Include(x => x.NoticiaCategoria)
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                              x.Slug == Slug)
                                                       .Select(c => new
                                                       {
                                                           c,
                                                           NoticiaGaleria = c.NoticiaGaleria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                           NoticiaCategoria = c.NoticiaCategoria
                                                       })
                                                       .ToList().Select(p => p.c).FirstOrDefault();

                if (Noticia == null || Noticia.Id == 0)
                {
                    return BadRequest("Noticia não encontrada");
                }

                NoticiaListaVm Retorno = new NoticiaListaVm();

                Mapper.Map(Noticia, Retorno);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasNoticias")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<NoticiaCategoria> NoticiaCategoriaList = db.NoticiaCategoria
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<NoticiaCategoria>, List<GenericVm>>(NoticiaCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}