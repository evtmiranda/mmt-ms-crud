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
        [Route("api/Produto/{id}")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Produto produto = produtoDAO.BuscarPorId(id);
                if (produto == null)
                    throw new KeyNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, produto);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("O produto {0} não foi encontrado", id);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
        }

        public HttpResponseMessage Post([FromBody] Produto produto)
        {
            try
            {
                produtoDAO.Adicionar(produto);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                string location = Url.Link("DefaultApi", new { controller = "Produto", id = produto.Id });
                response.Headers.Location = new Uri(location);

                return response;
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel cadastrar o produto. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                //verifica se o id existe antes de excluir
                HttpResponseMessage retornoGet = Get(id);

                if (retornoGet.StatusCode == HttpStatusCode.OK)
                    produtoDAO.ExcluirPorId(id);
                else
                    return retornoGet;

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel deletar o produto. erro: {0}", ex);
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        public HttpResponseMessage Patch([FromBody] Produto produto, [FromUri] int id)
        {
            try
            {
                Produto produtoAtual = produtoDAO.BuscarPorId(id);

                if (produtoAtual == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                produtoAtual.Ativo = produto.Ativo;
                produtoAtual.Descricao = produto.Descricao;
                produtoAtual.IdMenuCardapio = produto.IdMenuCardapio;
                produtoAtual.Nome = produto.Nome;
                produtoAtual.Valor = produto.Valor;

                produtoDAO.Atualizar(produtoAtual);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string mensagem = string.Format("nao foi possivel atualizar o produto. erro: {0}", ex);
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
        [Route("api/Produto/Listar")]
        public HttpResponseMessage ListarProdutos(int idMenuCardapio)
        {
            try
            {
                IList<Produto> produtos = produtoDAO.Listar(idMenuCardapio);
                return Request.CreateResponse(HttpStatusCode.OK, produtos);
            }
            catch (Exception)
            {
                string mensagem = string.Format("ocorreu um problema ao buscar os produtos. por favor, tente atualizar a página ou acessar dentro de alguns minutos...");
                HttpError error = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }
        }



    }
}
