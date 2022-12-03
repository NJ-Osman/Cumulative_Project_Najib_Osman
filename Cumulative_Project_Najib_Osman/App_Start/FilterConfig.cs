using System.Web;
using System.Web.Mvc;

namespace Cumulative_Project_Najib_Osman
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
