using System;
using Shared;
using System.Threading.Tasks;

//NOTE: I added this via browse...replace with Nuget package once you figure out where that is
//http://stackoverflow.com/questions/30304365/using-httpclient-in-asp-net-5-app
using System.Net.Http;
using System.Net.Http.Headers;

namespace Service
{
    public class ServiceImpl : IService
    {
        public string GetSatellite(string url)
        {
            string update = GetSatelliteUpdate(url).Result;

            return update;
        }

        private async Task<string> GetSatelliteUpdate(string url)
        {
            string satelliteUpdate = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {

                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", Constants.NET461_TOKEN);

                client.Timeout = new TimeSpan(1, 0, 0);
                response = await client.GetAsync(url);
            }
            catch (Exception e)
            {
                throw e;
            }

            satelliteUpdate = await response.Content.ReadAsStringAsync();

            return satelliteUpdate;
        }

        public bool DeleteSatelliteUpdate(int id)
        {
            throw new NotImplementedException();
        }

        public bool InsertSatelliteUpdate(string update)
        {
            throw new NotImplementedException();
        }

        public bool UpdateSatelliteUpdate(string update)
        {
            throw new NotImplementedException();
        }
    }
}
