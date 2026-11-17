using Core.Interfaces;
using Core.DAO;
using Core.Models;
using System;
using System.Linq;

namespace Core.Services
{
    public class KupovinaService : IKupovinaService
    {
        private readonly IKupovinaDAO _kupovinaDAO;

        public KupovinaService()
        {
            _kupovinaDAO = new KupovinaDAO();
        }

        public List<Kupovina> GetAll()
        {
            return _kupovinaDAO.GetAll();
        }

        public void Add(Kupovina kupovina)
        {
            List<Kupovina> kupovine = _kupovinaDAO.GetAll();

            bool postoji = kupovine.Any(k =>
                k.Posetilac.BrojClanskeKarte == kupovina.Posetilac.BrojClanskeKarte &&
                k.Knjiga.ISBN == kupovina.Knjiga.ISBN &&
                k.DatumKupovine == kupovina.DatumKupovine);

            if (postoji)
            {
                throw new InvalidOperationException(
                    "Ova kupovina vec postoji.");
            }

            _kupovinaDAO.Add(kupovina);
        }

        public void Update(Kupovina kupovina)
        {
            List<Kupovina> kupovine = _kupovinaDAO.GetAll();

            bool postoji = kupovine.Any(k =>
                k.Posetilac.BrojClanskeKarte == kupovina.Posetilac.BrojClanskeKarte &&
                k.Knjiga.ISBN == kupovina.Knjiga.ISBN &&
                k.DatumKupovine == kupovina.DatumKupovine);

            if (!postoji)
            {
                throw new InvalidOperationException(
                    "Kupovina ne postoji.");
            }

            _kupovinaDAO.Update(kupovina);
        }

        public void Delete(Kupovina kupovina)
        {
            List<Kupovina> kupovine = _kupovinaDAO.GetAll();

            bool postoji = kupovine.Any(k =>
                k.Posetilac.BrojClanskeKarte == kupovina.Posetilac.BrojClanskeKarte &&
                k.Knjiga.ISBN == kupovina.Knjiga.ISBN &&
                k.DatumKupovine == kupovina.DatumKupovine);

            if (!postoji)
            {
                throw new InvalidOperationException(
                    "Kupovina ne postoji.");
            }

            _kupovinaDAO.Delete(kupovina);
        }
    }
}