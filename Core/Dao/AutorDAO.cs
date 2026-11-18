using System;
using System.Collections.Generic;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    public class AutorDAO
    {
        private readonly List<Autor> _autori;
        private readonly Storage<Autor> _storage;

        public AutorDAO()
        {
            _storage = new Storage<Autor>("autori.csv");
            _autori = _storage.Load();
        }

        public Autor AddAutor(Autor autor)
        {
            _autori.Add(autor);
            _storage.Save(_autori);
            return autor;
        }

        public Autor? UpdateAutor(Autor autor)
        {
            Autor? stariAutor = GetAutorByBrojLicneKarte(autor.BrojLicneKarte);
            if (stariAutor is null) return null;

            stariAutor.Ime = autor.Ime;
            stariAutor.Prezime = autor.Prezime;
            stariAutor.DatumRodjenja = autor.DatumRodjenja;
            stariAutor.Adresa = autor.Adresa;
            stariAutor.Telefon = autor.Telefon;
            stariAutor.Imejl = autor.Imejl;
            stariAutor.GodineIskustva = autor.GodineIskustva;
            stariAutor.Knjige = autor.Knjige;

            _storage.Save(_autori);
            return stariAutor;
        }

        public Autor? GetAutorByBrojLicneKarte(string brojLicneKarte)
        {
            foreach (Autor autor in _autori)
            {
                if (autor.BrojLicneKarte == brojLicneKarte)
                {
                    return autor;
                }
            }

            return null;
        }


        public Autor? RemoveAutor(string brojLicneKarte)
        {
            Autor? autor = GetAutorByBrojLicneKarte(brojLicneKarte);

            if (autor is null) return null;

            _autori.Remove(autor);
            _storage.Save(_autori);

            return autor;
        }



        public List<Autor> GetAllAutori()
        {
            return _autori;
        }



    }
}
