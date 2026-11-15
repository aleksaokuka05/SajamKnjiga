using System;
using System.Collections.Generic;
using System.Text;
using Core.Interfaces;

namespace Core.Models
{
    public class Adresa : ISerializable
    {

        public string Ulica { get; set; } 

        public string Broj { get; set; } 

        public string Grad { get; set; } 

        public string Drzava { get; set; } 


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


        public string[] ToCSV()
        {
            string[] niz_polja =
                 {
                     Ulica,
                     Broj,
                     Grad,
                     Drzava
                 };

            return niz_polja;

        }

        public void FromCSV(string[] values)
        {
            Ulica = values[0];
            Broj = values[1];
            Grad = values[2];
            Drzava = values[3];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Ulica + " ");
            sb.Append(Broj + ", ");
            sb.Append(Grad + ", ");
            sb.Append(Drzava);

            return sb.ToString();
        }

    }
}
