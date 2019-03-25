using System.Collections.Generic;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class EnqueteVm
    {
        /// <summary>
        /// Id da Enquete
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Pergunta
        /// </summary>
        public string Pergunta { get; set; }

        /// <summary>
        /// Data da Publicacao da enquete
        /// </summary>
        public string DataPublicacao { get; set; }

        /// <summary>
        /// Data de Encerramento da enquete
        /// </summary>
        public string DataEncerramento { get; set; }

        /// <summary>
        /// Encerrado? True = Sim , False = Não
        /// </summary>
        public bool Encerrado { get; set; }

        /// <summary>
        /// Opcoes da enquete
        /// </summary>
        public List<EnqueteOpcaoVm> EnqueteOpcao { get; set; }
    }
}