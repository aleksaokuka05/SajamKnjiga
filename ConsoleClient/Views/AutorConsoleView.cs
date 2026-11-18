
using System;
using System.Collections.Generic;
using System.Linq;
using Core.DAO;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Views
{
    public class AutorConsoleView
    {
        private readonly AutorDAO _autorDAO;
        private readonly AdresaDAO _adresaDAO;
        private readonly KnjigaAutorDAO _knjigaAutorDAO;
        private readonly IzdavacDAO _izdavacDAO;
        private readonly KnjigaDAO _knjigaDAO;
        private readonly LinkService _linkService;

        public AutorConsoleView(
            AutorDAO autorDAO,
            AdresaDAO adresaDAO,
            KnjigaAutorDAO knjigaAutorDAO,
            IzdavacDAO izdavacDAO,
            KnjigaDAO knjigaDAO,
            LinkService linkService)
        {
            _autorDAO = autorDAO;
            _adresaDAO = adresaDAO;
            _knjigaAutorDAO = knjigaAutorDAO;
            _izdavacDAO = izdavacDAO;
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
                _linkService.LinkAll();
            }
        }

        private void PrikaziMeni()
        {
            Console.WriteLine();
            Console.WriteLine("=== AUTORI ===");
            Console.WriteLine("1 - Prikazi sve");
            Console.WriteLine("2 - Dodaj");
            Console.WriteLine("3 - Izmeni");
            Console.WriteLine("4 - Obrisi");
            Console.WriteLine("5 - Knjige autora");
            Console.WriteLine("0 - Nazad");
            Console.Write("Izbor: ");
        }

        private void ObradiIzbor(string izbor)
        {
            switch (izbor)
            {
                case "1": PrikaziSveAutore(); break;
                case "2": DodajAutora(); break;
                case "3": IzmeniAutora(); break;
                case "4": ObrisiAutora(); break;
                case "5": MeniKnjigaAutora(); break;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }

        private void MeniKnjigaAutora()
        {
            Console.Write("ID autora: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            Autor? autor = _autorDAO.GetAutorById(id);

            if (autor is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"=== KNJIGE AUTORA: {autor.Ime} {autor.Prezime} ===");

                PrikaziKnjigeAutora(autor);

                Console.WriteLine("1 - Dodaj knjigu");
                Console.WriteLine("2 - Ukloni knjigu");
                Console.WriteLine("0 - Nazad");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine() ?? "0";

                if (izbor == "0")
                {
                    return;
                }

                switch (izbor)
                {
                    case "1": DodajKnjiguAutoru(autor); break;
                    case "2": UkloniKnjiguAutoru(autor); break;
                    default: Console.WriteLine("Nepoznata opcija."); break;
                }
            }
        }

        private void PrikaziKnjigeAutora(Autor autor)
        {
            if (autor.Knjige.Count == 0)
            {
                Console.WriteLine("Autor nema knjiga.");
                return;
            }

            foreach (Knjiga knjiga in autor.Knjige)
            {
                Console.WriteLine(
                    $"ID: {knjiga.Id}, ISBN: {knjiga.ISBN}, NASLOV: {knjiga.Naziv}, GODINA: {knjiga.GodinaIzdanja}");
            }
        }

        private void DodajKnjiguAutoru(Autor autor)
        {
            if (_knjigaDAO.GetAllKnjige().Count == 0)
            {
                Console.WriteLine("Prvo dodaj bar jednu knjigu.");
                return;
            }

            Console.Write("ID knjige: ");

            int knjigaId = ConsoleViewUtils.SiguranUnosPostojecegId(
                id => _knjigaDAO.GetKnjigaById(id) is not null);

            try
            {
                _linkService.DodajKnjigaAutor(knjigaId, autor.Id);
                Console.WriteLine("Uspesno dodato.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void UkloniKnjiguAutoru(Autor autor)
        {
            Console.Write("ID knjige: ");
            int knjigaId = ConsoleViewUtils.SiguranUnosCelogBroja();

            KnjigaAutor? obrisana =
                _linkService.ObrisiKnjigaAutor(knjigaId, autor.Id);

            Console.WriteLine(
                obrisana is null ? "Autor nema tu knjigu." : "Uspesno uklonjeno.");
        }

        private void PrikaziSveAutore()
        {
            List<Autor> autori = _autorDAO.GetAllAutori();

            if (autori.Count == 0)
            {
                Console.WriteLine("Nema autora.");
                return;
            }

            foreach (Autor autor in autori)
            {
                string adresa = ConsoleViewUtils.AdresaUTekst(
                    _adresaDAO.GetAdresaById(autor.AdresaId));

                Console.WriteLine(
                    $"ID: {autor.Id}, IME: {autor.Ime} {autor.Prezime}, DATUM: {autor.DatumRodjenja:dd.MM.yyyy}, ADRESA: {adresa}, TELEFON: {autor.Telefon}, EMAIL: {autor.Imejl}, LK: {autor.BrojLicneKarte}, ISKUSTVO: {autor.GodineIskustva}");
            }
        }

        private void DodajAutora()
        {
            if (!PostojeAdrese())
            {
                return;
            }

            Autor autor = UnesiAutora();

            try
            {
                _autorDAO.AddAutor(autor);
                Console.WriteLine("Uspesno dodato.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void IzmeniAutora()
        {
            Console.Write("ID autora: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            if (_autorDAO.GetAutorById(id) is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            if (!PostojeAdrese())
            {
                return;
            }

            Autor autor = UnesiAutora();
            autor.Id = id;

            if (DaLiSefImaPremaloIskustva(autor))
            {
                return;
            }

            try
            {
                Autor? izmenjen = _autorDAO.UpdateAutor(autor);

                Console.WriteLine(
                    izmenjen is null ? "Nije pronadjeno." : "Uspesno izmenjeno.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ObrisiAutora()
        {
            Console.Write("ID autora: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            if (_autorDAO.GetAutorById(id) is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            if (DaLiSeAutorKoristi(id))
            {
                return;
            }

            _autorDAO.RemoveAutor(id);
            Console.WriteLine("Uspesno obrisano.");
        }

        private bool PostojeAdrese()
        {
            if (_adresaDAO.GetAllAdrese().Count > 0)
            {
                return true;
            }

            Console.WriteLine("Prvo dodaj bar jednu adresu.");
            return false;
        }

        private bool DaLiSeAutorKoristi(int autorId)
        {
            int brojKnjiga = _knjigaAutorDAO
                .PronadjiKnjigeAutora(autorId).Count;

            int brojIzdavaca = _izdavacDAO.GetAllIzdavaci()
                .FindAll(i => i.SefIzdavacaId == autorId).Count;

            if (brojKnjiga == 0 && brojIzdavaca == 0)
            {
                return false;
            }

            Console.WriteLine(
                $"Autor ne moze da se obrise jer je povezan sa drugim podacima (knjige: {brojKnjiga}, sef izdavaca: {brojIzdavaca}).");

            return true;
        }

        private bool DaLiSefImaPremaloIskustva(Autor autor)
        {
            List<Izdavac> izdavaci = _izdavacDAO.GetAllIzdavaci()
                .FindAll(i => i.SefIzdavacaId == autor.Id);

            if (izdavaci.Count == 0 ||
                autor.GodineIskustva >= PublisherService.MinimalnoIskustvoSefa)
            {
                return false;
            }

            string nazivi = string.Join(", ", izdavaci.Select(i => i.Naziv));

            Console.WriteLine(
                $"Autor je sef izdavaca ({nazivi}) i mora imati najmanje {PublisherService.MinimalnoIskustvoSefa} godina iskustva.");

            return true;
        }

        private Autor UnesiAutora()
        {
            Autor autor = new Autor();

            Console.Write("Ime: ");
            autor.Ime = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Prezime: ");
            autor.Prezime = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Datum rodjenja (dd.MM.yyyy): ");
            autor.DatumRodjenja = ConsoleViewUtils.SiguranUnosDatuma();

            Console.Write("ID adrese: ");
            autor.AdresaId = ConsoleViewUtils.SiguranUnosPostojecegId(
                id => _adresaDAO.GetAdresaById(id) is not null);

            Console.Write("Telefon: ");
            autor.Telefon = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Email: ");
            autor.Imejl = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Broj licne karte: ");
            autor.BrojLicneKarte = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Godine iskustva: ");
            autor.GodineIskustva = ConsoleViewUtils.SiguranUnosCelogBroja();

            return autor;
        }
    }
}
