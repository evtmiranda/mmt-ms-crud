using System;

namespace ms_crud_rest.Exceptions
{
    public class UsuarioNaoAutenticadoException : Exception
    {
    }

    public class EmpresaNaoEncontradaException : Exception
    {
        public override string Message
        {
            get
            {
                return "empresa não encontrada. por favor, verifique se o código da empresa está correto";
            }
        }
    }

    public class CadastroNaoRealizadoClienteException : Exception
    {
        public override string Message
        {
            get
            {
                return "não foi possível realizar o cadastro. por favor, tente novamente";
            }
        }
    }

    public class UsuarioJaExisteException : Exception
    {
        public override string Message
        {
            get
            {
                return "já existe um usuário com este e-mail. clique no link abaixo para fazer login";
            }
        }
    }

    public class NenhumCardapioEncontradoException : Exception
    {
        public override string Message
        {
            get
            {
                return "nenhum cardápio encontrado";
            }
        }
    }

    public class PagamentoNaoEncontradoException : Exception
    {
        public override string Message
        {
            get
            {
                return "nenhuma forma de pagamento encontrada";
            }
        }
    }

    public class HorarioNaoEncontradoException : Exception
    {
        public override string Message
        {
            get
            {
                return "nenhum horário de entrega encontrado";
            }
        }
    }


    public class PedidoNaoEncontradoException : Exception
    {
        public override string Message
        {
            get
            {
                return "pedido não encontrado";
            }
        }
    }

    public class PedidoNaoCadastradoClienteException : Exception
    {
        public override string Message
        {
            get
            {
                return "o pedido não pôde ser finalizado. por favor, realize o pedido novamente";
            }
        }
    }

    public class ClienteNuncaFezPedidosException : Exception
    {
        public override string Message
        {
            get
            {
                return "nenhum pedido encontrado =/";
            }
        }
    }

    public class LojaNaoPossuiPedidosException : Exception
    {
        public override string Message
        {
            get
            {
                return "nenhum pedido encontrado =/";
            }
        }
    }


    public class LojaNaoEncontradaException : Exception
    {
        public override string Message
        {
            get
            {
                return "nenhuma loja encontrada";
            }
        }
    }

    public class ParceiroNaoEncontradoException : Exception
    {
        public override string Message
        {
            get
            {
                return "parceiro nãoa encontrado";
            }
        }
    }

    public class ParceiroJaExisteException : Exception
    {
        public override string Message
        {
            get
            {
                return "já existe um parceiro com este nome";
            }
        }
    }

    public class LojaNaoPossuiParceirosException : Exception
    {
        public override string Message
        {
            get
            {
                return "nenhum parceiro encontrado =/";
            }
        }
    }
}
