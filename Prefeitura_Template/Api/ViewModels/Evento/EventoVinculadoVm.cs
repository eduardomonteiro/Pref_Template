using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto do Evento Vinculado
    /// </summary>
    public class EventoVinculadoVm
    {
        /// <summary>
        /// Slug do Evento
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Titulo do Evento
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Texto do Evento
        /// </summary>
        public string Texto { get; set; }

        /// <summary>
        /// Sub-Titulo do Evento
        /// </summary>
        public string SubTitulo { get; set; }

        /// <summary>
        /// Data e Horario doEvento
        /// </summary>
        public string DataHorarioEvento { get; set; }

        /// <summary>
        /// Tags que fizeram o vínculo
        /// </summary>
        public List<TagVm> Tag { get; set; }

        /// <summary>
        /// Categoria do Evento
        /// </summary>
        public GenericVm EventoCategoria { get; set; }
    }
}