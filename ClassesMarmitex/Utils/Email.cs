using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesMarmitex
{
    public class Email
    {
        private DadosRequisicaoRest retornoRequest;
        private RequisicoesREST rest;

        public Email()
        {
            this.rest = new RequisicoesREST();
        }

        public void EnviarEmailUnitario(DadosEnvioEmailUnitario dadosEmail)
        {
            string urlPostEmail = string.Format("/Email/EnviarEmailUnitario");

            //DadosEnvioEmailUnitario dadosEmail = new DadosEnvioEmailUnitario
            //{
            //    From = remetente,
            //    To = destinatario,
            //    Subject = assunto,
            //    Text = texto
            //};

            retornoRequest = rest.Post(urlPostEmail, dadosEmail);
        }
    }
}
