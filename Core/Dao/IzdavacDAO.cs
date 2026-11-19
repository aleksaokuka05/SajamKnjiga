
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    // Izvor: OISISI, Vezbe 3, CRUDExample (DAO pattern)
    public class IzdavacDAO
    {
        private readonly List<Izdavac> _izdavaci;
        private readonly Storage<Izdavac> _storage;

        private int _sledeciId;

        public IzdavacDAO()
        {
            _storage = new Storage<Izdavac>("izdavaci.csv");
            _izdavaci = _storage.Load();

            _sledeciId = _izdavaci.Count == 0
                ? 1
                : _izdavaci.Max(izdavac => izdavac.Id) + 1;
        }

        private int GenerateId()
        {
            return _sledeciId++;
        }

        public Izdavac AddIzdavac(Izdavac izdavac)
        {
            izdavac.Id = GenerateId();

            _izdavaci.Add(izdavac);
            _storage.Save(_izdavaci);

            return izdavac;
        }

        public Izdavac? UpdateIzdavac(Izdavac izdavac)
        {
            Izdavac? stariIzdavac = GetIzdavacById(izdavac.Id);

            if (stariIzdavac is null)
                return null;

            stariIzdavac.Sifra = izdavac.Sifra;
            stariIzdavac.Naziv = izdavac.Naziv;
            stariIzdavac.SefIzdavacaId = izdavac.SefIzdavacaId;
            stariIzdavac.SefIzdavaca = izdavac.SefIzdavaca;
            stariIzdavac.Autori = izdavac.Autori;
            stariIzdavac.Knjige = izdavac.Knjige;

            _storage.Save(_izdavaci);

            return stariIzdavac;
        }

        public Izdavac? GetIzdavacById(int id)
        {
            foreach (Izdavac izdavac in _izdavaci)
            {
                if (izdavac.Id == id)
                    return izdavac;
            }

            return null;
        }

        public Izdavac? RemoveIzdavac(int id)
        {
            Izdavac? izdavac = GetIzdavacById(id);

            if (izdavac is null)
                return null;

            _izdavaci.Remove(izdavac);
            _storage.Save(_izdavaci);

            return izdavac;
        }

        public List<Izdavac> GetAllIzdavaci()
        {
            return _izdavaci;
        }
    }
}
