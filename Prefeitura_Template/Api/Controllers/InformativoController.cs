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
    /// API Referente a "Informativo"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class InformativoController : ApiController
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
        [Route("Api/Informativos")]
        [ResponseType(typeof(InformativoVm))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            InformativoVm Retorno = new InformativoVm();
            Retorno.InformativoDestaque = Destaques(Palavra, Data, CategoriaId);
            Retorno.Informativo = NaoDestaques(PageNumber, PageSize, Palavra, Data, CategoriaId);

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
        [Route("Api/InformativoDestaques")]
        [ResponseType(typeof(List<InformativoListaVm>))]
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
        [Route("Api/InformativoNaoDestaques")]
        [ResponseType(typeof(List<InformativoListaVm>))]
        public IHttpActionResult GetNaoDestaques(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
        {
            return Ok(NaoDestaques(PageNumber, PageSize, Palavra, Data, CategoriaId));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<InformativoListaVm> Destaques(string Palavra = "", int Data = 0, int CategoriaId = 0)
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

                List<Informativo> InformativoList = db.Informativo.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                                          x.Destaque &&
                                                                          (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra) ||
                                                                                                             x.SubTitulo.Contains(Palavra))) &&
                                                                          (CategoriaId == 0 || x.InformativoCategoriaId == CategoriaId) &&
                                                                          (Data == 0 || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(Date)))
                                                                   .Select(c => new
                                                                   {
                                                                       c,
                                                                       InformativoGaleria = c.InformativoGaleria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                                       InformativoCategoria = c.InformativoCategoria
                                                                   })
                                                                   .ToList().Select(p => p.c).ToList();

                return Mapper.Map<List<Informativo>, List<InformativoListaVm>>(InformativoList.ToList());
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<InformativoListaVm> NaoDestaques(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
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

                List<Informativo> InformativoList = db.Informativo.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                                         !x.Destaque &&
                                                                         (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra) ||
                                                                                                             x.SubTitulo.Contains(Palavra))) &&
                                                                          (CategoriaId == 0 || x.InformativoCategoriaId == CategoriaId) &&
                                                                          (Data == 0 || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(Date)))
                                                                   .Select(c => new
                                                                   {
                                                                       c,
                                                                       InformativoGaleria = c.InformativoGaleria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                                       InformativoCategoria = c.InformativoCategoria
                                                                   })
                                                                   .ToList().Select(p => p.c).ToList();

                return Mapper.Map<List<Informativo>, List<InformativoListaVm>>(InformativoList.ToPagedList(PageNumber, PageSize).ToList());
            }
        }

        /// <summary>
        /// Retorna o Informativo conforme o slug
        /// </summary>
        /// <param name="Slug">Slug do Informativo</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Informativo")]
        [ResponseType(typeof(InformativoListaVm))]
        public IHttpActionResult Get(string Slug = "")
        {
            using (var db = new ApplicationDbContext())
            {
                Informativo Informativo = db.Informativo.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                              x.Slug == Slug)
                                                       .Select(c => new
                                                       {
                                                           c,
                                                           InformativoGaleria = c.InformativoGaleria.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                           InformativoCategoria = c.InformativoCategoria
                                                       })
                                                       .ToList().Select(p => p.c).FirstOrDefault();

                if (Informativo == null || Informativo.Id == 0)
                {
                    return BadRequest("Informativo não encontrado");
                }

                InformativoListaVm Retorno = new InformativoListaVm();

                Mapper.Map(Informativo, Retorno);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasInformativos")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<InformativoCategoria> InformativoCategoriaList = db.InformativoCategoria
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<InformativoCategoria>, List<GenericVm>>(InformativoCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}