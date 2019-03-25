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
    public class BuscaController : ApiController
    {
        /// <summary>
        /// Retorna a listagem de Bairros
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Busca")]
        [ResponseType(typeof(List<BuscaVm>))]
        public IHttpActionResult Get(string Palavra = "")
        {
            if (string.IsNullOrEmpty(Palavra))
            {
                return BadRequest("Digite a palavra que deseja buscar");
            }

            using (var db = new ApplicationDbContext())
            {
                List<BuscaVm> Retorno = new List<BuscaVm>();

                List<AaZ> AaZList = db.AaZ.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Url.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(AaZList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 6,
                    Descricao = x.Url,
                    Slug = null,
                    AreaDescricao = "Prefeitura de A a Z",
                    DataPublicacao = x.DataCadastro
                }));

                List<Concurso> ConcursoList = db.Concurso.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Descricao.Contains(Palavra) ||
                                                 x.Slug.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(ConcursoList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 18,
                    Descricao = x.Descricao,
                    Slug = x.Slug,
                    AreaDescricao = "Concursos",
                    DataPublicacao = x.DataCadastro
                }));

                List<Documento> DocumentoList = db.Documento.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Texto.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(DocumentoList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 10,
                    Descricao = x.Texto,
                    Slug = null,
                    AreaDescricao = "Documentos",
                    DataPublicacao = x.DataCadastro
                }));

                List<Enquete> EnqueteList = db.Enquete.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Pergunta.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(EnqueteList.Select(x => new BuscaVm
                {
                    Titulo = x.Pergunta,
                    AreaId = 20,
                    Descricao = null,
                    Slug = null,
                    AreaDescricao = "Enquetes",
                    DataPublicacao = x.DataCadastro
                }));

                List<Evento> EventoList = db.Evento.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                x.SubTitulo.Contains(Palavra) ||
                                                 x.Texto.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(EventoList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 7,
                    Descricao = x.SubTitulo,
                    Slug = x.Slug,
                    AreaDescricao = "Eventos",
                    DataPublicacao = x.DataCadastro
                }));

                List<ExPrefeito> ExPrefeitoList = db.ExPrefeito.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Nome.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(ExPrefeitoList.Select(x => new BuscaVm
                {
                    Titulo = x.Nome,
                    AreaId = 3,
                    Descricao = null,
                    Slug = null,
                    AreaDescricao = "Ex-Prefeitos",
                    DataPublicacao = x.DataCadastro
                }));

                List<GaleriaAudio> GaleriaAudioList = db.GaleriaAudio.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Descricao.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(GaleriaAudioList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 14,
                    Descricao = x.Descricao,
                    Slug = null,
                    AreaDescricao = "Galeria de Áudios",
                    DataPublicacao = x.DataCadastro
                }));

                List<GaleriaFoto> GaleriaFotoList = db.GaleriaFoto.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Descricao.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(GaleriaFotoList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 13,
                    Descricao = x.Descricao,
                    Slug = x.Slug,
                    AreaDescricao = "Galeria de Fotos",
                    DataPublicacao = x.DataCadastro
                }));

                List<GaleriaVideo> GaleriaVideoList = db.GaleriaVideo.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Descricao.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(GaleriaVideoList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 21,
                    Descricao = x.Descricao,
                    Slug = x.Slug,
                    AreaDescricao = "Galeria de Vídeos",
                    DataPublicacao = x.DataCadastro
                }));

                List<Informativo> InformativoList = db.Informativo.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Texto.Contains(Palavra) ||
                                                 x.SubTitulo.Contains(Palavra) ||
                                                 x.Slug.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(InformativoList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 12,
                    Descricao = x.SubTitulo,
                    Slug = x.Slug,
                    AreaDescricao = "Informativos",
                    DataPublicacao = x.DataCadastro
                }));

                List<Legislacao> LegislacaoList = db.Legislacao.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Descricao.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(LegislacaoList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 22,
                    Descricao = x.Descricao,
                    Slug = null,
                    AreaDescricao = "Legislação",
                    DataPublicacao = x.DataCadastro
                }));

                List<Licitacao> LicitacaoList = db.Licitacao.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Descricao.Contains(Palavra) ||
                                                 x.Slug.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(LicitacaoList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 17,
                    Descricao = x.Descricao,
                    Slug = x.Slug,
                    AreaDescricao = "Licitação",
                    DataPublicacao = x.DataCadastro
                }));

                List<Noticia> NoticiaList = db.Noticia.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.SubTitulo.Contains(Palavra) ||
                                                 x.Texto.Contains(Palavra) ||
                                                 x.Slug.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(NoticiaList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 8,
                    Descricao = x.SubTitulo,
                    Slug = x.Slug,
                    AreaDescricao = "Notícias",
                    DataPublicacao = x.DataCadastro
                }));

                List<PatrimonioHistoricoCultural> PatrimonioHistoricoCulturalList = db.PatrimonioHistoricoCultural.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Nome.Contains(Palavra) ||
                                                 x.Descricao.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(PatrimonioHistoricoCulturalList.Select(x => new BuscaVm
                {
                    Titulo = x.Nome,
                    AreaId = 24,
                    Descricao = x.Descricao,
                    Slug = null,
                    AreaDescricao = "Patrimonio Histórico Cultural",
                    DataPublicacao = x.DataCadastro
                }));

                List<PerfilSocioEconomico> PerfilSocioEconomicoList = db.PerfilSocioEconomico.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Descricao.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(PerfilSocioEconomicoList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 25,
                    Descricao = x.Descricao,
                    Slug = null,
                    AreaDescricao = "Perfil Sócio-Econômico",
                    DataPublicacao = x.DataCadastro
                }));

                List<PerguntasFrequentes> PerguntasFrequentesList = db.PerguntasFrequentes.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                 x.Descricao.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(PerguntasFrequentesList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 26,
                    Slug = null,
                    Descricao = x.Descricao,
                    AreaDescricao = "Perguntas Frequentes",
                    DataPublicacao = x.DataCadastro
                }));

                List<Projeto> ProjetoList = db.Projeto.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Titulo.Contains(Palavra) ||
                                                x.SubTitulo.Contains(Palavra) ||
                                                x.Slug.Contains(Palavra) ||
                                                 x.Descricao.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(ProjetoList.Select(x => new BuscaVm
                {
                    Titulo = x.Titulo,
                    AreaId = 9,
                    Descricao = x.SubTitulo,
                    Slug = x.Slug,
                    AreaDescricao = "Projetos",
                    DataPublicacao = x.DataCadastro
                }));

                List<Secretaria> SecretariaList = db.Secretaria.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Nome.Contains(Palavra) ||
                                                x.Slug.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(SecretariaList.Select(x => new BuscaVm
                {
                    Titulo = x.Nome,
                    AreaId = 11,
                    Descricao = null,
                    Slug = x.Slug,
                    AreaDescricao = "Secretarias",
                    DataPublicacao = x.DataCadastro
                }));

                List<Servico> ServicoList = db.Servico.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Nome.Contains(Palavra) ||
                                                x.Slug.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(ServicoList.Select(x => new BuscaVm
                {
                    Titulo = x.Nome,
                    AreaId = 27,
                    Descricao = null,
                    Slug = x.Slug,
                    AreaDescricao = "Serviços",
                    DataPublicacao = x.DataCadastro
                }));

                List<Turismo> TurismoList = db.Turismo.Where(x => x.Status == (int)StatusPadrao.Ativo &&
                                                (x.Nome.Contains(Palavra) ||
                                                x.Descricao.Contains(Palavra))
                                                ).ToList();

                Retorno.AddRange(TurismoList.Select(x => new BuscaVm
                {
                    Titulo = x.Nome,
                    AreaId = 5,
                    Descricao = x.Descricao,
                    Slug = null,
                    AreaDescricao = "Turismo",
                    DataPublicacao = x.DataCadastro
                }));

                Retorno = Retorno.OrderByDescending(x => x.DataPublicacao).ToList();

                return Ok(Retorno);
            }
        }
    }
}