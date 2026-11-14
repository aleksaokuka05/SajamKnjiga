using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Adresa
    {

        public string Ulica { get; set; } = string.Empty;

        public string Broj { get; set; } = string.Empty;

        public string Grad { get; set; } = string.Empty;

        public string Drzava { get; set; } = string.Empty;


        public Adresa()
        {



        }


        public Adresa(string ulica, string broj, string grad, string drzava)
        {
            Ulica = ulica;
            Broj = broj;
            Grad = grad;
            Drzava = drzava;
        }
    }
}
