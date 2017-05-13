using ClassesMarmitex;
using ms_crud_rest.DAO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ms_crud_rest.Controllers
{
    public class ProdutoAdicionalController : ApiController
    {
        private ProdutoAdicionalDAO produtoAdicionalDAO;
        private LojaDAO lojaDAO;
        private LogDAO logDAO;

        //construtor do controller, recebe um produtoDAO e um logDAO, que por sua vez recebe uma ISession.
        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public ProdutoAdicionalController(ProdutoAdicionalDAO produtoAdicionalDAO,  LojaDAO lojaDAO, LogDAO logDAO)
        {
            this.produtoAdicionalDAO = produtoAdicionalDAO;
            this.lojaDAO = lojaDAO;
            this.logDAO = logDAO;
        }

        /// <summary>
        /// Retorna todos os produtos adicionais de uma determinada loja
        /// </summary>
        /// <param name="idLoja">Id da loja ao qual os produtos pertencem</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ProdutoAdicional/Listar/{idLoja}")]
        public HttpResponseMessage ListarProdutosAdicionais(int idLoja)
        {
            try
            {
                List<DadosProdutoAdicional> produtosAdicionais = produtoAdicionalDAO.Listar(idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, produtosAdicionais);
            }
            catch (Exception)
            {
                string mensagem = string.Format("ocorreu um problema ao buscar os produtos adicionais. por favor, tente atualizar a página ou acessar dentro de alguns minutos...");
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
        }

        [Route("api/ProdutoAdicional/Adicionar/{idLoja}")]
        public HttpResponseMessage Post([FromBody] DadosProdutoAdicional produtoAdicional, [FromUri] int idLoja)
        {
            try
            {
                produtoAdicional.IdLoja = idLoja;

                produtoAdicionalDAO.Adicionar(produtoAdicional);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                return response;
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o produto adicional. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }
    }
}
