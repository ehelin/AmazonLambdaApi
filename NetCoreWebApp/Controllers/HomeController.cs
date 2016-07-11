using Service;
using Shared;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebApp.Models.Home;

namespace NetCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DisplaySatelliteUpdate(SatelliteGetModel inModel)
        {
            SatelliteGetDisplayModel outModel = new SatelliteGetDisplayModel();

            //TODO - html encode this
            //TODO - make url configurable
            string url = "http://localhost:56571/api/Net46AmazonLambda?satelliteId=" + inModel.SatelliteUpdateId;

            IService serv = new ServiceImpl();
            outModel.SatelliteUpdate = serv.GetSatellite(url);

            return View(outModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
