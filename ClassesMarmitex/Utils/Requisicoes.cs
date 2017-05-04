using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace ClassesMarmitex
{
    //public class Requisicoes
    //{
    //    private RequisicoesREST rest;

    //    //construtor da classe recebe um RequisicoesREST
    //    //O Ninject é o responsável por cuidar da criação de todos esses objetos
    //    public Requisicoes(RequisicoesREST rest)
    //    {
    //        this.rest = rest;
    //    }

    //    public List<MenuCardapio> ListarMenuCardapio(int idLoja)
    //    {
    //        DadosRequisicaoRest retornoGet = new DadosRequisicaoRest();

    //        List<MenuCardapio> listaMenuCardapio;

    //        retornoGet = rest.Get("/menucardapio/listar/" + idLoja);

    //        if (retornoGet.HttpStatusCode != HttpStatusCode.OK)
    //            return null;

    //        string json = retornoGet.objeto.ToString();

    //        listaMenuCardapio = JsonConvert.DeserializeObject<List<MenuCardapio>>(json);

    //        return listaMenuCardapio;
    //    }

    //    public List<Produto> ListarProdutos()
    //    {
    //        DadosRequisicaoRest retornoGet = new DadosRequisicaoRest();

    //        List<Produto> listaProdutos;

    //        retornoGet = rest.Get("/produto/listar");

    //        if (retornoGet.HttpStatusCode != HttpStatusCode.OK)
    //            return null;

    //        string json = retornoGet.objeto.ToString();

    //        listaProdutos = JsonConvert.DeserializeObject<List<Produto>>(json);

    //        return listaProdutos;
    //    }

    //    public List<FormaDePagamento> ListarFormasPagamento(int idLoja)
    //    {
    //        DadosRequisicaoRest retornoGet = new DadosRequisicaoRest();

    //        List<FormaDePagamento> listaFormaPagamento;

    //        retornoGet = rest.Get("/formaPagamento/listar/" + idLoja);

    //        if (retornoGet.HttpStatusCode != HttpStatusCode.OK)
    //            return null;

    //        string json = retornoGet.objeto.ToString();

    //        listaFormaPagamento = JsonConvert.DeserializeObject<List<FormaDePagamento>>(json);

    //        return listaFormaPagamento;
    //    }
    //}
}
