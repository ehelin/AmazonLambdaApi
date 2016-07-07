using System;
using Shared.Dto;
using Shared.Interfaces;
using AmazonAuthentication;
using System.Net;
using Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AmazonLambdaService
{
    public class AmazonLambdaServiceImpl : IService
    {
        private IAuthentication authentication = null;
        private string host = string.Empty;
        private string date = string.Empty;
        private string contentType = string.Empty;
        private string token = string.Empty;
        private string url = string.Empty;
        private Dictionary<string, string> headers = null;
        private Dictionary<string, string> parameters = null;

        public AmazonLambdaServiceImpl(string host,
                                        string date,
                                        string stringToSignDate,
                                        string verb,
                                        string subUrl,
                                        string algorithm,
                                        string region,
                                        string service,
                                        string urlSuffix,
                                        string accessKey,
                                        string secretKey,
                                        string token,
                                        string contentType,
                                        string url,
                                        bool test)
        {
            //regular headers
            this.token = token;
            this.contentType = contentType;
            this.token = token;
            this.date = date;
            this.host = host;
            this.url = url;

            if (test)
            {
                this.headers = this.GetHeadersTestSuite();
                this.parameters = this.GetParametersTestSuite();
            }
            else
            {
                this.headers = GetHeaders();
                this.parameters = GetParameters();
            }

            authentication = new AmazonAuthenticationImpl(host,
                                                          date,
                                                          stringToSignDate,
                                                          verb,
                                                          subUrl,
                                                          algorithm,
                                                          region,
                                                          service,
                                                          urlSuffix,
                                                          accessKey,
                                                          secretKey,
                                                          token,
                                                          contentType,
                                                          parameters,
                                                          headers);

            string authorization = this.authentication.GetAuthorization();
            headers.Add("Authorization", authorization);
        }
        
        public bool TestAwsSignatureCode()
        {
            bool works = false;

            foreach (KeyValuePair<string, string> header in headers)
            {
                if (header.Key.ToLower() == "authorization")
                {
                    if (header.Value == Constants.TEST_SUITE_AUTHORIZATION)
                        works = true;

                    break;
                }
            }

            return works;
        }
        public SatelliteUpdate GetSatellite()
        {
            SatelliteUpdate satelliteUpdate = null;

            string getWebClientResult = GET_WebClient();
            string getHttpClientResult1 = GET_HttpClient_HdrMethod1().Result;
            string getHttpClientResult2 = GET_HttpClient_HdrMethod2().Result;

            //TODO - convert result to a satelliteupdate or clientupdate
            satelliteUpdate = new SatelliteUpdate();

            return satelliteUpdate;
        }
        public bool DeleteSatelliteUpdate(int id)
        {
            throw new NotImplementedException();
        }
        public bool InsertSatelliteUpdate(SatelliteUpdate update)
        {
            throw new NotImplementedException();
        }
        public bool UpdateSatelliteUpdate(SatelliteUpdate update)
        {
            throw new NotImplementedException();
        }

        #region HttpCalls

        private string GET_WebClient()
        {
            string response = string.Empty;

            try
            {
                WebClient client = new WebClient();
                Uri uri = new Uri(url);

                foreach (KeyValuePair<string, string> header in headers)
                    client.Headers.Add(header.Key, header.Value);

                response = client.DownloadString(uri);
            }
            catch (Exception e)
            {
                response = e.Message;
            }

            return response;
        }
        private async Task<string> GET_HttpClient_HdrMethod1()
        {
            string response = string.Empty;
            HttpClient client = new HttpClient();

            try
            {
                client.Timeout = new TimeSpan(1, 0, 0);

                foreach (KeyValuePair<string, string> header in headers)
                {
                    if (header.Key.ToLower().IndexOf("content") != -1)
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.Value));
                    else if (header.Key.ToLower().IndexOf("authorization") != -1)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header.Key, header.Value);
                    else
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                HttpResponseMessage httpResponseMsg = await client.GetAsync(url);
                response = await httpResponseMsg.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                response = e.Message;
            }

            return response;
        }
        private async Task<string> GET_HttpClient_HdrMethod2()
        {
            string response = string.Empty;
            HttpClient client = new HttpClient();

            try
            {
                client.Timeout = new TimeSpan(1, 0, 0);
                
                foreach (KeyValuePair<string, string> header in headers)
                   client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

                HttpResponseMessage httpResponseMsg = await client.GetAsync(url);
                response = await httpResponseMsg.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                response = e.Message;
            }

            return response;
        }

        #endregion

        #region Shared 
        
        private Dictionary<string, string> GetHeadersTestSuite()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
 
            headers.Add("Host", this.host);
            headers.Add("X-Amz-Date", this.date);

            return headers;
        }
        private Dictionary<string, string> GetHeaders()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();

            headers.Add("Content-Type", this.contentType);
            headers.Add("Host", this.host);
            headers.Add("X-Amz-Date", this.date);
            headers.Add("x-api-key", this.token);

            return headers;
        }

        //hard coded for ease and since it won't change anytime soon
        private Dictionary<string, string> GetParametersTestSuite()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("Param1", "value1");
            parameters.Add("Param2", "value2");

            return parameters;
        }
        //hard coded for ease and since it won't change anytime soon
        private Dictionary<string, string> GetParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("SatelliteUpdateId", "5828");

            return parameters;
        }

        #endregion
    }
}
