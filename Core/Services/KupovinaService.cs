
using System;
using System.Collections.Generic;
using System.Linq;
using Core.DAO;
using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class KupovinaService : IKupovinaService
    {
        private readonly KupovinaDAO _kupovinaDAO;

        public KupovinaService()
        {
            _kupovinaDAO = new KupovinaDAO();
        }

        public List<Kupovina> GetAll()
        {
            return _kupovinaDAO.GetAllKupovine();
        }

        public void Add(Kupovina kupovina)
        {
            bool postoji = _kupovinaDAO.GetAllKupovine().Any(k =>
                k.Posetilac.BrojClanskeKarte == kupovina.Posetilac.BrojClanskeKarte &&
                k.Knjiga.ISBN == kupovina.Knjiga.ISBN &&
                k.DatumKupovine == kupovina.DatumKupovine);

            if (postoji)
                throw new InvalidOperationException(
                    "Ova kupovina vec postoji.");

            _kupovinaDAO.AddKupovina(kupovina);
        }

        public void Update(Kupovina kupovina)
        {
            Kupovina? rezultat = _kupovinaDAO.UpdateKupovina(kupovina);

            if (rezultat is null)
                throw new InvalidOperationException(
                    "Kupovina ne postoji.");
        }

        public void Delete(Kupovina kupovina)
        {
            Kupovina? rezultat = _kupovinaDAO.RemoveKupovina(
                kupovina.Posetilac.BrojClanskeKarte,
                kupovina.Knjiga.ISBN,
                kupovina.DatumKupovine);

            if (rezultat is null)
                throw new InvalidOperationException("Kupovina ne postoji.");
        }
    }
}
