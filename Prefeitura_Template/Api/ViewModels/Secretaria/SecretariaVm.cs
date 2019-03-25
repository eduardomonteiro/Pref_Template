using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class SecretariaVm
    {
        /// <summary>
        /// Nome da Secretaria
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Prefixo da Secretaria
        /// </summary>
        public string Prefixo { get; set; }
        /// <summary>
        /// Slug da Secretaria
        /// </summary>
        public string Slug { get; set; }
        /// <summary>
        /// Arquivo do Icone
        /// </summary>
        public string Icone { get; set; }

        /// <summary>
        /// Caminho Completo do Icone
        /// </summary>
        public string CaminhoLogicoIcone { get; set; }

        /// <summary>
        /// Arquivo da Imagem
        /// </summary>
        public string ImagemLocal { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem
        /// </summary>
        public string CaminhoLogicoImagemLocal { get; set; }

        /// <summary>
        /// Nome do Responsável
        /// </summary>
        public string NomeResponsavel { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Responsável
        /// </summary>
        public string ImagemResponsavel { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Responsável
        /// </summary>
        public string CaminhoLogicoImagemResponsavel { get; set; }

        /// <summary>
        /// Cargo do Responsável
        /// </summary>
        public string Cargo { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// Horário de Atendimento
        /// </summary>
        public string HorarioAtendimento { get; set; }

        /// <summary>
        /// Endereço
        /// </summary>
        public string Endereco { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Atribuições do Setor
        /// </summary>
        public string Atribuicao { get; set; }

        /// <summary>
        /// Listagem das Noticias
        /// </summary>
        public List<NoticiaVinculadaVm> Noticias { get; set; }

        /// <summary>
        /// Listagem dos eventos
        /// </summary>
        public List<EventoVinculadoVm> Eventos { get; set; }

        /// <summary>
        /// Listagem dos Documentos
        /// </summary>
        public List<DocumentoVinculadoVm> Documentos { get; set; }

        /// <summary>
        /// Listagem dos Projetos
        /// </summary>
        public List<ProjetoVinculadoVm> Projetos { get; set; }

        /// <summary>
        /// Listagem das Secretarias
        /// </summary>
        public List<SecretariaVinculadaVm> Secretarias { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public List<TagVm> Tag { get; set; }

        /// <summary>
        /// Categoria do Secretaria
        /// </summary>
        public GenericVm SecretariaCategoria { get; set; }

        /// <summary>
        /// Pricipais Serviços
        /// </summary>
        public string PincipaisServicos { get; set; }
    }
}