using AutoMapper;
using Prefeitura_Template.Api.ViewModels;
using Prefeitura_Template.Areas.Admin.Enums;
using Prefeitura_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using WebApi.OutputCache.V2;

namespace Prefeitura_Template.Api.Controllers
{
    /// <summary>
    /// API Referente a "Secretarias"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class SecretariaController : ApiController
    {
        [HttpGet]
        [Route("Api/Secretarias")]
        [ResponseType(typeof(List<SecretariaVinculadaVm>))]
        public IHttpActionResult Get()
        {
            using (var db = new ApplicationDbContext())
            {
                List<Secretaria> SecretariasList = db.Secretaria
                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                       .Select(c => new
                                                       {
                                                           c,
                                                           SecretariaServico = c.SecretariaServico.Where(x => x.Status != (int)StatusPadrao.Excluido),
                                                           SecretariaCategoria = c.SecretariaCategoria,
                                                           SecretariaNomePrefixo = c.SecretariaNomePrefixo
                                                       })
                                                        .ToList().Select(p => p.c).ToList();

                List<SecretariaVinculadaVm> Retorno = Mapper.Map<List<Secretaria>, List<SecretariaVinculadaVm>>(SecretariasList);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna as Informações da Secretaria conforme slug ou se é ou não o gabinete
        /// </summary>
        /// <param name="Slug">Slug da Secretaria</param>
        /// <param name="Gabinete">Gabinete</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Secretaria")]
        [ResponseType(typeof(SecretariaVm))]
        public IHttpActionResult Get(string Slug = "", bool Gabinete = false)
        {
            using (var db = new ApplicationDbContext())
            {
                Secretaria Secretaria = new Models.Secretaria();
                if (Gabinete)
                {
                    Secretaria = db.Secretaria.Where(x => x.SecretariaCategoriaId == 1 && x.Status == (int)StatusPadrao.Ativo)
                                            .Select(c => new
                                            {
                                                c,
                                                SecretariaServico = c.SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo).OrderBy(x => x.Ordem),
                                                SecretariaCategoria = c.SecretariaCategoria,
                                                SecretariaNomePrefixo = c.SecretariaNomePrefixo
                                            })
                                            .ToList().Select(p => p.c).FirstOrDefault();
}
                else if(Slug != "")
                {
                    Secretaria = db.Secretaria.Where(x => x.Slug == Slug && x.Status == (int)StatusPadrao.Ativo)
                                                .Select(c => new
                                                {
                                                    c,
                                                    SecretariaServico = c.SecretariaServico.Where(x => x.Status == (int)StatusPadrao.Ativo).OrderBy(x => x.Ordem),
                                                    SecretariaCategoria = c.SecretariaCategoria,
                                                    SecretariaNomePrefixo = c.SecretariaNomePrefixo
                                                })
                                                .ToList().Select(p => p.c).FirstOrDefault();
                }
                else
                {
                    return BadRequest("Secretaria não encontrada");
                }

                if (Secretaria == null || Secretaria.Id == 0)
                {
                    return BadRequest("Secretaria não encontrada");
                }

                SecretariaVm Retorno = new SecretariaVm();

                Mapper.Map(Secretaria, Retorno);

                List<string> GabineteTags = Secretaria.Tag.Select(x => x.Slug).ToList();

                List<Evento> Eventos = db.Evento.Include(x => x.EventoCategoria)
                                                .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                .ToList();
                List<int> EventosIds = Eventos.Select(x => x.Id).ToList();
                List<Tag> EventosTags = db.Tag.Where(x => x.AreaId == 7 && 
                                                     EventosIds.Contains(x.RegistroId) && 
                                                     GabineteTags.Contains(x.Slug)
                                                     )
                                                     .ToList();
                List<int> EventosTagsIds = EventosTags.Select(y => y.RegistroId).ToList();
                Eventos = Eventos.Where(x => EventosTagsIds.Contains(x.Id)).Take(4).ToList();


                List<Noticia> Noticias = db.Noticia.Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                   .OrderByDescending(x => x.Destaque)
                                                   .Select(c => new
                                                   {
                                                       c,
                                                       NoticiaCategoria = c.NoticiaCategoria,
                                                       NoticiaGaleria = c.NoticiaGaleria.FirstOrDefault()
                                                   })
                                                   .ToList().Select(p => p.c).ToList();

                List<int> NoticiasIds = Noticias.Select(x => x.Id).ToList();
                List<Tag> NoticiasTags = db.Tag.Where(x => x.AreaId == 8 && 
                                                      NoticiasIds.Contains(x.RegistroId) && 
                                                      GabineteTags.Contains(x.Slug)
                                                      )
                                                      .ToList();
                List<int> NoticiasTagsIds = NoticiasTags.Select(y => y.RegistroId).ToList();
                Noticias = Noticias.Where(x => NoticiasTagsIds.Contains(x.Id)).Take(3).ToList();


                List<Projeto> Projetos = db.Projeto.Include(x => x.Secretaria)
                                                   .Include(x => x.ProjetoCategoria)
                                                   .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                   .ToList();
                List<int> ProjetosIds = Projetos.Select(x => x.Id).ToList();
                List<Tag> ProjetosTags = db.Tag.Where(x => x.AreaId == 9 && 
                                                      ProjetosIds.Contains(x.RegistroId) && 
                                                      GabineteTags.Contains(x.Slug)
                                                      )
                                                      .ToList();
                List<int> ProjetosTagsIds = ProjetosTags.Select(y => y.RegistroId).ToList();
                Projetos = Projetos.Where(x => ProjetosTagsIds.Contains(x.Id)).Take(12).ToList();


                List<Documento> Documentos = db.Documento.Include(x => x.DocumentoCategoria)
                                                        .Include(x => x.DocumentoArquivo)
                                                        .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                        .ToList();
                List<int> DocumentosIds = Documentos.Select(x => x.Id).ToList();
                List<Tag> DocumentosTags = db.Tag.Where(x => x.AreaId == 10 && 
                                                        DocumentosIds.Contains(x.RegistroId) && 
                                                        GabineteTags.Contains(x.Slug)
                                                        )
                                                        .ToList();
                List<int> DocumentosTagsIds = DocumentosTags.Select(y => y.RegistroId).ToList();
                Documentos = Documentos.Where(x => DocumentosTagsIds.Contains(x.Id)).Take(12).ToList();

                List<Secretaria> Secretarias = db.Secretaria.Include(x => x.SecretariaServico)
                                                            .Include(x => x.SecretariaCategoria)
                                                            .Include(x => x.SecretariaNomePrefixo)
                                                            .Where(x => x.Status == (int)StatusPadrao.Ativo).Take(3).ToList();

                Retorno.Eventos = Mapper.Map<List<Evento>, List<EventoVinculadoVm>>(Eventos);

                Retorno.Noticias = Mapper.Map<List<Noticia>, List<NoticiaVinculadaVm>>(Noticias);

                Retorno.Projetos = Mapper.Map<List<Projeto>, List<ProjetoVinculadoVm>>(Projetos);

                Retorno.Documentos = Documentos.Select(x => new DocumentoVinculadoVm
                                    {
                                        Arquivo = x.DocumentoArquivoAtual.Arquivo,
                                        Nome = x.DocumentoArquivoAtual.ArquivoNome,
                                        CaminhoLogicoArquivo = x.DocumentoArquivoAtual.CaminhoLogicoArquivo,
                                        Formato = x.DocumentoArquivoAtual.Formato,
                                        Tag = Mapper.Map<List<Tag>, List<TagVm>>(x.Tag.ToList())
                                    }).ToList();

                Retorno.Secretarias = Mapper.Map<List<Secretaria>, List<SecretariaVinculadaVm>>(Secretarias);

                return Ok(Retorno);
            }
        }

        /// <summary>
        /// Retorna a listagem de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/CategoriasSecretarias")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult GetCategorias()
        {
            using (var db = new ApplicationDbContext())
            {
                List<SecretariaCategoria> SecretariasCategoriaList = db.SecretariaCategoria
                                                                       .Where(x => x.Status == (int)StatusPadrao.Ativo)
                                                                       .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<SecretariaCategoria>, List<GenericVm>>(SecretariasCategoriaList);

                return Ok(Retorno);
            }
        }
    }
}