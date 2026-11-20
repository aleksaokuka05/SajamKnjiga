
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    // Izvor: OISISI, Vezbe 3, CRUDExample (DAO pattern)
    public class KupovinaDAO
    {
        private readonly List<Kupovina> _kupovine;
        private readonly Storage<Kupovina> _storage;

        private int _sledeciId;

        public KupovinaDAO()
        {
            _storage = new Storage<Kupovina>("kupovine.csv");
            _kupovine = _storage.Load();

            _sledeciId = _kupovine.Count == 0
                ? 1
                : _kupovine.Max(kupovina => kupovina.Id) + 1;
        }

        private int GenerateId()
        {
            return _sledeciId++;
        }

        public Kupovina AddKupovina(Kupovina kupovina)
        {
            kupovina.Id = GenerateId();

            _kupovine.Add(kupovina);
            _storage.Save(_kupovine);

            return kupovina;
        }

        public Kupovina? UpdateKupovina(Kupovina kupovina)
        {
            Kupovina? staraKupovina = GetKupovinaById(kupovina.Id);

            if (staraKupovina is null)
                return null;

            staraKupovina.PosetilacId = kupovina.PosetilacId;
            staraKupovina.Posetilac = kupovina.Posetilac;
            staraKupovina.KnjigaId = kupovina.KnjigaId;
            staraKupovina.Knjiga = kupovina.Knjiga;
            staraKupovina.DatumKupovine = kupovina.DatumKupovine;
            staraKupovina.Ocena = kupovina.Ocena;
            staraKupovina.Komentar = kupovina.Komentar;

            _storage.Save(_kupovine);

            return staraKupovina;
        }

        public Kupovina? GetKupovinaById(int id)
        {
            foreach (Kupovina kupovina in _kupovine)
            {
                if (kupovina.Id == id)
                    return kupovina;
            }

            return null;
        }

        public Kupovina? RemoveKupovina(int id)
        {
            Kupovina? kupovina = GetKupovinaById(id);

            if (kupovina is null)
                return null;

            _kupovine.Remove(kupovina);
            _storage.Save(_kupovine);

            return kupovina;
        }

        public List<Kupovina> GetAllKupovine()
        {
            return _kupovine;
        }
    }
}
