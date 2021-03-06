﻿using System;
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
            string satelliteUpdateId = "";

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
                                                        true,
                                                        satelliteUpdateId);
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
            string satelliteUpdateId = "";

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
                                                        false,
                                                        satelliteUpdateId);
            string satelliteUpdate = serv.GetSatellite();

            if (String.IsNullOrEmpty(satelliteUpdate))
                throw new Exception("GET object is null");

        }
    }
}
