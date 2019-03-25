using System.Collections.Generic;

namespace Prefeitura_Template.ViewModels
{
    public class NoticiaVinculadaVm
    {
        public string Titulo { get; set; }

        public string SubTitulo { get; set; }

        public string DataPublicacao { get; set; }

        public List<TagVm> Tags { get; set; }

        public GenericVm Categoria { get; set; }
    }
}