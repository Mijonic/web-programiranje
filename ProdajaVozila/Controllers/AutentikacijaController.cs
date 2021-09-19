using ProdajaVozila.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProdajaVozila.Controllers
{
    public class AutentikacijaController : Controller
    {
        // GET: Autentikacija
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {


            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik korisnik = korisnici.Find(k => k.Username.Equals(username) && k.Password.Equals(password) && !k.LogickiObrisan);

            if (korisnik == null)
            {
                ViewBag.Message = "Neuspesno logovanje!";
                return View("Index");
            }

            Session["korisnik"] = korisnik;
            TempData["porukica"] = $"{korisnik.Username}, uspesno ste se ulogovali!";

            

            if (korisnik.Uloga == UlogaTip.ADMINISTRATOR)
                return RedirectToAction("AdminPrikaz", "Home");

            return RedirectToAction("PrikazVozilaProdaja", "Soping");
        }

        public ActionResult Registracija()
        {
          
            return View();
        }

       
        public ActionResult Izloguj()
        {
            Session["korisnik"] = null;

            return RedirectToAction("Index", "Autentikacija");
        }

        [HttpPost]
        public ActionResult Registracija(Korisnik korisnik)
        {

            bool validacijaUspesna = true;
            string poruka = "";


            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

            if(korisnik.Username == null || korisnik.Password == null || korisnik.Ime == null || korisnik.Prezime == null || korisnik.DatumRodjenja.Equals(DateTime.MinValue))
            {
                poruka += "Sva polja moraju biti popunjena!" + Environment.NewLine;
                validacijaUspesna = false;

            }else
            {
                if (korisnici.Contains(korisnik))
                {
                    poruka += $"Vec postoji korisnik sa {korisnik.Username}!" + Environment.NewLine;
                    validacijaUspesna = false;
                    return View();
                }

                foreach (Korisnik k in korisnici)
                {
                    if (k.Username.Equals(korisnik.Username))
                    {
                        poruka += $"Vec postoji korisnik sa {k.Username}" + Environment.NewLine;
                        validacijaUspesna = false;
                        break;
                    }
                }


                if (korisnik.Username.Equals("") || korisnik.Password.Equals("") || korisnik.Ime.Equals("") || korisnik.Prezime.Equals("") || korisnik.Pol.Equals("") || korisnik.DatumRodjenja.Equals(DateTime.MinValue))
                {
                    poruka += "Sva polja moraju biti popunjena!" + Environment.NewLine;
                    validacijaUspesna = false;
                }

               
                if (korisnik.Username.Length < 3)
                {
                    poruka += "Username mora imati minimalno 3 karaktera!" + Environment.NewLine;
                    validacijaUspesna = false;
                }

                if (korisnik.Password.Length < 8)
                {
                    poruka += "Password mora imati bar 8 karaktera!" + Environment.NewLine;
                    validacijaUspesna = false;
                }


            }

            

            if(!validacijaUspesna)
            {
                ViewBag.Message = poruka;



                return View();
            }

           

            korisnik.Uloga = UlogaTip.KUPAC;
            korisnik.Ulogovan = false;

            korisnici.Add(korisnik);
            ManipulacijaPodacima.UpisiKorisnika(korisnik, "~/App_Data/korisnici.txt");
   

            return RedirectToAction("Index", "Autentikacija");
        }


    }
}