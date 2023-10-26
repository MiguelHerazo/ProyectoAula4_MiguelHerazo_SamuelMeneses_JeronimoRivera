using System.Web;
using System.Web.Mvc;

namespace ProyectoDeAula4_MiguelHerazo_SamuelMeneses_JeronimoRivera
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
