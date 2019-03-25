using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    public class EventoVm
    {
        /// <summary>
        /// Galeria do Evento
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Galeria do Evento
        /// </summary>
        public string SubTitulo { get; set; }

        /// <summary>
        /// Texto do Evento
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// Slug do Evento
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Data do Evento
        /// </summary>
        public string DataHorarioEvento { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Evento
        /// </summary>
        public string Imagem { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Evento
        /// </summary>
        public string CaminhoLogicoImagem { get; set; }

        /// <summary>
        /// Categoria do Evento
        /// </summary>
        public GenericVm EventoCategoria { get; set; }

        /// <summary>
        /// Galeira do Evento
        /// </summary>
        public List<EventoGaleriaVm> EventoGaleria { get; set; }

        /// <summary>
        /// Link do Video
        /// </summary>
        public string LinkVideo { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public virtual List<TagVm> Tag { get; set; }
    }
}