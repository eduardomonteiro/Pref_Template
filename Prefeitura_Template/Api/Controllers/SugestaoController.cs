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
    /// API Referente a "Contatos"
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [CacheOutput(ServerTimeSpan = 30)]
    public class SugestaoController : ApiController
    {
        /// <summary>
        /// Cadastrar Sugestão
        /// </summary>
        /// <param name="Contato">Objeto do Contato</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Api/CadastrarSugestao")]
        [ResponseType(typeof(string))]
        public IHttpActionResult CadastrarSugestao(ContatoVm Contato)
        {
            if (Contato == null)
            {
                return BadRequest("Contato Obrigatório");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var db = new ApplicationDbContext())
            {
                try
                {
                    Contato Model = new Contato();

                    Mapper.Map(Contato, Model);

                    Model.Status = (int)StatusPadrao.Ativo;
                    Model.DataCadastro = DateTime.Now;

                    db.Entry(Model).State = EntityState.Added;
                    db.SaveChanges();
                    return Ok("Sugestão cadastrado com sucesso");
                }
                catch (Exception e)
                {
                    string Erro = "Ocorreu um erro ao salvar.";
                    if (!string.IsNullOrEmpty(e.Message))
                    {
                        Erro += " ----> MENSAGEM :" + e.Message.ToString();
                    }

                    if (!string.IsNullOrEmpty(e.Message))
                    {
                        Erro += " ----> INNEREXCEPTION :" + e.InnerException.ToString();
                    }

                    return BadRequest(Erro);
                }
            }
        }

        /// <summary>
        /// Retorna os tipos de sugestão
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/SugestaoTipo")]
        [ResponseType(typeof(List<GenericVm>))]
        public IHttpActionResult SugestaoTipo()
        {
            using (var db = new ApplicationDbContext())
            {
                List<ContatoTipo> ContatoTipoList = db.ContatoTipo.Where(x => x.ContatoCategoriaId == 2 &&
                                                            x.Status == (int)StatusPadrao.Ativo)
                                                            .ToList();

                List<GenericVm> Retorno = Mapper.Map<List<ContatoTipo>, List<GenericVm>>(ContatoTipoList);

                return Ok(Retorno);
            }
        }
    }
}