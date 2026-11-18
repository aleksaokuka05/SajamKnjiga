using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    public class AutorDAO
    {
        private readonly List<Autor> _autori;
        private readonly Storage<Autor> _storage;

        private int _sledeciId;

        public AutorDAO()
        {
            _storage = new Storage<Autor>("autori.csv");
            _autori = _storage.Load();
            _sledeciId = _autori.Count == 0 ? 1 : _autori.Max(autor => autor.Id) + 1;
        }

        private int GenerateId()
        {
            return _sledeciId++;
        }

        public Autor AddAutor(Autor autor)
        {
            autor.Id = GenerateId();
            _autori.Add(autor);
            _storage.Save(_autori);

            return autor;
        }

        public Autor? UpdateAutor(Autor autor)
        {
            Autor? stariAutor = GetAutorById(autor.Id);

            if (stariAutor is null) return null;

            stariAutor.Ime = autor.Ime;
            stariAutor.Prezime = autor.Prezime;
            stariAutor.DatumRodjenja = autor.DatumRodjenja;
            stariAutor.AdresaId = autor.AdresaId;
            stariAutor.Adresa = autor.Adresa;
            stariAutor.Telefon = autor.Telefon;
            stariAutor.Imejl = autor.Imejl;
            stariAutor.BrojLicneKarte = autor.BrojLicneKarte;
            stariAutor.GodineIskustva = autor.GodineIskustva;
            stariAutor.Knjige = autor.Knjige;

            _storage.Save(_autori);

            return stariAutor;
        }

        public Autor? GetAutorById(int id)
        {
            foreach (Autor autor in _autori)
            {
                if (autor.Id == id)
                {
                    return autor;
                }
            }

            return null;
        }

        public Autor? RemoveAutor(int id)
        {
            Autor? autor = GetAutorById(id);

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
