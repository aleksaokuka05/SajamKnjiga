
using System;
using System.Collections.Generic;
using System.Linq;
using Core.DAO;
using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class PosetilacService : IPosetilacService
    {
        private readonly PosetilacDAO _posetilacDAO;

        public PosetilacService()
        {
            _posetilacDAO = new PosetilacDAO();
        }

        public List<Posetilac> GetAll()
        {
            return _posetilacDAO.GetAllPosetioci();
        }

        public void Add(Posetilac posetilac)
        {
            bool postoji = _posetilacDAO.GetAllPosetioci()
                .Any(p => p.BrojClanskeKarte == posetilac.BrojClanskeKarte);

            if (postoji)
                throw new InvalidOperationException(
                    "Posetilac sa ovim brojem clanske karte vec postoji.");

            _posetilacDAO.AddPosetilac(posetilac);
        }

        public void Update(Posetilac posetilac)
        {
            Posetilac? rezultat = _posetilacDAO.UpdatePosetilac(posetilac);

            if (rezultat is null)
                throw new InvalidOperationException(
                    "Posetilac sa ovim brojem clanske karte ne postoji.");
        }

        public void Delete(string brojClanskeKarte)
        {
            Posetilac? rezultat =
                _posetilacDAO.RemovePosetilac(brojClanskeKarte);

            if (rezultat is null)
                throw new InvalidOperationException(
                    "Posetilac sa ovim brojem clanske karte ne postoji.");
        }
    }
}
