using Core.Interfaces;
using Core.DAO;
using Core.Models;
using System;
using System.Linq;

namespace Core.Services
{
    public class IzdavacService : IIzdavacService
    {
        private readonly IIzdavacDAO _izdavacDAO;

        public IzdavacService()
        {
            _izdavacDAO = new IzdavacDAO();
        }

        public List<Izdavac> GetAll()
        {
            return _izdavacDAO.GetAll();
        }

        public void Add(Izdavac izdavac)
        {
            List<Izdavac> izdavaci = _izdavacDAO.GetAll();

            bool postoji = izdavaci.Any(i =>
                i.Sifra == izdavac.Sifra);

            if (postoji)
            {
                throw new InvalidOperationException(
                    "Izdavac sa ovom sifrom vec postoji.");
            }

            _izdavacDAO.Add(izdavac);
        }

        public void Update(Izdavac izdavac)
        {
            List<Izdavac> izdavaci = _izdavacDAO.GetAll();

            bool postoji = izdavaci.Any(i =>
                i.Sifra == izdavac.Sifra);

            if (!postoji)
            {
                throw new InvalidOperationException(
                    "Izdavac sa ovom sifrom ne postoji.");
            }

            _izdavacDAO.Update(izdavac);
        }

        public void Delete(string sifra)
        {
            List<Izdavac> izdavaci = _izdavacDAO.GetAll();

            bool postoji = izdavaci.Any(i =>
                i.Sifra == sifra);

            if (!postoji)
            {
                throw new InvalidOperationException(
                    "Izdavac sa ovom sifrom ne postoji.");
            }

            _izdavacDAO.Delete(sifra);
        }
    }
}