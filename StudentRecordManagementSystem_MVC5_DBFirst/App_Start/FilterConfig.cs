using System.Web;
using System.Web.Mvc;

namespace StudentRecordManagementSystem_MVC5_DBFirst
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
