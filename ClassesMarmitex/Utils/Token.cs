using System;

namespace ClassesMarmitex.Utils
{
    public class Token
    {
        public string GetToken(string urlBase)
        {
            var oAuth = new Manager();

            // the URL to obtain a temporary "request token"
            var rtUrl = urlBase + "/Token";
            oAuth["username"] = "MaRmITEXuSerREST";
            oAuth["password"] = "ConseguiConfigurarOToken090420171008654";
            oAuth["grant_type"] = "password";
            OAuthResponse response = oAuth.AcquireRequestToken(rtUrl, "POST");

            return response.AllText;

        }
    }
}
