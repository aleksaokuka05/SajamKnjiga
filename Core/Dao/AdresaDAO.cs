
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    // Izvor: OISISI, Vezbe 3, CRUDExample (DAO pattern)
    public class AdresaDAO
    {
        private readonly List<Adresa> _adrese;
        private readonly Storage<Adresa> _storage;
        private int _sledeciId;

        public AdresaDAO()
        {
            _storage = new Storage<Adresa>("adrese.csv");
            _adrese = _storage.Load();
            _sledeciId = _adrese.Count == 0 ? 1 : _adrese.Max(adresa => adresa.Id) + 1;
        }

        private int GenerateId()
        {
            return _sledeciId++;
        }

        public Adresa AddAdresa(Adresa adresa)
        {
            adresa.Id = GenerateId();
            _adrese.Add(adresa);
            _storage.Save(_adrese);

            return adresa;
        }

        public Adresa? UpdateAdresa(Adresa adresa)
        {
            Adresa? staraAdresa = GetAdresaById(adresa.Id);

            if (staraAdresa is null) return null;

            staraAdresa.Ulica = adresa.Ulica;
            staraAdresa.Broj = adresa.Broj;
            staraAdresa.Grad = adresa.Grad;
            staraAdresa.Drzava = adresa.Drzava;

            _storage.Save(_adrese);

            return staraAdresa;
        }

        public Adresa? GetAdresaById(int id)
        {
            foreach (Adresa adresa in _adrese)
            {
                if (adresa.Id == id)
                {
                    return adresa;
                }
            }

            return null;
        }

        public Adresa? RemoveAdresa(int id)
        {
            Adresa? adresa = GetAdresaById(id);

            if (adresa is null) return null;

            _adrese.Remove(adresa);
            _storage.Save(_adrese);

            return adresa;
        }

        public List<Adresa> GetAllAdrese()
        {
            return _adrese;
        }
    }
}
