
using System;
using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Views
{
    public class StavkaListeZeljaConsoleView
    {
        private readonly StavkaListeZeljaDAO _stavkaListeZeljaDAO;
        private readonly PosetilacDAO _posetilacDAO;
        private readonly KnjigaDAO _knjigaDAO;
        private readonly LinkService _linkService;

        public StavkaListeZeljaConsoleView(
            StavkaListeZeljaDAO stavkaListeZeljaDAO,
            PosetilacDAO posetilacDAO,
            KnjigaDAO knjigaDAO,
            LinkService linkService)
        {
            _stavkaListeZeljaDAO = stavkaListeZeljaDAO;
            _posetilacDAO = posetilacDAO;
            _knjigaDAO = knjigaDAO;
            _linkService = linkService;
        }

        public void RunMenu()
        {
            while (true)
            {
                PrikaziMeni();
                string izbor = Console.ReadLine() ?? "0";

                if (izbor == "0")
                {
                    return;
                }

                ObradiIzbor(izbor);
            }
        }

        private void PrikaziMeni()
        {
            Console.WriteLine();
            Console.WriteLine("=== LISTA ZELJA ===");
            Console.WriteLine("1 - Prikazi sve veze");
            Console.WriteLine("2 - Dodaj vezu");
            Console.WriteLine("3 - Obrisi vezu");
            Console.WriteLine("0 - Nazad");
            Console.Write("Izbor: ");
        }

        private void ObradiIzbor(string izbor)
        {
            switch (izbor)
            {
                case "1": PrikaziSveStavkeListeZelja(); break;
                case "2": DodajStavkuListeZelja(); break;
                case "3": ObrisiStavkuListeZelja(); break;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }

        private void PrikaziSveStavkeListeZelja()
        {
            List<StavkaListeZelja> stavke =
                _stavkaListeZeljaDAO.PrikaziSveStavkeListeZelja();

            if (stavke.Count == 0)
            {
                Console.WriteLine("Nema veza.");
                return;
            }

            foreach (StavkaListeZelja stavka in stavke)
            {
                Posetilac? posetilac =
                    _posetilacDAO.GetPosetilacById(stavka.PosetilacId);

                string imePosetioca = posetilac is null
                    ? "nepoznat"
                    : $"{posetilac.Ime} {posetilac.Prezime}";

                string nazivKnjige = _knjigaDAO
                    .GetKnjigaById(stavka.KnjigaId)?.Naziv ?? "nepoznata";

                Console.WriteLine(
                    $"POSETILAC: {imePosetioca} (ID {stavka.PosetilacId}), KNJIGA: {nazivKnjige} (ID {stavka.KnjigaId})");
            }
        }

        private void DodajStavkuListeZelja()
        {
            if (_posetilacDAO.GetAllPosetioci().Count == 0)
            {
                Console.WriteLine("Prvo dodaj bar jednog posetioca.");
                return;
            }

            if (_knjigaDAO.GetAllKnjige().Count == 0)
            {
                Console.WriteLine("Prvo dodaj bar jednu knjigu.");
                return;
            }

            Console.Write("ID posetioca: ");
            int posetilacId = ConsoleViewUtils.SiguranUnosPostojecegId(
                id => _posetilacDAO.GetPosetilacById(id) is not null);

            Console.Write("ID knjige: ");
            int knjigaId = ConsoleViewUtils.SiguranUnosPostojecegId(
                id => _knjigaDAO.GetKnjigaById(id) is not null);

            try
            {
                _linkService.DodajStavkuListeZelja(posetilacId, knjigaId);
                Console.WriteLine("Uspesno dodato.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ObrisiStavkuListeZelja()
        {
            Console.Write("ID posetioca: ");
            int posetilacId = ConsoleViewUtils.SiguranUnosCelogBroja();

            Console.Write("ID knjige: ");
            int knjigaId = ConsoleViewUtils.SiguranUnosCelogBroja();

            StavkaListeZelja? obrisana =
                _linkService.ObrisiStavkuListeZelja(posetilacId, knjigaId);

            Console.WriteLine(
                obrisana is null ? "Nije pronadjeno." : "Uspesno obrisano.");
        }
    }
}
