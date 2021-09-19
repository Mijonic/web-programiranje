using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProdajaVozila.Models
{
    public class Kupovina
    {
        private Korisnik kupac;
        private Vozilo vozilo;
        private DateTime datumKupovine;
        private double naplacenaCena;

        public Kupovina()
        {


        }

        public Kupovina(Korisnik kupac, Vozilo vozilo, DateTime datumKupovine, double naplacenaCena)
        {
            Kupac = kupac;
            Vozilo = vozilo;
            DatumKupovine = datumKupovine;
            NaplacenaCena = naplacenaCena;
        }

        public override string ToString()
        {
            string s = "";

            // 5/27/2020 3:06:53 PM

            string[] datum = DatumKupovine.ToString().Split(' ');

            string pomDatum = datum[0]; // 6/22/2020
            string[] razdvajanje = pomDatum.Split('/');
            string dan = razdvajanje[1];
            int pomDan = int.Parse(dan);
            if (pomDan < 10)
            {
                dan = "0" + dan;
            }
            string mesec = razdvajanje[0];
            int pomMesec = int.Parse(mesec);
            if (pomMesec < 10)
            {
                mesec = "0" + mesec;
            }
            string godina = razdvajanje[2];

            string praviDatum = dan + "/" + mesec + "/" + godina;

            s += Kupac + "|" + Vozilo + "|" + praviDatum + "|" + naplacenaCena;

            return s;
        }

        public Korisnik Kupac { get => kupac; set => kupac = value; }
        public Vozilo Vozilo { get => vozilo; set => vozilo = value; }
        public DateTime DatumKupovine { get => datumKupovine; set => datumKupovine = value; }
        public double NaplacenaCena { get => naplacenaCena; set => naplacenaCena = value; }
    }
}