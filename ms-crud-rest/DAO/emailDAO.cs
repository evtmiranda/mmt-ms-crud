using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassesMarmitex;
using RestSharp;
using RestSharp.Authenticators;

namespace ms_crud_rest.DAO
{
    public class EmailDAO
    {
        public IRestResponse EnviarEmailUnitario(DadosEnvioEmailUnitario emailUnitario)
        {
            RestClient client = new RestClient()
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator =
            new HttpBasicAuthenticator("api",
                                      "")
            };

            RestRequest request = new RestRequest();
            request.AddParameter("domain", "", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", emailUnitario.From.Trim());
            request.AddParameter("to", emailUnitario.To);
            request.AddParameter("subject", emailUnitario.Subject);
            request.AddParameter("html", emailUnitario.Text);
            request.Method = Method.POST;

            return client.Execute(request);
        }
    }
}
