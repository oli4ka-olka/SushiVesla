﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Repositories;
using SushiVesla.WebUI.Binders;
using SushiVesla.WebUI.Infrastructure;

namespace SushiVesla.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                null,
                "",
                new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                null,
                "Product",
                new { controller = "Product", action = "List", category = (string)null, page = 1 }
            );

            routes.MapRoute(
                null,
                "Page{page}",
                new { controller = "Product", action = "List", category = (string)null }, new { page = @"\d+" }
            );

            routes.MapRoute(
                null,
                "{category}",
                new { controller = "Product", action = "List", page = 1 }
            );

            routes.MapRoute(
                null,
                "{category}/Page{page}",
                new { controller = "Product", action = "List" }, new { page = @"\d+" }
            );

            routes.MapRoute(
                null,
                "Search/Index",
                new { controller = "Search", action = "Index", page = 1 }
            );

            routes.MapRoute(
                null,
                "Search/Index/Page{page}",
                new { controller = "Search", action = "Index" }, new { page = @"\d+" }
            );

            routes.MapRoute(
                null,
                "Product/Product/ID{id}",
                new { controller = "Product", action = "Product" }, new { id = @"\d+" }
            );

            routes.MapRoute(
                null,
                "{controller}/{action}"
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());

            Database.SetInitializer<EFDbContext>(null);
        }
    }
}