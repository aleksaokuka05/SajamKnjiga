
using System;
using System.Collections.Generic;
using System.Linq;
using Core.DAO;
using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class IzdavacService : IIzdavacService
    {
        private readonly IzdavacDAO _izdavacDAO;

        public IzdavacService()
        {
            _izdavacDAO = new IzdavacDAO();
        }

        public List<Izdavac> GetAll()
        {
            return _izdavacDAO.GetAllIzdavaci();
        }

        public void Add(Izdavac izdavac)
        {
            bool postoji = _izdavacDAO.GetAllIzdavaci()
                .Any(i => i.Sifra == izdavac.Sifra);

            if (postoji)
            {
                throw new InvalidOperationException(
                    "Izdavac sa ovom sifrom vec postoji.");
            }

            _izdavacDAO.AddIzdavac(izdavac);
        }

        public void Update(Izdavac izdavac)
        {
            Izdavac? rezultat =
                _izdavacDAO.UpdateIzdavac(izdavac);

            if (rezultat is null)
            {
                throw new InvalidOperationException(
                    "Izdavac sa ovim ID-jem ne postoji.");
            }
        }

        public void Delete(int id)
        {
            Izdavac? rezultat =
                _izdavacDAO.RemoveIzdavac(id);

            if (rezultat is null)
            {
                throw new InvalidOperationException(
                    "Izdavac sa ovim ID-jem ne postoji.");
            }
        }
    }
}
