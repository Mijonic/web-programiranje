using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ProdajaVozila.Models
{
    public class ManipulacijaPodacima
    {
        public static List<Vozilo> UcitajVozila(string putanja)
        {
            List<Vozilo> vozila = new List<Vozilo>();

            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja,FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while( (line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split('|');

                if (podaci.Length == 9)
                {

                    Vozilo v = new Vozilo(podaci[0], podaci[1], podaci[2], podaci[3], int.Parse(podaci[4]), podaci[5], (GorivoTip)Enum.Parse(typeof(GorivoTip), podaci[6]), Double.Parse(podaci[7]), Boolean.Parse(podaci[8]));
                    vozila.Add(v);
                }else
                {
                    continue;
                }

                

            }

            sr.Close();
            stream.Close();

            return vozila;

        }

        public static List<Korisnik> UcitajKorisnike(string putanja)
        {

            List<Korisnik> korisnici = new List<Korisnik>();

            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split('|');

                if (podaci.Length == 10)
                {

                    Korisnik k = new Korisnik(podaci[0], podaci[1], podaci[2], podaci[3], (PolTip)Enum.Parse(typeof(PolTip), podaci[4]), podaci[5], DateTime.ParseExact(podaci[6], "dd/MM/yyyy", CultureInfo.InvariantCulture), (UlogaTip)Enum.Parse(typeof(UlogaTip), podaci[7]), Boolean.Parse( podaci[8]), Boolean.Parse(podaci[9]));
                    korisnici.Add(k);
                }else
                {
                    continue;
                }



            }

            sr.Close();
            stream.Close();

            return korisnici;


        }

        public static List<Kupovina> UcitajKupovine(string putanja)
        {
            List<Kupovina> kupovine = new List<Kupovina>();

            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split('|');

                if (podaci.Length == 21)
                {
                    Korisnik k = new Korisnik(podaci[0], podaci[1], podaci[2], podaci[3], (PolTip)Enum.Parse(typeof(PolTip), podaci[4]), podaci[5], DateTime.ParseExact(podaci[6], "dd/MM/yyyy", CultureInfo.CurrentCulture), (UlogaTip)Enum.Parse(typeof(UlogaTip), podaci[7]), Boolean.Parse(podaci[8]), Boolean.Parse(podaci[9]));
                    Vozilo v = new Vozilo(podaci[10], podaci[11], podaci[12], podaci[13], int.Parse(podaci[14]), podaci[15], (GorivoTip)Enum.Parse(typeof(GorivoTip), podaci[16]), Double.Parse(podaci[17]), Boolean.Parse(podaci[18]));
                    Kupovina kupovina = new Kupovina(k,v, DateTime.ParseExact(podaci[19], "dd/MM/yyyy",CultureInfo.CurrentCulture), Double.Parse(podaci[20]));
                    kupovine.Add(kupovina);
                }
                else
                {
                    continue;
                }



            }

            sr.Close();
            stream.Close();

            return kupovine;
        }


        public static void UpisiKorisnika(Korisnik k, string putanja)
        {
            putanja = HostingEnvironment.MapPath(putanja);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                sw.WriteLine(k);
            }
        }

        public static void UpisiVozilo(Vozilo v, string putanja)
        {
            putanja = HostingEnvironment.MapPath(putanja);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                sw.WriteLine(v);
            }
        }

        public static void UpisiSveKorisnike(List<Korisnik> lista, string putanja)
        {

            putanja = HostingEnvironment.MapPath(putanja);
            File.WriteAllText(putanja, String.Empty);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                foreach (Korisnik k in lista)
                    sw.WriteLine(k);
            }

            
        }

        public static void UpisiSvaVozila(List<Vozilo> lista, string putanja)
        {

            putanja = HostingEnvironment.MapPath(putanja);
            File.WriteAllText(putanja, String.Empty);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                foreach (Vozilo v in lista)
                    sw.WriteLine(v);
            }


        }

        public static void UpisiKupovine(List<Kupovina> lista, string putanja)
        {
            putanja = HostingEnvironment.MapPath(putanja);

            using (StreamWriter sw = File.CreateText(putanja))
            {
                foreach(Kupovina k in lista)
                    sw.WriteLine(k);
            }
        }









    }
}