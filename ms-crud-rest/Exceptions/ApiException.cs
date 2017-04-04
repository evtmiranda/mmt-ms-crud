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
}
