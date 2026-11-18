
using System;
using System.Collections.Generic;
using System.Linq;
using Core.DAO;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Views
{
    public class KnjigaConsoleView
    {
        private readonly KnjigaDAO _knjigaDAO;
        private readonly IzdavacDAO _izdavacDAO;
        private readonly KupovinaDAO _kupovinaDAO;
        private readonly KnjigaAutorDAO _knjigaAutorDAO;
        private readonly StavkaListeZeljaDAO _stavkaListeZeljaDAO;
        private readonly LinkService _linkService;

        public KnjigaConsoleView(
            KnjigaDAO knjigaDAO,
            IzdavacDAO izdavacDAO,
            KupovinaDAO kupovinaDAO,
            KnjigaAutorDAO knjigaAutorDAO,
            StavkaListeZeljaDAO stavkaListeZeljaDAO,
            LinkService linkService)
        {
            _knjigaDAO = knjigaDAO;
            _izdavacDAO = izdavacDAO;
            _kupovinaDAO = kupovinaDAO;
            _knjigaAutorDAO = knjigaAutorDAO;
            _stavkaListeZeljaDAO = stavkaListeZeljaDAO;
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
            Console.WriteLine("=== KNJIGE ===");
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
                case "1": PrikaziSveKnjige(); break;
                case "2": DodajKnjigu(); break;
                case "3": IzmeniKnjigu(); break;
                case "4": ObrisiKnjigu(); break;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }

        private void PrikaziSveKnjige()
        {
            List<Knjiga> knjige = _knjigaDAO.GetAllKnjige();

            if (knjige.Count == 0)
            {
                Console.WriteLine("Nema knjiga.");
                return;
            }

            foreach (Knjiga knjiga in knjige)
            {
                string izdavac = _izdavacDAO.GetIzdavacById(knjiga.IzdavacId)?.Naziv ?? "nepoznat";

                string autori = knjiga.Autori.Count == 0
                    ? "nema"
                    : string.Join(", ", knjiga.Autori.Select(autor => $"{autor.Ime} {autor.Prezime}"));

                Console.WriteLine(
                    $"ID: {knjiga.Id}, ISBN: {knjiga.ISBN}, NASLOV: {knjiga.Naziv}, ZANR: {knjiga.Zanr}, GODINA: {knjiga.GodinaIzdanja}, CENA: {knjiga.Cena:F2}, STRANA: {knjiga.BrojStrana}, IZDAVAC: {izdavac}, AUTORI: {autori}");
            }
        }

        private void DodajKnjigu()
        {
            if (!PostojeIzdavaci())
            {
                return;
            }

            Knjiga knjiga = UnesiKnjigu();

            try
            {
                _knjigaDAO.AddKnjiga(knjiga);
                Console.WriteLine("Uspesno dodato.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void IzmeniKnjigu()
        {
            Console.Write("ID knjige: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            if (_knjigaDAO.GetKnjigaById(id) is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            if (!PostojeIzdavaci())
            {
                return;
            }

            Knjiga knjiga = UnesiKnjigu();
            knjiga.Id = id;

            try
            {
                Knjiga? izmenjena = _knjigaDAO.UpdateKnjiga(knjiga);

                Console.WriteLine(
                    izmenjena is null ? "Nije pronadjeno." : "Uspesno izmenjeno.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ObrisiKnjigu()
        {
            Console.Write("ID knjige: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            if (_knjigaDAO.GetKnjigaById(id) is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            if (DaLiSeKnjigaKoristi(id))
            {
                return;
            }

            _knjigaDAO.RemoveKnjiga(id);
            Console.WriteLine("Uspesno obrisano.");
        }

        private bool PostojeIzdavaci()
        {
            if (_izdavacDAO.GetAllIzdavaci().Count > 0)
            {
                return true;
            }

            Console.WriteLine("Prvo dodaj bar jednog izdavaca.");
            return false;
        }

        private bool DaLiSeKnjigaKoristi(int knjigaId)
        {
            int brojKupovina = _kupovinaDAO.GetAllKupovine()
                .FindAll(k => k.KnjigaId == knjigaId).Count;

            int brojAutora = _knjigaAutorDAO
                .PronadjiAutoreKnjige(knjigaId).Count;

            int brojListaZelja = _stavkaListeZeljaDAO
                .PronadjiPosetioceZaKnjigu(knjigaId).Count;

            if (brojKupovina == 0 && brojAutora == 0 && brojListaZelja == 0)
            {
                return false;
            }

            Console.WriteLine(
                $"Knjiga ne moze da se obrise jer je povezana sa drugim podacima (kupovine: {brojKupovina}, autori: {brojAutora}, liste zelja: {brojListaZelja}).");

            return true;
        }

        private Knjiga UnesiKnjigu()
        {
            Knjiga knjiga = new Knjiga();

            Console.Write("ISBN: ");
            knjiga.ISBN = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Naslov: ");
            knjiga.Naziv = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Zanr: ");
            knjiga.Zanr = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Godina izdanja: ");
            knjiga.GodinaIzdanja = ConsoleViewUtils.SiguranUnosCelogBroja();

            Console.Write("Cena: ");
            knjiga.Cena = ConsoleViewUtils.SiguranUnosDecimalnogBroja();

            Console.Write("Broj strana: ");
            knjiga.BrojStrana = ConsoleViewUtils.SiguranUnosCelogBroja();

            Console.Write("ID izdavaca: ");
            knjiga.IzdavacId = ConsoleViewUtils.SiguranUnosPostojecegId(
                id => _izdavacDAO.GetIzdavacById(id) is not null);

            return knjiga;
        }
    }
}
