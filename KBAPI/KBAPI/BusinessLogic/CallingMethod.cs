using KBAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KBAPI.BusinessLogic
{
    public class CallingMethod
    {
        [HttpPost]
        public static async Task<string> post_method(string baseUrl, ParameterJSON common)
        {
            string result = "";
            try
            {
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    using (var httpClient = new HttpClient(httpClientHandler))
                    {
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpResponse = await httpClient.PostAsJsonAsync(baseUrl, common);
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            result = httpResponse.Content.ReadAsStringAsync().Result;
                        }
                        if (result == "")
                        {
                            result = "401 Unauthorized";
                        }
                    }
                }
                //}
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
