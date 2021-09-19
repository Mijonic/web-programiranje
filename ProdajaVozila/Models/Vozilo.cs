using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProdajaVozila.Models
{
    public class Vozilo
    {
        private string markaVozila;
        private string model;
        private string oznakaSasije;
        private string boja;
        private int brojVrata;
        private string opis;
        private GorivoTip vrstaGoriva;
        private double cena;
        private bool naStanju;

        public Vozilo()
        {

        }

        public Vozilo( Vozilo vozilo)
        {
            

            MarkaVozila = vozilo.MarkaVozila;
            Model = vozilo.Model;
            OznakaSasije = vozilo.OznakaSasije;
            Boja = vozilo.boja;
            BrojVrata = vozilo.BrojVrata;
            Opis = vozilo.opis;
            VrstaGoriva = vozilo.VrstaGoriva;
            Cena = vozilo.cena;
            NaStanju = vozilo.NaStanju;
        }

        public Vozilo(string markaVozila, string model, string oznakaSasije, string boja, int brojVrata, string opis, GorivoTip vrstaGoriva, double cena, bool naStanju)
        {
            MarkaVozila = markaVozila;
            Model = model;
            OznakaSasije = oznakaSasije;
            Boja = boja;
            BrojVrata = brojVrata;
            Opis = opis;
            VrstaGoriva = vrstaGoriva;
            Cena = cena;
            NaStanju = naStanju;
        }

        public override bool Equals(object obj)
        {
            bool provera = false;

            if (obj != null)
            {
                provera = false;
            }
            else
            {
                provera = ((Vozilo)obj).OznakaSasije.Equals(this.OznakaSasije);
            }
            return provera;
        }

        public override string ToString()
        {
            string s = "";

            s += MarkaVozila + "|" + Model + "|" + OznakaSasije + "|" + Boja + "|" + BrojVrata + "|" + Opis + "|" + VrstaGoriva.ToString() + "|" + Cena.ToString() + "|" + NaStanju;


            return s;
        }


        public string MarkaVozila { get => markaVozila; set => markaVozila = value; }
        public string Model { get => model; set => model = value; }
        public string OznakaSasije { get => oznakaSasije; set => oznakaSasije = value; }
        public string Boja { get => boja; set => boja = value; }
        public int BrojVrata { get => brojVrata; set => brojVrata = value; }
        public string Opis { get => opis; set => opis = value; }
        public GorivoTip VrstaGoriva { get => vrstaGoriva; set => vrstaGoriva = value; }
        public double Cena { get => cena; set => cena = value; }
        public bool NaStanju { get => naStanju; set => naStanju = value; }
    }
}