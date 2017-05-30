using ClassesMarmitex;
using ms_crud_rest.DAO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ms_crud_rest.Controllers
{
    public class ProdutoController : ApiController
    {
        private ProdutoDAO produtoDAO;
        private LogDAO logDAO;

        //construtor do controller, recebe um produtoDAO e um logDAO, que por sua vez recebe uma ISession.
        //O Ninject é o responsável por cuidar da criação de todos esses objetos
        public ProdutoController(ProdutoDAO produtoDAO, LogDAO logDAO)
        {
            this.produtoDAO = produtoDAO;
            this.logDAO = logDAO;
        }

        /// <summary>
        /// Busca um produto por id
        /// </summary>
        /// <param name="id">id do produto</param>
        /// <returns></returns>
        [Route("api/Produto/{id}/{idLoja}")]
        public HttpResponseMessage Get(int id, int idLoja)
        {
            try
            {
                Produto produto = produtoDAO.BuscarPorId(id, idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, produto);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível buscar o produto. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        [Route("api/Produto/Adicionar")]
        public HttpResponseMessage Post([FromBody] Produto produto)
        {
            try
            {
                produtoDAO.Adicionar(produto);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível adicionar o produto. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Inativa um produto
        /// </summary>
        /// <param name="produto">produto que será atualizado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Produto/Excluir")]
        public HttpResponseMessage ExcluirProduto([FromBody] Produto produto)
        {
            try
            {
                produtoDAO.Excluir(produto);
                return Request.CreateResponse(HttpStatusCode.OK, produto);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível excluir o produto. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Desativa um produto
        /// </summary>
        /// <param name="produto">produto que será atualizado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Produto/Desativar")]
        public HttpResponseMessage DesativarProduto([FromBody] Produto produto)
        {
            try
            {
                produtoDAO.Desativar(produto);
                return Request.CreateResponse(HttpStatusCode.OK, produto);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível desativar o produto. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Atualiza os dados de um produto
        /// </summary>
        /// <param name="produto">produto que será atualizado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Produto/Atualizar")]
        public HttpResponseMessage AtualizarProduto([FromBody] Produto produto)
        {
            try
            {
                produtoDAO.Atualizar(produto);

                return Request.CreateResponse(HttpStatusCode.OK, produto);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o produto. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Retorna todos os produtos existentes dentro do cardápio enviado como parâmetro
        /// </summary>
        /// <param name="idMenuCardapio">Id do cardápio ao qual os produtos pertencem</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Produto/Listar/{idMenuCardapio}/{idLoja}")]
        public HttpResponseMessage ListarProdutos(int idMenuCardapio, int idLoja)
        {
            try
            {
                IList<Produto> produtos = produtoDAO.Listar(idMenuCardapio, idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, produtos);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível buscar os produtos. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }


        #region produtos adicionais do produto

        [Route("api/Produto/AdicionarProdutoAdicional")]
        public HttpResponseMessage AdicionarProdutoAdicional([FromBody] DadosProdutoAdicionalProduto produtoAdicionalProduto)
        {
            try
            {
                produtoDAO.AdicionarProdutoAdicional(produtoAdicionalProduto);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível adicionar o produto adicional. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }


        /// <summary>
        /// Retorna todos os produtos adicionais de um determinado produto
        /// </summary>
        /// <param name="idProduto">Id do produto ao qual os produtos adicionais devem ser consultados</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Produto/BuscarProdutosAdicionaisDeUmProduto/{idProduto}/{idLoja}")]
        public HttpResponseMessage BuscarProdutosAdicionaisDeUmProduto(int idProduto, int idLoja)
        {
            try
            {
                List<DadosProdutoAdicionalProduto> produtosAdicionaisProduto = produtoDAO.BuscarProdutosAdicionaisDeUmProduto(idProduto, idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, produtosAdicionaisProduto);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível buscar os produtos adicionais. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Retorna um produto adicional
        /// </summary>
        /// <param name="idProdutoAdicionalProduto">Id do produto ao qual os produtos adicionais devem ser consultados</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Produto/BuscarProdutoAdicional/{idProdutoAdicionalProduto}/{idLoja}")]
        public HttpResponseMessage BuscarProdutoAdicional(int idProdutoAdicionalProduto, int idLoja)
        {
            try
            {
                DadosProdutoAdicionalProduto produtoAdicionalProduto = produtoDAO.BuscarProdutoAdicionalDeUmProduto(idProdutoAdicionalProduto, idLoja);
                return Request.CreateResponse(HttpStatusCode.OK, produtoAdicionalProduto);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível buscar o produto adicional. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        /// <summary>
        /// Inativa um produto adicional do produto
        /// </summary>
        /// <param name="produtoAdicional">produto que será atualizado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Produto/ExcluirProdutoAdicional")]
        public HttpResponseMessage ExcluirProdutoAdicional([FromBody] DadosProdutoAdicionalProduto produtoAdicional)
        {
            try
            {
                produtoDAO.ExcluirProdutoAdicional(produtoAdicional);
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
        /// Atualiza os dados de um produto adicional do produto
        /// </summary>
        /// <param name="produto">produto que será atualizado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Produto/ProdutoAdicional/Atualizar")]
        public HttpResponseMessage AtualizarProdutoAdicionalProduto([FromBody] DadosProdutoAdicionalProduto dadosProdutoAdicionalProduto)
        {
            try
            {
                produtoDAO.AtualizarProdutoAdicionalProduto(dadosProdutoAdicionalProduto);
                return Request.CreateResponse(HttpStatusCode.OK, dadosProdutoAdicionalProduto);
            }
            catch (Exception)
            {
                string mensagem = "Não foi possível atualizar o produto adicional. Por favor, tente novamente ou entre em contato com nosso suporte.";
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #endregion

    }
}
