using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Prefeitura_Template.Models;
using Prefeitura_Template.Api.ViewModels;
using Prefeitura_Template.Areas.Admin.Models;

namespace Prefeitura_Template.App_Start
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AaZCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<AaZ, AaZVm>().ReverseMap();

                cfg.CreateMap<Bairro, GenericVm>().ReverseMap();

                cfg.CreateMap<CidadeVm, Cidade>().ReverseMap().ForMember(x => x.DataFundacao,
                                                                        opt => opt.MapFrom(src => src.DataFundacao != null ? ((DateTime)src.DataFundacao).ToShortDateString() : ""));

                cfg.CreateMap<ConcursoModalidade, GenericVm>().ReverseMap();
                cfg.CreateMap<ConcursoArquivo, ConcursoArquivoVm>().ReverseMap();
                cfg.CreateMap<ConcursoVm, Concurso>().ReverseMap().ForMember(x => x.DataInicio,
                                                                        opt => opt.MapFrom(src => src.DataInicio != null ? ((DateTime)src.DataInicio).ToShortDateString() : ""))
                                                                .ForMember(x => x.DataFim,
                                                                        opt => opt.MapFrom(src => src.DataFim != null ? ((DateTime)src.DataFim).ToShortDateString() : ""));

                cfg.CreateMap<ContatoTipo, GenericVm>().ReverseMap();
                cfg.CreateMap<ContatoCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<Contato, ContatoVm>().ReverseMap();

                cfg.CreateMap<DocumentoViewModel, Documento>().ReverseMap();
                cfg.CreateMap<DocumentoCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<DocumentoArquivo, DocumentoArquivoVm>().ReverseMap();
                cfg.CreateMap<DocumentoVinculadoVm, Documento>().ReverseMap();
                cfg.CreateMap<DocumentoVm, Documento>().ReverseMap().ForMember(x => x.DataPublicacao,
                                                                        opt => opt.MapFrom(src => src.DataPublicacao != null ? ((DateTime)src.DataPublicacao).ToShortDateString() : ""));

                cfg.CreateMap<EnqueteOpcao, EnqueteOpcaoVm>().ReverseMap();
                cfg.CreateMap<EnqueteVm, Enquete>().ReverseMap().ForMember(x => x.DataPublicacao,
                                                                        opt => opt.MapFrom(src => src.DataPublicacao != null ? ((DateTime)src.DataPublicacao).ToShortDateString() : ""));

                cfg.CreateMap<EventoCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<EventoGaleria, EventoGaleriaVm>().ReverseMap();
                cfg.CreateMap<EventoVm ,Evento>().ReverseMap().ForMember(x => x.DataHorarioEvento,
                                                                        opt => opt.MapFrom(src => src.DataHorarioEvento != null ? ((DateTime)src.DataHorarioEvento).ToShortDateString() : ""));
                cfg.CreateMap<EventoVinculadoVm , Evento>().ReverseMap().ForMember(x => x.DataHorarioEvento,
                                                                        opt => opt.MapFrom(src => src.DataHorarioEvento != null ? ((DateTime)src.DataHorarioEvento).ToShortDateString() : ""));

                cfg.CreateMap<ExPrefeitoListaVm, ExPrefeito>().ReverseMap().ForMember(x => x.DataFimLegislatura,
                                                                        opt => opt.MapFrom(src => src.DataFimLegislatura != null ? ((DateTime)src.DataFimLegislatura).ToShortDateString() : ""))
                                                                        .ForMember(x => x.DataInicioLegislatura,
                                                                        opt => opt.MapFrom(src => src.DataInicioLegislatura != null ? ((DateTime)src.DataInicioLegislatura).ToShortDateString() : ""));

                cfg.CreateMap<GaleriaAudioCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<GaleriaAudioListaVm, GaleriaAudio>().ReverseMap().ForMember(x => x.DataPublicacao,
                                                                        opt => opt.MapFrom(src => src.DataPublicacao != null ? ((DateTime)src.DataPublicacao).ToShortDateString() : ""));

                cfg.CreateMap<GaleriaFotoCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<GaleriaFotoGaleria, GaleriaFotoGaleriaVm>().ReverseMap();
                cfg.CreateMap<GaleriaFotoListaVm, GaleriaFoto>().ReverseMap().ForMember(x => x.DataPublicacao,
                                                                        opt => opt.MapFrom(src => src.DataPublicacao != null ? ((DateTime)src.DataPublicacao).ToShortDateString() : ""));

                cfg.CreateMap<GaleriaVideoCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<GaleriaVideoListaVm, GaleriaVideo>().ReverseMap().ForMember(x => x.DataPublicacao,
                                                                        opt => opt.MapFrom(src => src.DataPublicacao != null ? ((DateTime)src.DataPublicacao).ToShortDateString() : ""));

                cfg.CreateMap<InformativoCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<InformativoGaleria, InformativoGaleriaVm>().ReverseMap();
                cfg.CreateMap<InformativoListaVm, Informativo>().ReverseMap().ForMember(x => x.DataPublicacao,
                                                                        opt => opt.MapFrom(src => src.DataPublicacao != null ? ((DateTime)src.DataPublicacao).ToShortDateString() : ""));

                cfg.CreateMap<LegislacaoCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<LegislacaoArquivo, LegislacaoArquivoVm>().ReverseMap();
                cfg.CreateMap<LegislacaoVm, Legislacao>().ReverseMap().ForMember(x => x.DataPublicacao,
                                                                        opt => opt.MapFrom(src => src.DataPublicacao != null ? ((DateTime)src.DataPublicacao).ToShortDateString() : ""));

                cfg.CreateMap<LicitacaoModalidade, GenericVm>().ReverseMap();
                cfg.CreateMap<LicitacaoArquivo, LicitacaoArquivoVm>().ReverseMap();
                cfg.CreateMap<LicitacaoVm, Licitacao>().ReverseMap().ForMember(x => x.DataPublicacao,
                                                                        opt => opt.MapFrom(src => src.DataPublicacao != null ? ((DateTime)src.DataPublicacao).ToShortDateString() : ""))
                                                                    .ForMember(x => x.DataAlteracaoEditavel,
                                                                        opt => opt.MapFrom(src => src.DataAlteracaoEditavel != null ? ((DateTime)src.DataAlteracaoEditavel).ToShortDateString() : ""));

                cfg.CreateMap<NewsLetter, NewsLetterVm>().ReverseMap();

                cfg.CreateMap<NoticiaCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<NoticiaGaleria, NoticiaGaleriaVm>().ReverseMap();
                cfg.CreateMap<NoticiaVinculadaVm, Noticia>().ReverseMap().ForMember(x => x.DataPublicacao,
                                                                        opt => opt.MapFrom(src => src.DataPublicacao != null ? ((DateTime)src.DataPublicacao).ToString("dd/MM/yyyy hh:mm") : ""));
                cfg.CreateMap<NoticiaListaVm, Noticia>().ReverseMap().ForMember(x => x.DataPublicacao,
                                                                        opt => opt.MapFrom(src => src.DataPublicacao != null ? ((DateTime)src.DataPublicacao).ToString("dd/MM/yyyy hh:mm") : ""));

                cfg.CreateMap<PatrimonioHistoricoCulturalCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<PatrimonioHistoricoCultural, PatrimonioHistoricoCulturalListaVm>().ReverseMap();

                cfg.CreateMap<PerfilSocioEconomicoCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<PerfilSocioEconomico, PerfilSocioEconomicoVm>().ReverseMap();

                cfg.CreateMap<PerguntasFrequentesCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<PerguntasFrequentesVm, PerguntasFrequentes>().ReverseMap();

                cfg.CreateMap<ProjetoCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<ProjetoArquivo, ProjetoArquivoVm>().ReverseMap();
                cfg.CreateMap<Projeto, ProjetoVinculadoVm>().ReverseMap();
                cfg.CreateMap<Projeto, ProjetoVm>().ReverseMap();

                cfg.CreateMap<SecretariaCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<Secretaria, SecretariaVinculadaVm>().ReverseMap();
                cfg.CreateMap<Secretaria, SecretariaVm>().ReverseMap();

                cfg.CreateMap<ServicoCategoria, GenericVm>().ReverseMap();
                cfg.CreateMap<ServicoListaVm, Servico>().ReverseMap();
                cfg.CreateMap<ServicoVm, Servico>().ReverseMap();
                cfg.CreateMap<ServicoArquivoVm, ServicoArquivo>().ReverseMap();
                cfg.CreateMap<ServicoPinVm, ServicoPin>().ReverseMap();

                cfg.CreateMap<StatusPublicacao, GenericVm>().ReverseMap();

                cfg.CreateMap<Timeline, TimelineVm>().ReverseMap();

                cfg.CreateMap<Turismo, TurismoListaVm>().ReverseMap();

                cfg.CreateMap<Tag, TagVm>().ReverseMap();
            });
        }
    }
}