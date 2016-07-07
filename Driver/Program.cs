using System;
using Shared.Dto;
using Shared.Interfaces;
using AmazonLambdaService;

namespace Driver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunTests();
        }

        private static void RunTests()
        {
            TestAwsSampleGetCall();
            TestSatelliteGet();
        }
        
        private static void TestAwsSampleGetCall()
        {
            string host = "example.amazonaws.com";
            string date = "20150830T123600Z";
            string stringToSignDate = "20150830";
            string verb = "GET";
            string subUrl = "/";
            string algorithm = "AWS4-HMAC-SHA256";
            string region = "us-east-1";
            string service = "service";
            string urlSuffix = "aws4_request";
            string accessKey = "AKIDEXAMPLE";
            string secretKey = "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY";
            string token = "3qKXOJPm3ma9MlD7zTYje7HuK4asp1yt4OMiomv4";
            string contentType = "application/json";
            string url = "";

            IService serv = new AmazonLambdaServiceImpl(host,
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
                                                        url, 
                                                        true);
            bool result = serv.TestAwsSignatureCode();

            if (!result)
                throw new System.Exception("Aws Test Suite results failed this code");

        }

        private static void TestSatelliteGet()
        {
            string token = "3qKXOJPm3ma9MlD7zTYje7HuK4asp1yt4OMiomv4";
            string contentType = "application/json";
            string host = "n8picmhn99.execute-api.us-west-2.amazonaws.com";
            string accessKey = "AKIAJPWEP3MHIUVO2G2A";
            string secretKey = "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";
            string region = "us-west-2";
            string service = "execute-api";
            string url = "https://n8picmhn99.execute-api.us-west-2.amazonaws.com/test/satellite?SatelliteUpdateId=5828";
            DateTime localNow = DateTime.UtcNow;
            string response = string.Empty;
            string date = localNow.ToString("yyyyMMddTHHmmssZ");
            string stringToSignDate = localNow.ToString("yyyyMMdd");
            string verb = "GET";
            string subUrl = "/test/satellite";
            string algorithm = "AWS4-HMAC-SHA256";
            string urlSuffix = "aws4_request";

            IService serv = new AmazonLambdaServiceImpl(host,
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
                                                        url, 
                                                        false);
            SatelliteUpdate update = serv.GetSatellite();

            if (update == null)
                throw new System.Exception("GET object is null");
        }
    }
}
