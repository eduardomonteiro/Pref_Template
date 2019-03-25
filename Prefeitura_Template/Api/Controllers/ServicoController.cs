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
    /// API Referente a "Serviços"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class ServicoController : ApiController
    {
        /// <summary>
        /// Retorna a listagem de items
        /// </summary>
        /// <param name="PageNumber">Número da página</param>
        /// <param name="PageSize">Quantidade de itens na página</param>
        /// <param name="Palavra">Palavra Filtro</param>
        /// <param name="CategoriaId">Id da Categoria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Servicos")]
        [ResponseType(typeof(List<ServicoListaVm>))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20, string Palavra = "", int CategoriaId = 0)
        {
            using (var db = new ApplicationDbContext())
            {

                List<Servico> ServicosList = db.Servico.Include(x => x.ServicoCategoria)
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                             (string.IsNullOrEmpty(Palavra) || (x.Nome.Contains(Palavra))) &&
                                                             (CategoriaId == 0 || x.ServicoCategoriaId == CategoriaId))
                                                       .ToList();

                List<ServicoListaVm> Retorno = Mapper.Map<List<Servico>, List<ServicoListaVm>>(ServicosList.ToPagedList(PageNumber, PageSize).ToList());

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de items de destaque
        /// </summary>
        /// <param name="Palavra">Palavra Filtro</param>
        /// <param name="CategoriaId">Id da Categoria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/ServicoDestaques")]
        [ResponseType(typeof(List<ServicoListaVm>))]
        public IHttpActionResult GetDestaques(string Palavra = "", int CategoriaId = 0)
        {
            return Ok(Destaques(Palavra, CategoriaId));
        }

        /// <summary>
        /// Retorna a listagem de items nao destaques
        /// </summary>
        /// <param name="PageNumber">Número da página</param>
        /// <param name="PageSize">Quantidade de itens na página</param>
        /// <param name="Palavra">Palavra Filtro</param>
        /// <param name="CategoriaId">Id da Categoria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/ServicoNaoDestaques")]
        [ResponseType(typeof(List<ServicoListaVm>))]
        public IHttpActionResult GetNaoDestaques(int PageNumber = 1, int PageSize = 20, string Palavra = "", int CategoriaId = 0)
        {
            return Ok(NaoDestaques(PageNumber, PageSize, Palavra, CategoriaId));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<ServicoListaVm> Destaques(string Palavra = "", int CategoriaId = 0)
        {
            using (var db = new ApplicationDbContext())
            {

                List<Servico> ServicosList = db.Servico.Include(x => x.ServicoCategoria)
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                             x.Destaque &&
                                                             (string.IsNullOrEmpty(Palavra) || (x.Nome.Contains(Palavra))) &&
                                                             (CategoriaId == 0 || x.ServicoCategoriaId == CategoriaId))
                                                       .ToList();

                List<ServicoListaVm> Retorno = Mapper.Map<List<Servico>, List<ServicoListaVm>>(ServicosList.ToList());

                return Retorno;
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<ServicoListaVm> NaoDestaques(int PageNumber = 1, int PageSize = 20, string Palavra = "", int CategoriaId = 0)
        {
            using (var db = new ApplicationDbContext())
            {
                List<Servico> ServicosList = db.Servico.Include(x => x.ServicoCategoria)
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                             !x.Destaque &&
                                                             (string.IsNullOrEmpty(Palavra) || (x.Nome.Contains(Palavra))) &&
                                                             (CategoriaId == 0 || x.ServicoCategoriaId == CategoriaId))
                                                       .ToList();

                List<ServicoListaVm> Retorno = Mapper.Map<List<Servico>, List<ServicoListaVm>>(ServicosList.ToPagedList(PageNumber, PageSize).ToList());

                return Retorno;
            }
        }

        /// <summary>
        /// Retorna a listagem de items com pins
        /// </summary>
        /// <param name="CategoriaId">Id da Categoria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/ServicosPins")]
        [ResponseType(typeof(List<ServicoListaVm>))]
        public IHttpActionResult GetPins(int CategoriaId = 0)
        {
            using (var db = new ApplicationDbContext())
            {
                List<Servico> ServicosList = db.Servico.Where(x => x.Status == (int)StatusPadrao.Ativo && 
                                                              x.ServicoPin.Any() &&
                                                             (CategoriaId == 0 || x.ServicoCategoriaId == CategoriaId))
                                                       .Select(c => new
                                                       {
                                                           c,
                                                           ServicoArquivo = c.ServicoArquivo.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                           ServicoPin = c.ServicoPin.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                           ServicoCategoria = c.ServicoCategoria
                                                       })
                                                       .ToList().Select(p => p.c).ToList();

                List<ServicoListaVm> Retorno = Mapper.Map<List<Servico>, List<ServicoListaVm>>(ServicosList.ToList());

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna o Servico conforme o slug
        /// </summary>
        /// <param name="Slug">Slug do Servico</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Servico")]
        [ResponseType(typeof(ServicoListaVm))]
        public IHttpActionResult Get(string Slug = "")
        {
            using (var db = new ApplicationDbContext())
            {
                Servico Servico = db.Servico.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                    x.Slug == Slug)
                                            .Select(c => new
                                            {
                                                c,
                                                ServicoArquivo = c.ServicoArquivo.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                ServicoPin = c.ServicoPin.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                ServicoCategoria = c.ServicoCategoria
                                            })
                                            .ToList().Select(p => p.c).FirstOrDefault();

                if (Servico == null || Servico.Id == 0)
                {
                    return BadRequest("Serviço não encontrado");
                }

                ServicoListaVm Retorno = new ServicoListaVm();

                Mapper.Map(Servico, Retorno);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasServicos")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<ServicoCategoria> ServicoCategoriaList = db.ServicoCategoria
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<ServicoCategoria>, List<GenericVm>>(ServicoCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}