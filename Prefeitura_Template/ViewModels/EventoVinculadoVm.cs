using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prefeitura_Template.ViewModels
{
    public class EventoVinculadoVm
    {
        public string Titulo { get; set; }

        public string SubTitulo { get; set; }

        public string DataEvento { get; set; }

        public List<TagVm> Tags { get; set; }

        public GenericVm Categoria { get; set; }
    }
}