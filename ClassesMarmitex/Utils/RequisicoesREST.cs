﻿namespace ClassesMarmitex
{
    using Newtonsoft.Json;
    using System.IO;
    using System.Net;
    using System.Text;

    public class RequisicoesREST
    {
        public DadosRequisicaoRest Post(string recurso, object objeto)
        {
            DadosRequisicaoRest dadosPost = new DadosRequisicaoRest();
            string conteudo = "";

            //faz o post de um objeto em um determinado recurso
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:29783/api/" + recurso);
                request.Method = "POST";
                request.Accept = "application/json";

                string json = JsonConvert.SerializeObject(objeto);

                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                request.GetRequestStream().Write(jsonBytes, 0, jsonBytes.Length);

                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                dadosPost.HttpStatusCode = response.StatusCode;

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    conteudo = reader.ReadToEnd();
                }

                dadosPost.objeto = conteudo;

                return dadosPost;
            }
            //se for algum erro do protocolo HTTP, captura o retorno HTTP para utilizar no retorno do método
            catch (WebException wEx)
            {
                //cria um webResponse
                var webResponse = wEx.Response as System.Net.HttpWebResponse;

                //verifica se não é erro do protocolo HTTP. Se não for, devolve um InternalServerError
                if (wEx.Status != WebExceptionStatus.ProtocolError) {
                    dadosPost.HttpStatusCode = HttpStatusCode.InternalServerError;
                    dadosPost.objeto = wEx.Message != null ? wEx.Message : "";
                    return dadosPost;
                }

                //Retorna o status HTTP
                dadosPost.HttpStatusCode = webResponse.StatusCode;
                dadosPost.objeto = wEx.Message != null ? wEx.Message : "";

                return dadosPost;
            }
            //Se ocorrer qualquer outra exceção retorna um InternalServerError
            catch (System.Exception ex)
            {
                dadosPost.HttpStatusCode = HttpStatusCode.InternalServerError;
                dadosPost.objeto = ex.Message != null ? ex.Message : "";

                return dadosPost;
            }
        }

        public DadosRequisicaoRest Get(string recurso, int id = 0)
        {
            DadosRequisicaoRest retorno = new DadosRequisicaoRest();
            string conteudo;

            //faz o get de um objeto em um determinado recurso
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:29783/api/" + recurso);

                if (id != 0)
                    request.Headers.Add("id", id.ToString());

                request.Method = "GET";
                request.Accept = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                retorno.HttpStatusCode = response.StatusCode;

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    conteudo = reader.ReadToEnd();
                }

                retorno.objeto = conteudo;

                return retorno;
            }
            //se for algum erro do protocolo HTTP, captura o retorno HTTP para utilizar no retorno do método
            catch (WebException wEx)
            {
                //cria um webResponse
                var webResponse = wEx.Response as System.Net.HttpWebResponse;

                //verifica se não é erro do protocolo HTTP. Se não for, devolve um InternalServerError
                if (wEx.Status != WebExceptionStatus.ProtocolError)
                {
                    retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                    retorno.objeto = wEx.Message != null ? wEx.Message : "";
                    return retorno;
                }

                //Retorna o status HTTP
                retorno.HttpStatusCode = webResponse.StatusCode;
                retorno.objeto = wEx.Message != null ? wEx.Message : "";

                return retorno;
            }
            //Se ocorrer qualquer outra exceção retorna um InternalServerError
            catch (System.Exception ex)
            {
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.objeto = ex.Message != null ? ex.Message : "";

                return retorno;
            }
        }

        public DadosRequisicaoRest PostComRetorno(string recurso, object objeto)
        {
            DadosRequisicaoRest retorno = new DadosRequisicaoRest();

            string conteudo;

            //faz o get de um objeto em um determinado recurso
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:29783/api/" + recurso);
                request.Method = "POST";
                request.Accept = "application/json";

                string json = JsonConvert.SerializeObject(objeto);

                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                request.GetRequestStream().Write(jsonBytes, 0, jsonBytes.Length);

                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                retorno.HttpStatusCode = response.StatusCode;

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    conteudo = reader.ReadToEnd();
                }

                retorno.objeto = conteudo;

                return retorno;
            }
            //se for algum erro do protocolo HTTP, captura o retorno HTTP para utilizar no retorno do método
            catch (WebException wEx)
            {
                //cria um webResponse
                var webResponse = wEx.Response as System.Net.HttpWebResponse;

                //verifica se não é erro do protocolo HTTP. Se não for, devolve um InternalServerError
                if (wEx.Status != WebExceptionStatus.ProtocolError)
                {
                    retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                    retorno.objeto = wEx.Message != null ? wEx.Message : "";
                    return retorno;
                }

                //Retorna o status HTTP
                retorno.HttpStatusCode = webResponse.StatusCode;
                retorno.objeto = wEx.Message != null ? wEx.Message : "";

                return retorno;
            }
            //Se ocorrer qualquer outra exceção retorna um InternalServerError
            catch (System.Exception ex)
            {
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.objeto = ex.Message != null ? ex.Message : "";

                return retorno;
            }
        }
    }
}
