using System;
using System.Collections.Generic;

using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    public class PosetilacDAO
    {
        private readonly List<Posetilac> _posetioci;
        private readonly Storage<Posetilac> _storage;

        public PosetilacDAO()
        {
            _storage = new Storage<Posetilac>("posetioci.csv");
            _posetioci = _storage.Load();
        }

        public Posetilac AddPosetilac(Posetilac posetilac)
        {
            _posetioci.Add(posetilac);
            _storage.Save(_posetioci);
            return posetilac;
        }

        public Posetilac? UpdatePosetilac(Posetilac posetilac)
        {
            Posetilac? stariPosetilac =
                GetPosetilacByBrojClanskeKarte(posetilac.BrojClanskeKarte);

            if (stariPosetilac is null)
                return null;

            stariPosetilac.Ime = posetilac.Ime;
            stariPosetilac.Prezime = posetilac.Prezime;
            stariPosetilac.DatumRodjenja = posetilac.DatumRodjenja;
            stariPosetilac.Adresa = posetilac.Adresa;
            stariPosetilac.KontaktTelefon = posetilac.KontaktTelefon;
            stariPosetilac.Email = posetilac.Email;
            stariPosetilac.TrenutnaGodinaClanstva = posetilac.TrenutnaGodinaClanstva;
            stariPosetilac.Status = posetilac.Status;
            stariPosetilac.ProsecnaOcena = posetilac.ProsecnaOcena;
            stariPosetilac.Kupovine = posetilac.Kupovine;
            stariPosetilac.ListaZelja = posetilac.ListaZelja;

            _storage.Save(_posetioci);
            return stariPosetilac;
        }

        public Posetilac? GetPosetilacByBrojClanskeKarte(string brojClanskeKarte)
        {
            foreach (Posetilac posetilac in _posetioci)
            {
                if (posetilac.BrojClanskeKarte == brojClanskeKarte)
                    return posetilac;
            }

            return null;
        }

        public Posetilac? RemovePosetilac(string brojClanskeKarte)
        {
            Posetilac? posetilac =
                GetPosetilacByBrojClanskeKarte(brojClanskeKarte);

            if (posetilac is null)
                return null;

            _posetioci.Remove(posetilac);
            _storage.Save(_posetioci);

            return posetilac;
        }

        public List<Posetilac> GetAllPosetioci()
        {
            return _posetioci;
        }
    }
}
