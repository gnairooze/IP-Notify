using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_Notify
{
    static class PublicIPManager
    {
        public static string GetPublicIP(Serilog.Core.Logger logger, string publicIPProviderUrl)
        {
            if (string.IsNullOrEmpty(publicIPProviderUrl))
            {
                logger.Error("public IP provider URL is empty");
                return string.Empty;
            }

            logger.Debug("public IP provider URL is {0}", publicIPProviderUrl);

            var client = new RestClient(publicIPProviderUrl);

            var response = client.Execute(new RestRequest());

            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                logger.Error("cannot retrieve public IP. Status Code {0}. Response content is {1}", response.StatusCode, response.Content);
            }

            return response.Content;
        }
    }
}
