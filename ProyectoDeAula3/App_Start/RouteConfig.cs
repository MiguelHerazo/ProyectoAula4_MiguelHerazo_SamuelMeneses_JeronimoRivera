﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProyectoDeAula4_MiguelHerazo_SamuelMeneses_JeronimoRivera
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ruta para acceder a la acción ProcesarFormulario
            routes.MapRoute(
                name: "ProcesarFormulario",
                url: "IdeaNegocio/ProcesarFormulario",
                defaults: new { controller = "IdeaNegocio", action = "Index" }
            );

            routes.MapRoute(
                name: "EliminarEquipo",
                url: "Home/EliminarEquipo/{equipoId}",
                defaults: new { controller = "Home", action = "EliminarEquipo" }
            );

            // Ruta para acceder a la acción AccionConfirmacion
            routes.MapRoute(
                name: "AccionConfirmacion",
                url: "IdeaNegocio/AccionConfirmacion",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
