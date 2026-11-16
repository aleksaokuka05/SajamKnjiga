using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Posetilac
    {
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public DateTime DatumRodjenja { get; set; }
        public string Adresa { get; set; } = string.Empty;
        public string KontaktTelefon { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BrojClanskeKarte { get; set; } = string.Empty;
        public int TrenutnaGodinaClanstva {  get; set; }
        public StatusPosetioca Status {  get; set; }
        public double ProsecnaOcena { get; set; }
        public List<Kupovina> Kupovine { get; set; } = new();
        public List<Knjiga> ListaZelja { get; set; } = new();

        public Posetilac()
        {

        }

        public Posetilac(string ime, string prezime, DateTime datumRodjenja,
            string adresa, string kontaktTelefon, string email,
            string brojClanskeKarte, int trenutnaGodinaClanstva,
            StatusPosetioca status, double prosecnaOcena)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            Adresa = adresa;
            KontaktTelefon = kontaktTelefon;
            Email = email;
            BrojClanskeKarte = brojClanskeKarte;
            TrenutnaGodinaClanstva = trenutnaGodinaClanstva;
            Status = status;
            ProsecnaOcena = prosecnaOcena;
        }

    }
}
