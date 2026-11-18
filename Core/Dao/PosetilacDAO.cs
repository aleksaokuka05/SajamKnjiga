
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    public class PosetilacDAO
    {
        private readonly List<Posetilac> _posetioci;
        private readonly Storage<Posetilac> _storage;

        private int _sledeciId;

        public PosetilacDAO()
        {
            _storage = new Storage<Posetilac>("posetioci.csv");
            _posetioci = _storage.Load();

            _sledeciId = _posetioci.Count == 0
                ? 1
                : _posetioci.Max(posetilac => posetilac.Id) + 1;
        }

        private int GenerateId()
        {
            return _sledeciId++;
        }

        public Posetilac AddPosetilac(Posetilac posetilac)
        {
            posetilac.Id = GenerateId();

            _posetioci.Add(posetilac);
            _storage.Save(_posetioci);

            return posetilac;
        }

        public Posetilac? UpdatePosetilac(Posetilac posetilac)
        {
            Posetilac? stariPosetilac = GetPosetilacById(posetilac.Id);

            if (stariPosetilac is null)
                return null;

            stariPosetilac.Ime = posetilac.Ime;
            stariPosetilac.Prezime = posetilac.Prezime;
            stariPosetilac.DatumRodjenja = posetilac.DatumRodjenja;
            stariPosetilac.AdresaId = posetilac.AdresaId;
            stariPosetilac.Adresa = posetilac.Adresa;
            stariPosetilac.KontaktTelefon = posetilac.KontaktTelefon;
            stariPosetilac.Email = posetilac.Email;
            stariPosetilac.BrojClanskeKarte = posetilac.BrojClanskeKarte;
            stariPosetilac.TrenutnaGodinaClanstva = posetilac.TrenutnaGodinaClanstva;
            stariPosetilac.Status = posetilac.Status;
            stariPosetilac.Kupovine = posetilac.Kupovine;
            stariPosetilac.ListaZelja = posetilac.ListaZelja;

            _storage.Save(_posetioci);

            return stariPosetilac;
        }

        public Posetilac? GetPosetilacById(int id)
        {
            foreach (Posetilac posetilac in _posetioci)
            {
                if (posetilac.Id == id)
                    return posetilac;
            }

            return null;
        }

        public Posetilac? RemovePosetilac(int id)
        {
            Posetilac? posetilac = GetPosetilacById(id);

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
