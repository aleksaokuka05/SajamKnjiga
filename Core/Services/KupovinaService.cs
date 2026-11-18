
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
            bool postoji = _kupovinaDAO.GetAllKupovine()
                .Any(k => k.PosetilacId == kupovina.PosetilacId
                       && k.KnjigaId == kupovina.KnjigaId
                       && k.DatumKupovine == kupovina.DatumKupovine);

            if (postoji)
            {
                throw new InvalidOperationException(
                    "Ova kupovina vec postoji.");
            }

            _kupovinaDAO.AddKupovina(kupovina);
        }

        public void Update(Kupovina kupovina)
        {
            Kupovina? rezultat =
                _kupovinaDAO.UpdateKupovina(kupovina);

            if (rezultat is null)
            {
                throw new InvalidOperationException(
                    "Kupovina sa ovim ID-jem ne postoji.");
            }
        }

        public void Delete(Kupovina kupovina)
        {
            Kupovina? rezultat =
                _kupovinaDAO.RemoveKupovina(kupovina.Id);

            if (rezultat is null)
            {
                throw new InvalidOperationException(
                    "Kupovina sa ovim ID-jem ne postoji.");
            }
        }
    }
}
