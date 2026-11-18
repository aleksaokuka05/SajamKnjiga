
using System;
using System.Collections.Generic;


using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    public class KupovinaDAO
    {
        private readonly List<Kupovina> _kupovine;
        private readonly Storage<Kupovina> _storage;

        public KupovinaDAO()
        {
            _storage = new Storage<Kupovina>("kupovine.csv");
            _kupovine = _storage.Load();
        }

        public Kupovina AddKupovina(Kupovina kupovina)
        {
            _kupovine.Add(kupovina);
            _storage.Save(_kupovine);
            return kupovina;
        }

        public Kupovina? UpdateKupovina(Kupovina kupovina)
        {
            Kupovina? staraKupovina = GetKupovinaById(
                kupovina.Posetilac.BrojClanskeKarte,
                kupovina.Knjiga.ISBN,
                kupovina.DatumKupovine);

            if (staraKupovina is null)
                return null;

            staraKupovina.Posetilac = kupovina.Posetilac;
            staraKupovina.Knjiga = kupovina.Knjiga;
            staraKupovina.DatumKupovine = kupovina.DatumKupovine;
            staraKupovina.Ocena = kupovina.Ocena;
            staraKupovina.Komentar = kupovina.Komentar;

            _storage.Save(_kupovine);
            return staraKupovina;
        }

        public Kupovina? GetKupovinaById(
            string brojClanskeKarte,
            string isbn,
            DateTime datumKupovine)
        {
            foreach (Kupovina kupovina in _kupovine)
            {
                if (kupovina.Posetilac.BrojClanskeKarte == brojClanskeKarte &&
                    kupovina.Knjiga.ISBN == isbn &&
                    kupovina.DatumKupovine == datumKupovine)
                {
                    return kupovina;
                }
            }

            return null;
        }

        public Kupovina? RemoveKupovina(
            string brojClanskeKarte,
            string isbn,
            DateTime datumKupovine)
        {
            Kupovina? kupovina = GetKupovinaById(
                brojClanskeKarte, isbn, datumKupovine);

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
