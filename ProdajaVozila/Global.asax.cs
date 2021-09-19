using ProdajaVozila.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProdajaVozila
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            List<Vozilo> vozila = ManipulacijaPodacima.UcitajVozila("~/App_Data/vozila.txt");
            HttpContext.Current.Application["vozila"] = vozila;

            List<Korisnik> korisnici = ManipulacijaPodacima.UcitajKorisnike("~/App_Data/korisnici.txt");
            HttpContext.Current.Application["korisnici"] = korisnici;

            List<Kupovina> kupovine = ManipulacijaPodacima.UcitajKupovine("~/App_Data/kupovine.txt");
            HttpContext.Current.Application["kupovine"] = kupovine;



        }
    }
}
