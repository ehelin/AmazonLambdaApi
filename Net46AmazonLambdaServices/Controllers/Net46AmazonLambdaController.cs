using System;
using System.Web.Http;
using Shared;
using Shared.Interfaces;
using AmazonLambdaService;

namespace Net46AmazonLambdaServices.Controllers
{
    public class Net46AmazonLambdaController : ApiController
    {
        public string Get(string satelliteId)
        {
            string satelliteUpdate = String.Empty;
            DateTime localNow = DateTime.UtcNow;
            string date = localNow.ToString("yyyyMMddTHHmmssZ");
            string stringToSignDate = localNow.ToString("yyyyMMdd");

            //TODO - html encode the id
            string url = Constants.url + satelliteId;

            IService serv = new AmazonLambdaServiceImpl(Constants.host,
                                                        date,
                                                        stringToSignDate,
                                                        Constants.verb,
                                                        Constants.subUrl,
                                                        Constants.algorithm,
                                                        Constants.region,
                                                        Constants.service,
                                                        Constants.urlSuffix,
                                                        Constants.accessKey,
                                                        Constants.secretKey,
                                                        Constants.token,
                                                        Constants.contentType,
                                                        url,
                                                        false,
                                                        satelliteId);
            satelliteUpdate = serv.GetSatellite();

            if (String.IsNullOrEmpty(satelliteUpdate))
                satelliteUpdate = "GET object is null";

            return satelliteUpdate;
        }
    }
}
