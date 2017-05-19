using System;

namespace ms_crud_rest.Exceptions
{
    [Serializable()]
    public class UsuarioNaoAutenticadoException : System.Exception
    {
        public UsuarioNaoAutenticadoException() : base() { }
        public UsuarioNaoAutenticadoException(string message) : base(message) { }
        public UsuarioNaoAutenticadoException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected UsuarioNaoAutenticadoException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}