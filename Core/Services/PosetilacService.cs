using Core.Interfaces;
using Core.DAO;
using Core.Models;
using System;
using System.Linq;

namespace Core.Services
{
    public class PosetilacService : IPosetilacService
    {
        private readonly IPosetilacDAO _posetilacDAO;

        public PosetilacService()
        {
            _posetilacDAO = new PosetilacDAO();
        }
        public List<Posetilac> GetAll()
        {
            return _posetilacDAO.GetAll();
        }

        public void Add(Posetilac posetilac)
        {
            List<Posetilac> posetioci = _posetilacDAO.GetAll();

            bool postoji = posetioci.Any(p =>
                p.BrojClanskeKarte == posetilac.BrojClanskeKarte);

            if (postoji)
            {
                throw new InvalidOperationException(
                    "Posetilac sa ovim brojem clanske karte vec postoji.");
            }

            _posetilacDAO.Add(posetilac);
        }

        public void Update(Posetilac posetilac)
        {
            List<Posetilac> posetioci = _posetilacDAO.GetAll();

            bool postoji = posetioci.Any(p =>
                p.BrojClanskeKarte == posetilac.BrojClanskeKarte);

            if (!postoji)
            {
                throw new InvalidOperationException(
                    "Posetilac sa ovim brojem clanske karte ne postoji.");
            }

            _posetilacDAO.Update(posetilac);
        }

        public void Delete(string brojClanskeKarte)
        {
            List<Posetilac> posetioci = _posetilacDAO.GetAll();

            bool postoji = posetioci.Any(p =>
                p.BrojClanskeKarte == brojClanskeKarte);

            if (!postoji)
            {
                throw new InvalidOperationException(
                    "Posetilac sa ovim brojem clanske karte ne postoji.");
            }

            _posetilacDAO.Delete(brojClanskeKarte);
        }
    }
}