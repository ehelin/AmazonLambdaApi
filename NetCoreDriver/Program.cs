using Shared;
using Shared.Dto;
using Service;

namespace TestDriver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunTests();
        }
        
        private static void RunTests()
        {
            System.Threading.Thread.Sleep(10000);  //wait for .net 4.6 API to start
            TestSatelliteGet();
        }

        private static void TestSatelliteGet()
        {
            string url = "http://localhost:56571/api/Net46AmazonLambda?satelliteId=5828";

            IService serv = new ServiceImpl();
            string update = serv.GetSatellite(url);

            if (string.IsNullOrEmpty(update))
                throw new System.Exception("GET object is null");
        }
    }
}
