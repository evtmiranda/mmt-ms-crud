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
        private LogDAO logDAO;

        //construtor do controller, recebe um produtoDAO e um logDAO, que por sua vez recebe uma ISession.
        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public ProdutoAdicionalController(ProdutoAdicionalDAO produtoAdicionalDAO, LogDAO logDAO)
        {
            this.produtoAdicionalDAO = produtoAdicionalDAO;
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

        /// <summary>
        /// Retorna todos os produtos adicionais de um determinado produto
        /// </summary>
        /// <param name="idProduto">Id do produto ao qual os produtos adicionais devem ser consultados</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ProdutoAdicional/BuscarProdutosAdicionaisDeUmProduto/{idProduto}")]
        public HttpResponseMessage BuscarProdutosAdicionaisDeUmProduto(int idProduto)
        {
            try
            {
                List<DadosProdutoAdicionalProduto> produtosAdicionaisProduto = produtoAdicionalDAO.BuscarProdutosAdicionaisDeUmProduto(idProduto);
                return Request.CreateResponse(HttpStatusCode.OK, produtosAdicionaisProduto);
            }
            catch (Exception)
            {
                string mensagem = string.Format("ocorreu um problema ao buscar os produtos adicionais. por favor, tente atualizar a página ou acessar dentro de alguns minutos...");
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
        }
    }
}
