using System;

namespace ms_crud_rest.Exceptions
{
    [Serializable()]
    public class PedidoNaoCadastradoClienteException : System.Exception
    {
        public PedidoNaoCadastradoClienteException() : base() { }
        public PedidoNaoCadastradoClienteException(string message) : base(message) { }
        public PedidoNaoCadastradoClienteException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected PedidoNaoCadastradoClienteException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}