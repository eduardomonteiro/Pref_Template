using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prefeitura_Template.Api.ViewModels
{
    /// <summary>
    /// Objeto de Retorno
    /// </summary>
    public class CidadeVm
    {
        /// <summary>
        /// HTML de descrição da página de "A Cidade"
        /// </summary>
        [AllowHtml]
        public string Descricao { get; set; }

        /// <summary>
        /// Arquivo da Imagem de "A Cidade"
        /// </summary>
        public string ImagemDescricao { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem de "A Cidade"
        /// </summary>
        public string CaminhoLogicoImagemDescricao { get; set; }

        /// <summary>
        /// HTML de descrição da Bandeira
        /// </summary>
        [AllowHtml]
        public string DescricaoBandeira { get; set; }

        /// <summary>
        /// Arquivo da Imagem da Bandeira
        /// </summary>
        public string ImagemBandeira { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem da Bandeira
        /// </summary>
        public string CaminhoLogicoImagemBandeira { get; set; }

        /// <summary>
        /// HTML de descrição do Brasão
        /// </summary>
        [AllowHtml]
        public string DescricaoBrasao { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Brasão
        /// </summary>
        public string ImagemBrasao { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Brasão
        /// </summary>
        public string CaminhoLogicoImagemBrasao { get; set; }

        /// <summary>
        /// HTML de descrição do Invista na Cidade
        /// </summary>
        [AllowHtml]
        public string DescricaoInvista { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Invista na Cidade
        /// </summary>
        public string ImagemInvista { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Invista na Cidade
        /// </summary>
        public string CaminhoLogicoImagemInvista { get; set; }

        /// <summary>
        /// HTML de descrição do Hino
        /// </summary>
        [AllowHtml]
        public string DescricaoHino { get; set; }

        /// <summary>
        /// Arquivo da Imagem do Brasão
        /// </summary>
        public string AudioHino { get; set; }

        /// <summary>
        /// Caminho Completo do Arquivo da Imagem do Brasão
        /// </summary>
        public string CaminhoLogicoAudioHino { get; set; }

        /// <summary>
        /// Data de Fundação da Cidade
        /// </summary>
        public string DataFundacao { get; set; }

        /// <summary>
        /// Atual Prefeito da Cidade
        /// </summary>
        public string AtualPrefeito { get; set; }

        /// <summary>
        /// População da Cidade
        /// </summary>
        public string Populacao { get; set; }

        /// <summary>
        /// Clima da Cidade
        /// </summary>
        public string Clima { get; set; }

        /// <summary>
        /// Area da Cidade
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Dennsidade da Cidade
        /// </summary>
        public string Densidade { get; set; }

        /// <summary>
        /// Altitude da Cidade
        /// </summary>
        public string Altitude { get; set; }

        /// <summary>
        /// Lista com a timeline
        /// </summary>
        public List<TimelineVm> Timeline { get; set; }
    }
}