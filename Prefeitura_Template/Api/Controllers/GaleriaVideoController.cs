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
    /// API Referente a "Galeria de Videos"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class GaleriaVideoController : ApiController
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
        [Route("Api/GaleriaVideos")]
        [ResponseType(typeof(GaleriaVideoVm))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            GaleriaVideoVm Retorno = new GaleriaVideoVm();
            Retorno.GaleriaVideoDestaque = Destaques(Palavra, Data, CategoriaId);
            Retorno.GaleriaVideo = NaoDestaques(PageNumber, PageSize, Palavra, Data, CategoriaId);

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
        [Route("Api/GaleriaVideosDestaques")]
        [ResponseType(typeof(List<GaleriaVideoListaVm>))]
        public IHttpActionResult GetDestaques(string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            return Ok(Destaques(Palavra, Data, CategoriaId));
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
        [Route("Api/GaleriaVideosNaoDestaques")]
        [ResponseType(typeof(List<GaleriaVideoListaVm>))]
        public IHttpActionResult GetNaoDestaques(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            return Ok(NaoDestaques(PageNumber, PageSize, Palavra, Data, CategoriaId));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<GaleriaVideoListaVm> Destaques(string Palavra = "", int Data = 0, int CategoriaId = 0)
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

                List<GaleriaVideo> GaleriaVideoList = db.GaleriaVideo.Include(x => x.GaleriaVideoCategoria)
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                              x.Destaque &&
                                                              (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra))) &&
                                                              (CategoriaId == 0 || x.GaleriaVideoCategoriaId == CategoriaId) &&
                                                              (Data == 0 || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(Date)))
                                                       .ToList();

                return Mapper.Map<List<GaleriaVideo>, List<GaleriaVideoListaVm>>(GaleriaVideoList.ToList());
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<GaleriaVideoListaVm> NaoDestaques(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
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

                List<GaleriaVideo> GaleriaVideoList = db.GaleriaVideo.Include(x => x.GaleriaVideoCategoria)
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                             !x.Destaque &&
                                                             (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra))) &&
                                                              (CategoriaId == 0 || x.GaleriaVideoCategoriaId == CategoriaId) &&
                                                              (Data == 0 || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(Date)))
                                                       .ToList();

                return Mapper.Map<List<GaleriaVideo>, List<GaleriaVideoListaVm>>(GaleriaVideoList.ToPagedList(PageNumber, PageSize).ToList());
            }
        }

        /// <summary>
        /// Retorna a Galeria de Video conforme o slug
        /// </summary>
        /// <param name="Slug">Slug da Galeria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/GaleriaVideo")]
        [ResponseType(typeof(GaleriaVideoListaVm))]
        public IHttpActionResult Get(string Slug = "")
        {
            using (var db = new ApplicationDbContext())
            {
                GaleriaVideo GaleriaVideo = db.GaleriaVideo.Include(x => x.GaleriaVideoCategoria)
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                              x.Slug == Slug)
                                                       .FirstOrDefault();

                if (GaleriaVideo == null || GaleriaVideo.Id == 0)
                {
                    return BadRequest("Galeria de Video não encontrada");
                }

                GaleriaVideoListaVm Retorno = new GaleriaVideoListaVm();

                Mapper.Map(GaleriaVideo, Retorno);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasGaleriaVideos")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<GaleriaVideoCategoria>GaleriaVideoCategoriaList = db.GaleriaVideoCategoria
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<GaleriaVideoCategoria>, List<GenericVm>>(GaleriaVideoCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}