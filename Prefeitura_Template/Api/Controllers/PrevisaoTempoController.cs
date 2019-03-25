using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Xml.Serialization;
using Prefeitura_Template.Areas.Admin.Utils;
using Prefeitura_Template.Api.ViewModels;
using System.Web;
using WebApi.OutputCache.V2;

namespace Prefeitura_Template.Api.Controllers
{
    /// <summary>
    /// API com a Previsão do Tempo
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class PrevisaoTempoController : ApiController
    {
        /// <summary>
        /// Retorna a Previsão do Tempo para os próximos dias
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("Api/PrevisaoTempo")]
        [ResponseType(typeof(CidadePrevisaoVm))]
        public IHttpActionResult Get()
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://servicos.cptec.inpe.br/XML/cidade/3993/previsao.xml");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "GET";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var ser = new XmlSerializer(typeof(CidadePrevisaoVm));

                    var temperatura = (CidadePrevisaoVm)ser.Deserialize(new StringReader(streamReader.ReadToEnd()));

                    if(temperatura != null && temperatura.previsao.Count > 0)
                    {
                        for (int i = 0; i < temperatura.previsao.Count; i++)
                        {
                            temperatura.previsao[i].iconebranco = "http://" + HttpContext.Current.Request.Url.Authority + "/Areas/Admin/Images/clima_icones/branco/" + temperatura.previsao[i].tempo + ".png";
                            temperatura.previsao[i].iconepreto = "http://" + HttpContext.Current.Request.Url.Authority + "/Areas/Admin/Images/clima_icones/preto/" + temperatura.previsao[i].tempo + ".png";
                            try
                            {
                                temperatura.previsao[i].tempo = Utils.clima[temperatura.previsao[i].tempo];
                            }
                            catch
                            {
                                temperatura.previsao[i].tempo = temperatura.previsao[i].tempo;
                            }
                        }
                    }
                    
                    return Ok(temperatura);
                }
            }
            catch
            {
                return BadRequest("Ocorreu um erro ao pegar a previsão");
            }
        }
    }
}