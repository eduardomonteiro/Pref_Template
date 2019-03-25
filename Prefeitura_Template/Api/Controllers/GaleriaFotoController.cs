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
    /// API Referente a "Galeria de Fotos"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class GaleiraFotoController : ApiController
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
        [Route("Api/GaleriaFotos")]
        [ResponseType(typeof(GaleriaFotoVm))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            GaleriaFotoVm Retorno = new GaleriaFotoVm();
            Retorno.GaleriaFotoDestaque = Destaques(Palavra, Data, CategoriaId);
            Retorno.GaleriaFoto = NaoDestaques(PageNumber, PageSize, Palavra, Data, CategoriaId);

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
        [Route("Api/GaleriaFotosDestaques")]
        [ResponseType(typeof(List<GaleriaFotoListaVm>))]
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
        [Route("Api/GaleriaFotosNaoDestaques")]
        [ResponseType(typeof(List<GaleriaFotoListaVm>))]
        public IHttpActionResult GetNaoDestaques(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            return Ok(NaoDestaques(PageNumber, PageSize, Palavra, Data, CategoriaId));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<GaleriaFotoListaVm> Destaques(string Palavra = "", int Data = 0, int CategoriaId = 0)
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

                List<GaleriaFoto> GaleriaFotoList = db.GaleriaFoto.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                              x.Destaque && 
                                                              (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra) ||
                                                                                                 x.Fotografo.Contains(Palavra))) &&
                                                              (CategoriaId == 0 || x.GaleriaFotoCategoriaId == CategoriaId) &&
                                                              (Data == 0 || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(Date)))
                                                              .Select(c => new
                                                              {
                                                                  c,
                                                                  GaleriaFotoGaleria = c.GaleriaFotoGaleria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                                  GaleriaFotoCategoria = c.GaleriaFotoCategoria
                                                              })
                                                              .ToList().Select(p => p.c).ToList();

                return Mapper.Map<List<GaleriaFoto>, List<GaleriaFotoListaVm>>(GaleriaFotoList.ToList());
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<GaleriaFotoListaVm> NaoDestaques(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
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

                List<GaleriaFoto> GaleriaFotoList = db.GaleriaFoto.Where(x => x.Status == (int)StatusPadrao.Ativo  && 
                                                                         !x.Destaque &&
                                                                         (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra) ||
                                                                                                             x.Fotografo.Contains(Palavra))) &&
                                                                          (CategoriaId == 0 || x.GaleriaFotoCategoriaId == CategoriaId) &&
                                                                          (Data == 0 || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(Date)))
                                                                   .Select(c => new
                                                                   {
                                                                       c,
                                                                       GaleriaFotoGaleria = c.GaleriaFotoGaleria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                                       GaleriaFotoCategoria = c.GaleriaFotoCategoria
                                                                   })
                                                                   .ToList().Select(p => p.c).ToList();

                return Mapper.Map<List<GaleriaFoto>, List<GaleriaFotoListaVm>>(GaleriaFotoList.ToPagedList(PageNumber, PageSize).ToList());
            }
        }

        /// <summary>
        /// Retorna a Galeria conforme o slug
        /// </summary>
        /// <param name="Slug">Slug da Galeria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/GaleriaFoto")]
        [ResponseType(typeof(GaleriaFotoListaVm))]
        public IHttpActionResult Get(string Slug = "")
        {
            using (var db = new ApplicationDbContext())
            {
                GaleriaFoto GaleriaFoto = db.GaleriaFoto.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                              x.Slug == Slug)
                                                       .Select(c => new
                                                       {
                                                           c,
                                                           GaleriaFotoGaleria = c.GaleriaFotoGaleria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                           GaleriaFotoCategoria = c.GaleriaFotoCategoria
                                                       })
                                                       .ToList().Select(p => p.c).FirstOrDefault();

                if (GaleriaFoto == null || GaleriaFoto.Id == 0)
                {
                    return BadRequest("Galeria de Foto não encontrada");
                }

                GaleriaFotoListaVm Retorno = new GaleriaFotoListaVm();

                Mapper.Map(GaleriaFoto, Retorno);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasGaleriaFotos")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<GaleriaFotoCategoria> GaleriaFotoCategoriaList = db.GaleriaFotoCategoria
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<GaleriaFotoCategoria>, List<GenericVm>>(GaleriaFotoCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}