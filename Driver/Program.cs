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
            string host = "";
            string date = "";
            string stringToSignDate = "";
            string verb = "";
            string subUrl = "";
            string algorithm = "";
            string region = "";
            string service = "";
            string urlSuffix = "";
            string accessKey = "";
            string secretKey = "";
            string token = "";
            string contentType = "";
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
            string token = "";
            string contentType = "";
            string host = "";
            string accessKey = "";
            string secretKey = "";
            string region = "";
            string service = "";
            string url = "";
            DateTime localNow = DateTime.UtcNow;
            string response = string.Empty;
            string date = localNow.ToString("yyyyMMddTHHmmssZ");
            string stringToSignDate = localNow.ToString("yyyyMMdd");
            string verb = "";
            string subUrl = "";
            string algorithm = "";
            string urlSuffix = "";

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
