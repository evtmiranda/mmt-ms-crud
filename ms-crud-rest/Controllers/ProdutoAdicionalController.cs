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
        /// Retorna todos os produtos existentes dentro do cardápio enviado como parâmetro
        /// </summary>
        /// <param name="idMenuCardapio">Id do cardápio ao qual os produtos pertencem</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ProdutoAdicional/Listar/{idLoja}")]
        public HttpResponseMessage ListarProdutos(int idLoja)
        {
            try
            {
                IList<DadosProdutoAdicional> produtosAdicionais = produtoAdicionalDAO.Listar(idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, produtosAdicionais);
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
