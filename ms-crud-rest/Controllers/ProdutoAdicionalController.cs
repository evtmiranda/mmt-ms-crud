using ClassesMarmitex;
using ms_crud_rest.DAO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassesMarmitex.Utils;

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
        [Route("api/ProdutoAdicional/{id}/{idLoja}")]
        public HttpResponseMessage Get(int id, int idLoja)
        {
            try
            {
                DadosProdutoAdicional produtoAdicional = produtoAdicionalDAO.BuscarPorId(id, idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, produtoAdicional);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível buscar o produto adicional. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
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
                string mensagem = "Não foi possível buscar os produtos adicionais. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
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
            catch (Exception)
            {
                string mensagem = "Não foi possível adicionar o produto adicional. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Exclui um produto adicional
        /// </summary>
        /// <param name="produtoAdicional">produto adicional que será excluido</param>
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
                string mensagem = "Não foi possível excluir o produto adicional. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Desativa um produto adicional
        /// </summary>
        /// <param name="produtoAdicional">produto adicional que será desativado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ProdutoAdicional/Desativar")]
        public HttpResponseMessage Desativar([FromBody] DadosProdutoAdicional produtoAdicional)
        {
            try
            {
                produtoAdicionalDAO.Desativar(produtoAdicional);

                return Request.CreateResponse(HttpStatusCode.OK, produtoAdicional);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível desativar o produto adicional. Por favor, tente novamente ou entre em contato com nosso suporte.";
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
                string mensagem = "Não foi possível atualizar o produto adicional. Por favor, tente novamente ou entre em contato com nosso suporte.";
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
            catch (Exception)
            {
                string mensagem = "Não foi possível adicionar o item para o produto adicional. Por favor, tente novamente ou entre em contato com nosso suporte.";
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
                string mensagem = "Não foi possível excluir o item. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpPost]
        [Route("api/ProdutoAdicional/DesativarItem")]
        public HttpResponseMessage DesativarItem([FromBody] DadosProdutoAdicionalItem item)
        {
            try
            {
                produtoAdicionalDAO.DesativarItem(item);

                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível desativar o item. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [HttpGet]
        [Route("api/ProdutoAdicional/Item/{id}/{idLoja}")]
        public HttpResponseMessage BuscarItem(int id, int idLoja)
        {
            try
            {
                DadosProdutoAdicionalItem item = produtoAdicionalDAO.BuscarItemPorId(id, idLoja);

                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (KeyNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível buscar o item. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
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
                string mensagem = "Não foi possível atualizar o item. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #endregion
    }
}
