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
    /// API Referente a "Galeria de Audios"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class DocumentoController : ApiController
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
        [Route("Api/Documentos")]
        [ResponseType(typeof(List<DocumentoVm>))]
        public IHttpActionResult Get(int PageNumber = 1, int PageSize = 20, string Palavra = "", int Data = 0, int CategoriaId = 0)
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

                List<Documento> DocumentoList = db.Documento.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                             (string.IsNullOrEmpty(Palavra) || (x.Titulo.Contains(Palavra))) &&
                                                             (CategoriaId == 0 || x.DocumentoCategoriaId == CategoriaId) &&
                                                             (Data == 0 || DbFunctions.TruncateTime(x.DataPublicacao) >= DbFunctions.TruncateTime(Date)))
                                                           .Select(c => new
                                                           {
                                                               c,
                                                               DocumentoArquivo = c.DocumentoArquivo.Where(x => x.Status == (int)StatusPadrao.Ativo),
                                                               DocumentoCategoria = c.DocumentoCategoria
                                                           })
                                                           .ToList().Select(p => p.c).ToList();

                List<DocumentoVm> Retorno = Mapper.Map<List<Documento>, List<DocumentoVm>>(DocumentoList.ToPagedList(PageNumber, PageSize).ToList());

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasDocumentos")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<DocumentoCategoria> DocumentoCategoriaList = db.DocumentoCategoria
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<DocumentoCategoria>, List<GenericVm>>(DocumentoCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}