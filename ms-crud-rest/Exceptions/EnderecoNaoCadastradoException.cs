using System;

namespace ms_crud_rest.Exceptions
{
    [Serializable()]
    public class EnderecoNaoCadastradoException : System.Exception
    {
        public EnderecoNaoCadastradoException() : base() { }
        public EnderecoNaoCadastradoException(string message) : base(message) { }
        public EnderecoNaoCadastradoException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected EnderecoNaoCadastradoException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}