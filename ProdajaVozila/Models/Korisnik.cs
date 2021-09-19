using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProdajaVozila.Models
{
    public class Korisnik
    {
        private string username;
        private string password;
        private string ime;
        private string prezime;
        private PolTip pol;
        private string email;
        private DateTime datumRodjenja;
        private UlogaTip uloga;
        private bool ulogovan;
        private bool logickiObrisan;

        public Korisnik()
        {
            Username = "";
            Password = "";
            LogickiObrisan = false;
            Ulogovan = false;

        }

        public Korisnik(string username, string password, string ime, string prezime, PolTip pol, string email, DateTime datumRodjenja, UlogaTip uloga, bool ulogovan, bool obrisan)
        {
            Username = username;
            Password = password;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            Ulogovan = ulogovan;
            LogickiObrisan = obrisan;
        }

        public override bool Equals(object obj)
        {
            bool provera = false;

            if(obj != null)
            {
                provera = false;
            }else
            {
                provera = ((Korisnik)obj).Username.Equals(this.Username);
            }
            return provera;
        }

        public override string ToString()
        {
            string s = "";
            string u = "";

            if (Uloga == UlogaTip.KUPAC)
                u = "KUPAC";
            else if (Uloga == UlogaTip.ADMINISTRATOR)
                u = "ADMINISTRATOR";
            else u = "POSETILAC";

            

           string[] datum = DatumRodjenja.ToString().Split(' ');

            string pomDatum = datum[0]; // 6/22/2020
            string[] razdvajanje = pomDatum.Split('/');
            string dan = razdvajanje[1];
            int pomDan = int.Parse(dan);
            if(pomDan < 10 )
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



               

            s += Username + "|" + Password + "|" + Ime + "|" + Prezime + "|" + Pol + "|" + Email + "|" + praviDatum + "|" + u + "|" + Ulogovan + "|" + LogickiObrisan;
       

            return s;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public PolTip Pol { get => pol; set => pol = value; }
        public string Email { get => email; set => email = value; }
        public DateTime DatumRodjenja { get => datumRodjenja; set => datumRodjenja = value; }
        public UlogaTip Uloga { get => uloga; set => uloga = value; }
        public bool Ulogovan { get => ulogovan; set => ulogovan = value; }
        public bool LogickiObrisan { get => logickiObrisan; set => logickiObrisan = value; }
    }
}