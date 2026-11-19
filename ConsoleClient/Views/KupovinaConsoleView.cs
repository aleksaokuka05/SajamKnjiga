
using System;
using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Views
{
    public class KupovinaConsoleView
    {
        private readonly KupovinaDAO _kupovinaDAO;
        private readonly PosetilacDAO _posetilacDAO;
        private readonly KnjigaDAO _knjigaDAO;
        private readonly LinkService _linkService;

        public KupovinaConsoleView(
            KupovinaDAO kupovinaDAO,
            PosetilacDAO posetilacDAO,
            KnjigaDAO knjigaDAO,
            LinkService linkService)
        {
            _kupovinaDAO = kupovinaDAO;
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
                    return;

                ObradiIzbor(izbor);
                _linkService.LinkAll();
            }
        }

        private void PrikaziMeni()
        {
            Console.WriteLine();
            Console.WriteLine("=== KUPOVINE ===");
            Console.WriteLine("1 - Prikazi sve");
            Console.WriteLine("2 - Dodaj");
            Console.WriteLine("3 - Izmeni");
            Console.WriteLine("4 - Obrisi");
            Console.WriteLine("0 - Nazad");
            Console.Write("Izbor: ");
        }

        private void ObradiIzbor(string izbor)
        {
            switch (izbor)
            {
                case "1": PrikaziSveKupovine(); break;
                case "2": DodajKupovinu(); break;
                case "3": IzmeniKupovinu(); break;
                case "4": ObrisiKupovinu(); break;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }

        private void PrikaziSveKupovine()
        {
            List<Kupovina> kupovine =
                _kupovinaDAO.GetAllKupovine();

            if (kupovine.Count == 0)
            {
                Console.WriteLine("Nema kupovina.");
                return;
            }

            foreach (Kupovina kupovina in kupovine)
            {
                Posetilac? posetilac =
                    _posetilacDAO.GetPosetilacById(kupovina.PosetilacId);

                string imePosetioca = posetilac is null
                    ? "nepoznat"
                    : $"{posetilac.Ime} {posetilac.Prezime}";

                string nazivKnjige = _knjigaDAO
                    .GetKnjigaById(kupovina.KnjigaId)?.Naziv
                    ?? "nepoznata";

                Console.WriteLine(
                    $"ID: {kupovina.Id}, " +
                    $"POSETILAC: {imePosetioca}, " +
                    $"KNJIGA: {nazivKnjige}, " +
                    $"DATUM: {kupovina.DatumKupovine:dd.MM.yyyy}, " +
                    $"OCENA: {kupovina.Ocena}, " +
                    $"KOMENTAR: {kupovina.Komentar}");
            }
        }

        private void DodajKupovinu()
        {
            if (!PostojePosetiociIKnjige())
                return;

            Kupovina kupovina = UnesiKupovinu();

            _kupovinaDAO.AddKupovina(kupovina);

            Console.WriteLine("Uspesno dodato.");
        }

        private void IzmeniKupovinu()
        {
            Console.Write("ID kupovine: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            if (_kupovinaDAO.GetKupovinaById(id) is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            if (!PostojePosetiociIKnjige())
                return;

            Kupovina kupovina = UnesiKupovinu();
            kupovina.Id = id;

            Kupovina? izmenjena =
                _kupovinaDAO.UpdateKupovina(kupovina);

            Console.WriteLine(
                izmenjena is null
                    ? "Nije pronadjeno."
                    : "Uspesno izmenjeno.");
        }

        private void ObrisiKupovinu()
        {
            Console.Write("ID kupovine: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            Kupovina? obrisana =
                _kupovinaDAO.RemoveKupovina(id);

            Console.WriteLine(
                obrisana is null
                    ? "Nije pronadjeno."
                    : "Uspesno obrisano.");
        }

        private bool PostojePosetiociIKnjige()
        {
            if (_posetilacDAO.GetAllPosetioci().Count == 0)
            {
                Console.WriteLine("Prvo dodaj bar jednog posetioca.");
                return false;
            }

            if (_knjigaDAO.GetAllKnjige().Count == 0)
            {
                Console.WriteLine("Prvo dodaj bar jednu knjigu.");
                return false;
            }

            return true;
        }

        private Kupovina UnesiKupovinu()
        {
            Kupovina kupovina = new Kupovina();

            Console.Write("ID posetioca: ");

            kupovina.PosetilacId =
                ConsoleViewUtils.SiguranUnosPostojecegId(
                    id => _posetilacDAO.GetPosetilacById(id) is not null);

            kupovina.Posetilac =
                _posetilacDAO.GetPosetilacById(kupovina.PosetilacId);

            Console.Write("ID knjige: ");

            kupovina.KnjigaId =
                ConsoleViewUtils.SiguranUnosPostojecegId(
                    id => _knjigaDAO.GetKnjigaById(id) is not null);

            kupovina.Knjiga =
                _knjigaDAO.GetKnjigaById(kupovina.KnjigaId);

            Console.Write("Datum kupovine (dd.MM.yyyy): ");

            kupovina.DatumKupovine =
                ConsoleViewUtils.SiguranUnosDatuma();

            Console.Write("Ocena (1-5): ");

            kupovina.Ocena =
                ConsoleViewUtils.SiguranUnosOcene();

            Console.Write("Komentar: ");

            kupovina.Komentar =
                ConsoleViewUtils.SiguranUnosTeksta(false);

            return kupovina;
        }
    }
}