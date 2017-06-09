using System;

namespace ms_crud_rest.Exceptions
{
    [Serializable()]
    public class ParceiroNaoCadastradoException : System.Exception
    {
        public ParceiroNaoCadastradoException() : base() { }
        public ParceiroNaoCadastradoException(string message) : base(message) { }
        public ParceiroNaoCadastradoException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ParceiroNaoCadastradoException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}