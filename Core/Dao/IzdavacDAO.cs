
using System;
using System.Collections.Generic;


using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    public class IzdavacDAO
    {
        private readonly List<Izdavac> _izdavaci;
        private readonly Storage<Izdavac> _storage;

        public IzdavacDAO()
        {
            _storage = new Storage<Izdavac>("izdavaci.csv");
            _izdavaci = _storage.Load();
        }

        public Izdavac AddIzdavac(Izdavac izdavac)
        {
            _izdavaci.Add(izdavac);
            _storage.Save(_izdavaci);
            return izdavac;
        }

        public Izdavac? UpdateIzdavac(Izdavac izdavac)
        {
            Izdavac? stariIzdavac = GetIzdavacBySifra(izdavac.Sifra);

            if (stariIzdavac is null)
                return null;

            stariIzdavac.Naziv = izdavac.Naziv;
            stariIzdavac.SefIzdavaca = izdavac.SefIzdavaca;
            stariIzdavac.Autori = izdavac.Autori;
            stariIzdavac.Knjige = izdavac.Knjige;

            _storage.Save(_izdavaci);
            return stariIzdavac;
        }

        public Izdavac? GetIzdavacBySifra(string sifra)
        {
            foreach (Izdavac izdavac in _izdavaci)
            {
                if (izdavac.Sifra == sifra)
                    return izdavac;
            }

            return null;
        }

        public Izdavac? RemoveIzdavac(string sifra)
        {
            Izdavac? izdavac = GetIzdavacBySifra(sifra);

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
