using System.Web;
using System.Web.Mvc;

namespace Net46AmazonLambdaServices
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
