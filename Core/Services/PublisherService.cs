
using System;
using System.Collections.Generic;
using Core.DAO;
using Core.Models;

namespace Core.Services
{
    public class PublisherService
    {
        public const int MinimalnoIskustvoSefa = 5;

        private readonly IzdavacDAO _izdavacDAO;
        private readonly AutorDAO _autorDAO;
        private readonly KnjigaDAO _knjigaDAO;

        public PublisherService(
            IzdavacDAO izdavacDAO,
            AutorDAO autorDAO,
            KnjigaDAO knjigaDAO)
        {
            _izdavacDAO = izdavacDAO;
            _autorDAO = autorDAO;
            _knjigaDAO = knjigaDAO;
        }

        public Izdavac DodajIzdavaca(Izdavac izdavac)
        {
            ProveriSefaIzdavaca(izdavac.SefIzdavacaId);
            return _izdavacDAO.AddIzdavac(izdavac);
        }

        public Izdavac? IzmeniIzdavaca(Izdavac izdavac)
        {
            ProveriSefaIzdavaca(izdavac.SefIzdavacaId);
            return _izdavacDAO.UpdateIzdavac(izdavac);
        }

        public Izdavac? ObrisiIzdavaca(int id)
        {
            int brojKnjiga = 0;

            foreach (Knjiga knjiga in _knjigaDAO.GetAllKnjige())
            {
                if (knjiga.IzdavacId == id)
                {
                    brojKnjiga++;
                }
            }

            if (brojKnjiga > 0)
            {
                throw new InvalidOperationException(
                    $"Izdavac ne moze da se obrise jer ima knjige ({brojKnjiga}).");
            }

            return _izdavacDAO.RemoveIzdavac(id);
        }

        public Autor? PronadjiSefaIzdavaca(Izdavac izdavac)
        {
            if (izdavac.SefIzdavacaId == 0)
            {
                return null;
            }

            return _autorDAO.GetAutorById(izdavac.SefIzdavacaId);
        }

        public Izdavac? PronadjiIzdavacaPoId(int id)
        {
            return _izdavacDAO.GetIzdavacById(id);
        }

        public List<Izdavac> PrikaziSveIzdavace()
        {
            return _izdavacDAO.GetAllIzdavaci();
        }

        private void ProveriSefaIzdavaca(int sefIzdavacaId)
        {
            if (sefIzdavacaId == 0)
            {
                return;
            }

            Autor? sefIzdavaca =
                _autorDAO.GetAutorById(sefIzdavacaId);

            if (sefIzdavaca is null)
            {
                throw new InvalidOperationException(
                    $"Autor sa ID {sefIzdavacaId} ne postoji.");
            }

            if (sefIzdavaca.GodineIskustva < MinimalnoIskustvoSefa)
            {
                throw new InvalidOperationException(
                    $"Sef izdavaca mora imati najmanje {MinimalnoIskustvoSefa} godina iskustva " +
                    $"(autor {sefIzdavaca.Ime} {sefIzdavaca.Prezime} ima {sefIzdavaca.GodineIskustva}).");
            }
        }
    }
}
