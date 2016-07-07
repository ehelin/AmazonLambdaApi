using System;
using Shared.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace AmazonAuthentication
{
    public class AmazonAuthenticationImpl : IAuthentication
    {
        private string host = string.Empty;
        private string date = string.Empty;
        private string stringToSignDate = string.Empty;
        private string verb = string.Empty;
        private string subUrl = string.Empty;
        private string algorithm = string.Empty;
        private string region = string.Empty;
        private string service = string.Empty;
        private string urlSuffix = string.Empty;
        private string accessKey = string.Empty;
        private string secretKey = string.Empty;
        private string contentType = string.Empty;
        private string token = string.Empty;
        private Dictionary<string, string> parameters = null;
        private Dictionary<string, string> headers = null;

        public AmazonAuthenticationImpl(string host,
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
                                        Dictionary<string, string> parameters,
                                        Dictionary<string, string> headers)
        {
            this.host = host;
            this.date = date;
            this.stringToSignDate = stringToSignDate;
            this.verb = verb;
            this.subUrl = subUrl;
            this.algorithm = algorithm;
            this.region = region;
            this.service = service;
            this.urlSuffix = urlSuffix;
            this.accessKey = accessKey;
            this.secretKey = secretKey;
            this.token = token;
            this.contentType = contentType;
            this.parameters = parameters;
            this.headers = headers;
        }

        public string GetAuthorization()
        {
            string authorization = string.Empty;

            string canonicalRequest = CreateCanonicalRequest(verb, subUrl, parameters, headers);

            string stringToSign = CreateStringToSign(algorithm, headers, canonicalRequest, stringToSignDate, region, service, urlSuffix);

            string signature = CreateAwsSignature(secretKey, stringToSignDate, region, service, stringToSign, algorithm);

            authorization = CreateAwsAuthorization(algorithm, accessKey, stringToSignDate, region, service, urlSuffix, signature, headers);

            return authorization;
        }

        public string CreateCanonicalRequest(string verb,
                                             string subUrl,
                                             Dictionary<string, string> parameters,
                                             Dictionary<string, string> headers)
        {
            string canonicalRequest = string.Empty;

            string parameterList = string.Empty;
            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                parameterList += parameter.Key + "=" + parameter.Value + "&";
            }
            parameterList = parameterList.Trim('&');

            string headerList = string.Empty;
            string signedHeaderList = string.Empty;
            foreach (KeyValuePair<string, string> header in headers)
            {
                if (header.Key.ToLower().IndexOf("content") != -1)
                {
                    headerList += header.Key.ToLower() + ":\n";
                    signedHeaderList += header.Key.ToLower() + ";";
                }
                else
                {
                    headerList += header.Key.ToLower() + ":" + header.Value + "\n";
                    signedHeaderList += header.Key.ToLower() + ";";
                }
            }
            signedHeaderList = signedHeaderList.Trim(';');

            string body = "";
            byte[] bytes = SHA256GetHash(body);
            string payload = HexEncode(bytes);

            canonicalRequest = verb + "\n"
                + subUrl + "\n"
                + parameterList + "\n"
                + headerList + "\n"
                + signedHeaderList + "\n"
                + payload;

            return canonicalRequest;
        }

        public string CreateStringToSign(string algorithm,
                                         Dictionary<string, string> headers,
                                         string canonicalRequest,
                                         string stringToSignDate,
                                         string region,
                                         string service,
                                         string urlSuffix)
        {
            string stringToSign = string.Empty;

            byte[] bytes = SHA256GetHash(canonicalRequest);
            string hashedCR = HexEncode(bytes);

            string date = headers["X-Amz-Date"];

            stringToSign = algorithm + "\n"
                        + date + "\n"
                        + stringToSignDate + "/" + region + "/" + service + "/" + urlSuffix + "\n"
                        + hashedCR;

            return stringToSign;
        }

        private string CreateAwsSignature(string secretKey,
                                          string stringToSignDate,
                                          string regionName,
                                          string serviceName,
                                          string stringToSign,
                                          string algorithm)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + secretKey).ToCharArray());
            byte[] kDate = HmacSHA256(kSecret, stringToSignDate, algorithm);
            byte[] kRegion = HmacSHA256(kDate, regionName, algorithm);
            byte[] kService = HmacSHA256(kRegion, serviceName, algorithm);
            byte[] kSigning = HmacSHA256(kService, "aws4_request", algorithm);

            byte[] kSigningStringToSign = HmacSHA256(kSigning, stringToSign, algorithm);
            string signature = HexEncode(kSigningStringToSign);

            return signature;
        }

        private string CreateAwsAuthorization(string algorithm,
                                              string accessKey,
                                              string stringToSignDate,
                                              string region,
                                              string service,
                                              string urlSuffix,
                                              string signature,
                                              Dictionary<string, string> headers)
        {
            string authorization = string.Empty;

            string credential = " Credential=" + accessKey + "/"
                                + stringToSignDate + "/" + region + "/" + service + "/" + urlSuffix;

            string signedHeaderList = string.Empty;
            foreach (KeyValuePair<string, string> header in headers)
            {
                signedHeaderList += header.Key.ToLower() + ";";
            }
            signedHeaderList = signedHeaderList.Trim(';');

            authorization = algorithm + credential
                            + ", SignedHeaders=" + signedHeaderList
                            + ", Signature=" + signature;

            return authorization;
        }

        #region Methods borrowed from  http://stackoverflow.com/questions/32906755/aws-api-gateway-signature

        private string HexEncode(byte[] byteValues)
        {
            return BitConverter.ToString(byteValues).Replace("-", string.Empty).ToLowerInvariant();
        }
        private byte[] SHA256GetHash(String data)
        {
            byte[] byteValues = Encoding.UTF8.GetBytes(data.ToCharArray());

            return SHA256.Create().ComputeHash(byteValues);
        }
        private byte[] HmacSHA256(byte[] key, String data, string algorithm)
        {
            return new HMACSHA256(key).ComputeHash(ToBytes(data));
        }

        private static byte[] ToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str.ToCharArray());
        }

        #endregion
    }
}
