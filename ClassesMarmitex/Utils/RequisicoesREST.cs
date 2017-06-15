﻿using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace ClassesMarmitex
{
    public class RequisicoesREST
    {
        public string urlBase = "http://localhost:29783/";

        public DadosRequisicaoRest Post(string recurso, object objeto = null)
        {
            DadosRequisicaoRest retorno = new DadosRequisicaoRest();
            string conteudo = "";
            string json = "";

            //faz o post de um objeto em um determinado recurso
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlBase + "/api/" + recurso);
                request.Method = "POST";
                request.Accept = "application/json";

                json = JsonConvert.SerializeObject(objeto);

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
                try
                {
                    string mensagemErro = "";

                    try
                    {
                        mensagemErro = new StreamReader(wEx.Response.GetResponseStream()).ReadToEnd();
                    }
                    catch
                    {
                        mensagemErro = "";
                    }

                    //cria um webResponse
                    var webResponse = wEx.Response as HttpWebResponse;

                    //verifica se não é erro do protocolo HTTP. Se não for, devolve um InternalServerError
                    if (wEx.Status != WebExceptionStatus.ProtocolError)
                    {
                        retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                        retorno.objeto = mensagemErro;

                        return retorno;
                    }

                    //Retorna o status HTTP
                    retorno.HttpStatusCode = webResponse.StatusCode;
                    retorno.objeto = mensagemErro;

                    return retorno;
                }
                catch (System.Exception)
                {
                    retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                    retorno.objeto = "";

                    return retorno;
                }
            }
            //Se ocorrer qualquer outra exceção retorna um InternalServerError
            catch (System.Exception ex)
            {
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.objeto = ex.Message ?? "";

                return retorno;
            }
        }

        public DadosRequisicaoRest Get(string recurso, int id = 0)
        {
            DadosRequisicaoRest retorno = new DadosRequisicaoRest();
            string conteudo;

            //faz o get de um objeto em um determinado recurso
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlBase + "/api/" + recurso);

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
                try
                {
                    string mensagemErro = "";

                    try
                    {
                        mensagemErro = new StreamReader(wEx.Response.GetResponseStream()).ReadToEnd();
                    }
                    catch
                    {
                        mensagemErro = "";
                    }

                    //cria um webResponse
                    var webResponse = wEx.Response as HttpWebResponse;

                    //verifica se não é erro do protocolo HTTP. Se não for, devolve um InternalServerError
                    if (wEx.Status != WebExceptionStatus.ProtocolError)
                    {
                        retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                        retorno.objeto = mensagemErro;

                        return retorno;
                    }

                    //Retorna o status HTTP
                    retorno.HttpStatusCode = webResponse.StatusCode;
                    retorno.objeto = mensagemErro;

                    return retorno;
                }
                catch (System.Exception)
                {
                    retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                    retorno.objeto = "";

                    return retorno;
                }
            }
            //Se ocorrer qualquer outra exceção retorna um InternalServerError
            catch (System.Exception ex)
            {
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.objeto = ex.Message ?? "";

                return retorno;
            }
        }

        public DadosRequisicaoRest Patch(string recurso, object objeto)
        {
            DadosRequisicaoRest retorno = new DadosRequisicaoRest();
            string conteudo = "";

            //faz o post de um objeto em um determinado recurso
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlBase + "/api/" + recurso);
                request.Method = "PATCH";
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
                try
                {
                    string mensagemErro = "";

                    try
                    {
                        mensagemErro = new StreamReader(wEx.Response.GetResponseStream()).ReadToEnd();
                    }
                    catch
                    {
                        mensagemErro = "";
                    }

                    //cria um webResponse
                    var webResponse = wEx.Response as HttpWebResponse;

                    //verifica se não é erro do protocolo HTTP. Se não for, devolve um InternalServerError
                    if (wEx.Status != WebExceptionStatus.ProtocolError)
                    {
                        retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                        retorno.objeto = mensagemErro;

                        return retorno;
                    }

                    //Retorna o status HTTP
                    retorno.HttpStatusCode = webResponse.StatusCode;
                    retorno.objeto = mensagemErro;

                    return retorno;
                }
                catch (System.Exception)
                {
                    retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                    retorno.objeto = "";

                    return retorno;
                }

            }
            //Se ocorrer qualquer outra exceção retorna um InternalServerError
            catch (System.Exception ex)
            {
                retorno.HttpStatusCode = HttpStatusCode.InternalServerError;
                retorno.objeto = ex.Message ?? "";

                return retorno;
            }
        }
    }
}
