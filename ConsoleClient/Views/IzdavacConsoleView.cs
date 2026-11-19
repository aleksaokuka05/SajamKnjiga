using System;
using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Views
{
    public class IzdavacConsoleView
    {
        private readonly PublisherService _publisherService;
        private readonly LinkService _linkService;
        private readonly AutorDAO _autorDAO;

        public IzdavacConsoleView(
            PublisherService publisherService,
            LinkService linkService,
            AutorDAO autorDAO)
        {
            _publisherService = publisherService;
            _linkService = linkService;
            _autorDAO = autorDAO;
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
            Console.WriteLine("=== IZDAVACI ===");
            Console.WriteLine("1 - Prikazi sve");
            Console.WriteLine("2 - Dodaj");
            Console.WriteLine("3 - Izmeni");
            Console.WriteLine("4 - Obrisi");
            Console.WriteLine("5 - Knjige i autori izdavaca");
            Console.WriteLine("0 - Nazad");
            Console.Write("Izbor: ");
        }

        private void ObradiIzbor(string izbor)
        {
            switch (izbor)
            {
                case "1": PrikaziSveIzdavace(); break;
                case "2": DodajIzdavaca(); break;
                case "3": IzmeniIzdavaca(); break;
                case "4": ObrisiIzdavaca(); break;
                case "5": PrikaziKnjigeIAutoreIzdavaca(); break;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }

        private void PrikaziKnjigeIAutoreIzdavaca()
        {
            Console.Write("ID izdavaca: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            Izdavac? izdavac =
                _publisherService.PronadjiIzdavacaPoId(id);

            if (izdavac is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            _linkService.LinkAll();

            Console.WriteLine();
            Console.WriteLine(
                $"=== KNJIGE IZDAVACA: {izdavac.Naziv} ===");

            if (izdavac.Knjige.Count == 0)
            {
                Console.WriteLine("Izdavac nema knjiga.");
            }

            foreach (Knjiga knjiga in izdavac.Knjige)
            {
                Console.WriteLine(
                    $"ID: {knjiga.Id}, " +
                    $"ISBN: {knjiga.ISBN}, " +
                    $"NASLOV: {knjiga.Naziv}, " +
                    $"GODINA: {knjiga.GodinaIzdanja}");
            }

            Console.WriteLine(
                $"=== AUTORI IZDAVACA: {izdavac.Naziv} ===");

            if (izdavac.Autori.Count == 0)
            {
                Console.WriteLine("Izdavac nema autora.");
            }

            foreach (Autor autor in izdavac.Autori)
            {
                Console.WriteLine(
                    $"ID: {autor.Id}, " +
                    $"IME: {autor.Ime} {autor.Prezime}");
            }
        }

        private void PrikaziSveIzdavace()
        {
            List<Izdavac> izdavaci =
                _publisherService.PrikaziSveIzdavace();

            if (izdavaci.Count == 0)
            {
                Console.WriteLine("Nema izdavaca.");
                return;
            }

            foreach (Izdavac izdavac in izdavaci)
            {
                Console.WriteLine(
                    $"ID: {izdavac.Id}, " +
                    $"SIFRA: {izdavac.Sifra}, " +
                    $"NAZIV: {izdavac.Naziv}, " +
                    $"SEF: {SefIzdavacaUTekst(izdavac)}");
            }
        }

        private void DodajIzdavaca()
        {
            if (!PostojeAutori())
                return;

            Izdavac izdavac = UnesiIzdavaca();

            try
            {
                _publisherService.DodajIzdavaca(izdavac);
                Console.WriteLine("Uspesno dodat izdavac.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void IzmeniIzdavaca()
        {
            Console.Write("ID izdavaca: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            if (_publisherService.PronadjiIzdavacaPoId(id) is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            if (!PostojeAutori())
                return;

            Izdavac izdavac = UnesiIzdavaca();
            izdavac.Id = id;

            try
            {
                Izdavac? izmenjen =
                    _publisherService.IzmeniIzdavaca(izdavac);

                Console.WriteLine(
                    izmenjen is null
                        ? "Nije pronadjeno."
                        : "Uspesno izmenjen.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ObrisiIzdavaca()
        {
            Console.Write("ID izdavaca: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            try
            {
                Izdavac? obrisan =
                    _publisherService.ObrisiIzdavaca(id);

                Console.WriteLine(
                    obrisan is null
                        ? "Nije pronadjeno."
                        : "Uspesno obrisan.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string SefIzdavacaUTekst(Izdavac izdavac)
        {
            if (izdavac.SefIzdavacaId == 0)
                return "nema";

            Autor? sefIzdavaca =
                _publisherService.PronadjiSefaIzdavaca(izdavac);

            return sefIzdavaca is null
                ? "nepoznat"
                : $"{sefIzdavaca.Ime} {sefIzdavaca.Prezime}";
        }

        private bool PostojeAutori()
        {
            if (_autorDAO.GetAllAutori().Count > 0)
                return true;

            Console.WriteLine("Prvo dodaj bar jednog autora.");
            return false;
        }

        private Izdavac UnesiIzdavaca()
        {
            Izdavac izdavac = new Izdavac();

            Console.Write("Sifra izdavaca: ");
            izdavac.Sifra =
                ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Naziv: ");
            izdavac.Naziv =
                ConsoleViewUtils.SiguranUnosTeksta();

            Console.WriteLine("Dostupni autori:");

            foreach (Autor autor in _autorDAO.GetAllAutori())
            {
                Console.WriteLine(
                    $"ID: {autor.Id}, " +
                    $"{autor.Ime} {autor.Prezime}, " +
                    $"ISKUSTVO: {autor.GodineIskustva}");
            }

            Console.Write("ID autora koji je sef izdavaca: ");

            izdavac.SefIzdavacaId =
                ConsoleViewUtils.SiguranUnosPostojecegId(
                    id => _autorDAO.GetAutorById(id) is not null);

            izdavac.SefIzdavaca =
                _autorDAO.GetAutorById(izdavac.SefIzdavacaId);

            return izdavac;
        }
    }
}