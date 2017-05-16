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
        /// Busca um produto adicional por id
        /// </summary>
        /// <param name="id">id do produto adicional</param>
        /// <returns></returns>
        [Route("api/ProdutoAdicional/{id}")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                DadosProdutoAdicional produtoAdicional = produtoAdicionalDAO.BuscarPorId(id);
                if (produtoAdicional == null)
                    throw new KeyNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, produtoAdicional);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("O produto adicional {0} não foi encontrado", id);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
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

        /// <summary>
        /// Inativa um produto adicional
        /// </summary>
        /// <param name="produtoAdicional">produto adicional que será atualizado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ProdutoAdicional/Excluir")]
        public HttpResponseMessage Excluir([FromBody] DadosProdutoAdicional produtoAdicional)
        {
            try
            {
                produtoAdicionalDAO.Excluir(produtoAdicional);

                return Request.CreateResponse(HttpStatusCode.OK, produtoAdicional);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir o produto adicional. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Atualiza os dados de um produto adicional
        /// </summary>
        /// <param name="produtoAdicional">produto adicional que será atualizado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ProdutoAdicional/Atualizar")]
        public HttpResponseMessage AtualizarProduto([FromBody] DadosProdutoAdicional produtoAdicional)
        {
            try
            {
                produtoAdicionalDAO.Atualizar(produtoAdicional);

                return Request.CreateResponse(HttpStatusCode.OK, produtoAdicional);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o produto adicional. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #region item produto adicional

        [Route("api/ProdutoAdicional/AdicionarItem")]
        public HttpResponseMessage AdicionarItem([FromBody] DadosProdutoAdicionalItem item)
        {
            try
            {
                produtoAdicionalDAO.AdicionarItem(item);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                return response;
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o item. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/ProdutoAdicional/ExcluirItem")]
        public HttpResponseMessage ExcluirItem([FromBody] DadosProdutoAdicionalItem item)
        {
            try
            {
                produtoAdicionalDAO.ExcluirItem(item);

                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir o item. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/ProdutoAdicional/Item/{id}")]
        public HttpResponseMessage BuscarItem(int id)
        {
            try
            {
                DadosProdutoAdicionalItem item = produtoAdicionalDAO.BuscarItemPorId(id);
                if (item == null)
                    throw new KeyNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("O item {0} não foi encontrado", id);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
        }

        [HttpPost]
        [Route("api/ProdutoAdicional/AtualizarItem")]
        public HttpResponseMessage AtualizarItem([FromBody] DadosProdutoAdicionalItem item)
        {
            try
            {
                produtoAdicionalDAO.AtualizarItem(item);

                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o item. Por favor, tente novamente ou entre em contato com o administrador do sistema";
                HttpError error = new HttpError(mensagem);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #endregion
    }
}
