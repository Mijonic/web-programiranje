using ProdajaVozila.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProdajaVozila.Controllers
{
    public class SopingController : Controller
    {
        // GET: Soping
        public ActionResult Index()
        {
            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.KUPAC)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");


            ViewBag.Korisnik = Session["korisnik"];

            return View();
        }

        public ActionResult PrikazVozilaProdaja()
        {

            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.KUPAC)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");

            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];
            List<Vozilo> naStanju = new List<Vozilo>();

            foreach (Vozilo v in vozila)
            {
                if (v.NaStanju)
                    naStanju.Add(v);
            }

            return View(naStanju);
        }


        public ActionResult PrikazDetalja(string oznakaSasije)
        {
            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.KUPAC)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");

            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];
            Vozilo vozilo = vozila.Find(v => v.OznakaSasije.Equals(oznakaSasije));

            ViewBag.Korisnik = Session["korisnik"];

            return View(vozilo);
        }

        public ActionResult KupiVozilo(string oznakaSasije)
        {
            if (Session["korisnik"] != null)
            {
                Korisnik kor = (Korisnik)Session["korisnik"];
                if (kor.Uloga != UlogaTip.KUPAC)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");

            Korisnik k = (Korisnik)Session["korisnik"];
            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];
            List<Kupovina> kupovine = (List<Kupovina>)HttpContext.Application["kupovine"];
            Vozilo vozilo = vozila.Find(v => v.OznakaSasije.Equals(oznakaSasije));
            vozilo.NaStanju = false;


            DateTime vreme = DateTime.Now;
            vreme = vreme.Date;

       

            Kupovina kupovina = new Kupovina(k,vozilo, vreme, vozilo.Cena);
            kupovine.Add(kupovina);
            ManipulacijaPodacima.UpisiKupovine(kupovine, "~/App_Data/kupovine.txt");
            ManipulacijaPodacima.UpisiSvaVozila(vozila, "~/App_Data/vozila.txt");

            return RedirectToAction("PrikazKupovina","Soping"); // redirekcija na prikaz svih ranijih kupovina korisnika

        }


        public ActionResult PrikazKupovina()
        {

            if (Session["korisnik"] != null)
            {
                Korisnik kor = (Korisnik)Session["korisnik"];
                if (kor.Uloga != UlogaTip.KUPAC)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");

            List<Kupovina> kupovine = (List<Kupovina>)HttpContext.Application["kupovine"];
            List<Kupovina> pomKupovine = new List<Kupovina>();
            Korisnik k = (Korisnik)Session["korisnik"];
            

            foreach(Kupovina kup in kupovine)
            {
                if(kup.Kupac.Username == k.Username)
                {
                    pomKupovine.Add(kup);
                }
            }

            if(pomKupovine.Count == 0)
            {
                ViewBag.Message = "Niste izvrsili ni jednu kupovinu do sada.";
            }

           
            

            return View(pomKupovine);
        }
    }
}