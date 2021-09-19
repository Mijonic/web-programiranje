using ProdajaVozila.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProdajaVozila.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga == UlogaTip.ADMINISTRATOR)
                    return RedirectToAction("AdminPrikaz", "Home");

                if (k.Uloga == UlogaTip.KUPAC)
                    return RedirectToAction("Index", "Soping");
            }


            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];

            //neulogovan vidi samo vozila koja moze da kupi, tj naStanju su
            List<Vozilo> naStanju = new List<Vozilo>();

           
            foreach(Vozilo v in vozila)
            {
                if (v.NaStanju)
                    naStanju.Add(v);
            }

            return View(naStanju);
        }

        public ActionResult Sortiraj()
        {
            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];
           
            string poCemu = Request["nacin"];
            string smer = Request["sort"];

            //marka, model,cena
            Sortiraj(vozila, poCemu, smer);

            return RedirectToAction("Index");
        }

        public ActionResult Pretraga()
        {
            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];
            List<Vozilo> pomocna = new List<Vozilo>();

            var marka = Request["marka"];
            var model = Request["model"];

            if (marka != null)
            {
                ViewBag.kriterijumPretrage = "marki";

                foreach (Vozilo v in vozila)
                {
                    if (v.MarkaVozila.ToLower().Equals(marka.ToLower()) && v.NaStanju )
                    {
                        pomocna.Add(v);
                    }
                }
            }else if(model != null)
            {
                ViewBag.kriterijumPretrage = "modelu";

                foreach (Vozilo v in vozila)
                {
                    if (v.Model.ToLower().Equals(model.ToLower()) && v.NaStanju)
                    {
                        pomocna.Add(v);
                    }
                }

            }else
            {
                ViewBag.kriterijumPretrage = "ceni";
                double cenaDonja;
                Double.TryParse(Request["cenaDonja"], out cenaDonja);

                double cenaGornja;
                Double.TryParse(Request["cenaGornja"], out cenaGornja);

                foreach (Vozilo v in vozila)
                {
                    if (v.Cena >= cenaDonja && v.Cena <= cenaGornja && v.NaStanju)
                    {
                        pomocna.Add(v);
                    }
                }



            }


            return View(pomocna);
        }

        public ActionResult AdminPrikaz()
        {
            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.ADMINISTRATOR)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if(Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");


            return View();
        }

        public ActionResult PrikazKorisnika()
        {
            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.ADMINISTRATOR)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");


            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

            List<Korisnik> kupci = new List<Korisnik>();

            foreach( Korisnik k in korisnici)
            {
                if (k.Uloga == UlogaTip.KUPAC)
                {
                    if (!k.LogickiObrisan)
                        kupci.Add(k);
                }
                    
            }




            return View(kupci);
        }


        public ActionResult LogickiObrisi()
        {

            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.ADMINISTRATOR)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");

            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik korisnik = korisnici.Find(k => k.Username.Equals(Request["username"]));

            if(korisnik != null)
            {
                korisnik.LogickiObrisan = true;
            }

            ManipulacijaPodacima.UpisiSveKorisnike(korisnici, "~/App_Data/korisnici.txt");



            return RedirectToAction("PrikazKorisnika", "Home");

        }

        public ActionResult DodavanjeNovogVozila()
        {
            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.ADMINISTRATOR)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");


            return View("DodavanjeVozila");
        }


        [HttpPost]
        public ActionResult DodavanjeVozila(Vozilo vozilo)
        {

            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.ADMINISTRATOR)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");


            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];
            

            bool validacijaUspesna = true;
            string poruka = "";




            if (vozilo.MarkaVozila == null || vozilo.Model == null || vozilo.OznakaSasije == null || vozilo.Boja == null || vozilo.Opis == null)
            {
                poruka += "Sva polja moraju biti popunjena!" + Environment.NewLine;
                validacijaUspesna = false;

            }
            else
            {
                foreach(Vozilo v in vozila)
                {
                    if(v.OznakaSasije.Equals(vozilo.OznakaSasije))
                    {
                        poruka += $"Vec postoji vozilo sa {vozilo.OznakaSasije} sasijom!" + Environment.NewLine;
                        validacijaUspesna = false;
                        break;
                    }
                }

               
               


                if (vozilo.MarkaVozila == "" || vozilo.Model == "" || vozilo.OznakaSasije == "" || vozilo.Boja == "" || vozilo.Opis == "")
                {
                    poruka += "Sva polja moraju biti popunjena!" + Environment.NewLine;
                    validacijaUspesna = false;
                }


                if (vozilo.MarkaVozila.Length < 3)
                {
                    poruka += "Marka vozila mora imati minimalno 3 karaktera!" + Environment.NewLine;
                    validacijaUspesna = false;
                }

                if(vozilo.BrojVrata == 0)
                {
                    poruka += "Broj vrata nije u odgovarajucem formatu!" + Environment.NewLine;
                    validacijaUspesna = false;
                }

                if(vozilo.Cena == 0)
                {
                    poruka += "Cena nije u odgovarajucem formatu!" + Environment.NewLine;
                    validacijaUspesna = false;
                }

                

                

                

            }



            if (!validacijaUspesna)
            {
                ViewBag.Message = poruka;



                return View();
            }

            vozilo.NaStanju = true;


           

            vozila.Add(vozilo);
            ManipulacijaPodacima.UpisiVozilo(vozilo, "~/App_Data/vozila.txt");
        
            


            
            return RedirectToAction("PrikazVozila", "Home");
        }


        public ActionResult PrikazVozila()
        {
            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.ADMINISTRATOR)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");

            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];
            List<Kupovina> kupovine = (List<Kupovina>)HttpContext.Application["kupovine"];

            AdministratorViewModel podaci = new AdministratorViewModel(vozila, kupovine);
            


            return View(podaci);
        }

        public ActionResult Modifikovanje()
        {

            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.ADMINISTRATOR)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");

            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];
            List<Kupovina> kupovine = (List<Kupovina>)HttpContext.Application["kupovine"];

            AdministratorViewModel podaci = new AdministratorViewModel(vozila, kupovine);

            ViewBag.Message = "";

            return View(podaci);
        }


        
        public ActionResult ModifikovanjeVozila(string id)
        {
            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.ADMINISTRATOR)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");


            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];
            Vozilo vozilo = vozila.Find(v => v.OznakaSasije.Equals(id));

            
            Session["vozilo"] = vozilo;


            return View(vozilo);
        }

       
        [HttpPost]
        public ActionResult ModifikacijaPostojecegVozila(Vozilo vozilo)
        {
            if (Session["korisnik"] != null)
            {
                Korisnik k = (Korisnik)Session["korisnik"];
                if (k.Uloga != UlogaTip.ADMINISTRATOR)
                    return RedirectToAction("Index", "Autentikacija");
            }

            if (Session["korisnik"] == null)
                return RedirectToAction("Index", "Autentikacija");

            List<Vozilo> vozila = (List<Vozilo>)HttpContext.Application["vozila"];
            Vozilo pomVozilo = (Vozilo)Session["vozilo"];


            bool validacijaUspesna = true;
            string poruka = "";




            if (vozilo.MarkaVozila == null || vozilo.Model == null || vozilo.OznakaSasije == null || vozilo.Boja == null || vozilo.Opis == null)
            {
                poruka += "Sva polja moraju biti popunjena!" + Environment.NewLine;
                validacijaUspesna = false;

            }
            else
            {
                


                if (vozilo.MarkaVozila == "" || vozilo.Model == "" || vozilo.OznakaSasije == "" || vozilo.Boja == "" || vozilo.Opis == "")
                {
                    poruka += "Sva polja moraju biti popunjena!" + Environment.NewLine;
                    validacijaUspesna = false;
                }


                if (vozilo.MarkaVozila.Length < 3)
                {
                    poruka += "Marka vozila mora imati minimalno 3 karaktera!" + Environment.NewLine;
                    validacijaUspesna = false;
                }

                if (vozilo.BrojVrata == 0)
                {
                    poruka += "Broj vrata nije u odgovarajucem formatu!" + Environment.NewLine;
                    validacijaUspesna = false;
                }

                if (vozilo.Cena == 0)
                {
                    poruka += "Cena nije u odgovarajucem formatu!" + Environment.NewLine;
                    validacijaUspesna = false;
                }






            }



            if (!validacijaUspesna)
            {
              

                ViewBag.Message = poruka;

                return View("ModifikovanjeVozila", vozilo);
            }

            vozilo.NaStanju = true;

            Vozilo zaIzmenu = vozila.Find(v => v.OznakaSasije.Equals(pomVozilo.OznakaSasije));

            vozila.RemoveAll(x => x.OznakaSasije == zaIzmenu.OznakaSasije);
            vozila.Add(vozilo);
            ManipulacijaPodacima.UpisiSvaVozila(vozila, "~/App_Data/vozila.txt");
            HttpContext.Application["vozila"] = ManipulacijaPodacima.UcitajVozila("~/App_Data/vozila.txt");

           
            return RedirectToAction("PrikazVozila", "Home");



           
        }

        private void Sortiraj(List<Vozilo> vozila,string poCemu, string smer)
        {
            if (poCemu.Equals("marka"))
            {
                if (smer.Equals("rastuce"))
                {
                    for (int i = 0; i < vozila.Count - 1; i++)
                    {
                        for (int j = i + 1; j < vozila.Count; j++)
                        {
                            if (vozila[i].NaStanju && vozila[j].NaStanju)
                            {
                                int rezPoredjena = String.Compare(vozila[i].MarkaVozila.ToLower(), vozila[j].MarkaVozila.ToLower());
                                if (rezPoredjena > 0)
                                {
                                    Vozilo v = new Vozilo(vozila[i]);
                                    vozila[i] = vozila[j];
                                    vozila[j] = v;

                                }
                            }

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < vozila.Count - 1; i++)
                    {
                        for (int j = i + 1; j < vozila.Count; j++)
                        {
                            if (vozila[i].NaStanju && vozila[j].NaStanju)
                            {
                                int rezPoredjena = String.Compare(vozila[i].MarkaVozila.ToLower(), vozila[j].MarkaVozila.ToLower());
                                if (rezPoredjena < 0)
                                {
                                    Vozilo v = new Vozilo(vozila[i]);
                                    vozila[i] = vozila[j];
                                    vozila[j] = v;

                                }
                            }

                        }
                    }
                }

            }
            else if (poCemu.Equals("model"))
            {
                if (smer.Equals("rastuce"))
                {
                    for (int i = 0; i < vozila.Count - 1; i++)
                    {
                        for (int j = i + 1; j < vozila.Count; j++)
                        {
                            if (vozila[i].NaStanju && vozila[j].NaStanju)
                            {
                                int rezPoredjena = String.Compare(vozila[i].Model.ToLower(), vozila[j].Model.ToLower());
                                if (rezPoredjena > 0)
                                {
                                    Vozilo v = new Vozilo(vozila[i]);
                                    vozila[i] = vozila[j];
                                    vozila[j] = v;

                                }
                            }

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < vozila.Count - 1; i++)
                    {
                        for (int j = i + 1; j < vozila.Count; j++)
                        {
                            if (vozila[i].NaStanju && vozila[j].NaStanju)
                            {
                                int rezPoredjena = String.Compare(vozila[i].Model.ToLower(), vozila[j].Model.ToLower());
                                if (rezPoredjena < 0)
                                {
                                    Vozilo v = new Vozilo(vozila[i]);
                                    vozila[i] = vozila[j];
                                    vozila[j] = v;

                                }
                            }

                        }
                    }
                }


            }
            else
            {
                if (poCemu.Equals("cena"))
                {
                    if (smer.Equals("rastuce"))
                    {
                        for (int i = 0; i < vozila.Count - 1; i++)
                        {
                            for (int j = i + 1; j < vozila.Count; j++)
                            {
                                if (vozila[i].NaStanju && vozila[j].NaStanju)
                                {

                                    if (vozila[i].Cena > vozila[j].Cena)
                                    {
                                        Vozilo v = new Vozilo(vozila[i]);
                                        vozila[i] = vozila[j];
                                        vozila[j] = v;

                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < vozila.Count - 1; i++)
                        {
                            for (int j = i + 1; j < vozila.Count; j++)
                            {
                                if (vozila[i].NaStanju && vozila[j].NaStanju)
                                {
                                    if (vozila[i].Cena < vozila[j].Cena)
                                    {
                                        Vozilo v = new Vozilo(vozila[i]);
                                        vozila[i] = vozila[j];
                                        vozila[j] = v;

                                    }
                                }

                            }
                        }
                    }

                }
            }
        }
                            
    }
}