using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProdajaVozila.Models
{
    public class AdministratorViewModel
    {
        private List<Vozilo> vozila;
        private List<Kupovina> kupovine;

        public AdministratorViewModel()
        {
            vozila = new List<Vozilo>();
            kupovine = new List<Kupovina>();
        }

        public AdministratorViewModel(List<Vozilo> vozila, List<Kupovina> kupovine)
        {
            Vozila = vozila;
            Kupovine = kupovine;
        }

        public List<Vozilo> Vozila { get => vozila; set => vozila = value; }
        public List<Kupovina> Kupovine { get => kupovine; set => kupovine = value; }
    }
}