using System;
using System.Collections.Generic;
using System.Linq;
using Core.DAO;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Views
{
    public class PosetilacConsoleView
    {
        private readonly PosetilacDAO _posetilacDAO;
        private readonly AdresaDAO _adresaDAO;
        private readonly KupovinaDAO _kupovinaDAO;
        private readonly StavkaListeZeljaDAO _stavkaListeZeljaDAO;
        private readonly KnjigaDAO _knjigaDAO;
        private readonly LinkService _linkService;

        public PosetilacConsoleView(
            PosetilacDAO posetilacDAO,
            AdresaDAO adresaDAO,
            KupovinaDAO kupovinaDAO,
            StavkaListeZeljaDAO stavkaListeZeljaDAO,
            KnjigaDAO knjigaDAO,
            LinkService linkService)
        {
            _posetilacDAO = posetilacDAO;
            _adresaDAO = adresaDAO;
            _kupovinaDAO = kupovinaDAO;
            _stavkaListeZeljaDAO = stavkaListeZeljaDAO;
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
            Console.WriteLine("=== POSETIOCI ===");
            Console.WriteLine("1 - Prikazi sve");
            Console.WriteLine("2 - Dodaj");
            Console.WriteLine("3 - Izmeni");
            Console.WriteLine("4 - Obrisi");
            Console.WriteLine("5 - Kupljene knjige");
            Console.WriteLine("6 - Lista zelja");
            Console.WriteLine("0 - Nazad");
            Console.Write("Izbor: ");
        }

        private void ObradiIzbor(string izbor)
        {
            switch (izbor)
            {
                case "1": PrikaziSvePosetioce(); break;
                case "2": DodajPosetioca(); break;
                case "3": IzmeniPosetioca(); break;
                case "4": ObrisiPosetioca(); break;
                case "5": PrikaziKupljeneKnjige(); break;
                case "6": PokreniMeniListeZelja(); break;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }

        private Posetilac? UnesiPostojecegPosetioca()
        {
            Console.Write("ID posetioca: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            Posetilac? posetilac = _posetilacDAO.GetPosetilacById(id);

            if (posetilac is null)
                Console.WriteLine("Nije pronadjeno.");

            return posetilac;
        }

        private void PrikaziKupljeneKnjige()
        {
            Posetilac? posetilac = UnesiPostojecegPosetioca();

            if (posetilac is null)
                return;

            _linkService.LinkAll();

            Console.WriteLine();
            Console.WriteLine(
                $"=== KUPLJENE KNJIGE: {posetilac.Ime} {posetilac.Prezime} ===");

            if (posetilac.Kupovine.Count == 0)
            {
                Console.WriteLine("Posetilac nema kupovina.");
                return;
            }

            foreach (Kupovina kupovina in posetilac.Kupovine)
            {
                string nazivKnjige = kupovina.Knjiga?.Naziv ?? "nepoznata";
                decimal cena = kupovina.Knjiga?.Cena ?? 0;

                Console.WriteLine(
                    $"ID KUPOVINE: {kupovina.Id}, " +
                    $"KNJIGA: {nazivKnjige}, " +
                    $"CENA: {cena:F2}, " +
                    $"DATUM: {kupovina.DatumKupovine:dd.MM.yyyy}, " +
                    $"OCENA: {kupovina.Ocena}");
            }

            decimal ukupnaPotrosnja = posetilac.Kupovine.Sum(
                kupovina => kupovina.Knjiga?.Cena ?? 0);

            Console.WriteLine(
                $"PROSECNA OCENA: {posetilac.ProsecnaOcena:F2}");

            Console.WriteLine(
                $"UKUPNA POTROSNJA: {ukupnaPotrosnja:F2}");
        }

        private void PokreniMeniListeZelja()
        {
            Posetilac? posetilac = UnesiPostojecegPosetioca();

            if (posetilac is null)
                return;

            while (true)
            {
                _linkService.LinkAll();

                Console.WriteLine();
                Console.WriteLine(
                    $"=== LISTA ZELJA: {posetilac.Ime} {posetilac.Prezime} ===");

                PrikaziListuZelja(posetilac);

                Console.WriteLine("1 - Dodaj knjigu");
                Console.WriteLine("2 - Ukloni knjigu");
                Console.WriteLine("0 - Nazad");
                Console.Write("Izbor: ");

                string izbor = Console.ReadLine() ?? "0";

                if (izbor == "0")
                    return;

                switch (izbor)
                {
                    case "1":
                        DodajKnjiguNaListuZelja(posetilac);
                        break;

                    case "2":
                        UkloniKnjiguSaListeZelja(posetilac);
                        break;

                    default:
                        Console.WriteLine("Nepoznata opcija.");
                        break;
                }
            }
        }

        private void PrikaziListuZelja(Posetilac posetilac)
        {
            if (posetilac.ListaZelja.Count == 0)
            {
                Console.WriteLine("Lista zelja je prazna.");
                return;
            }

            foreach (Knjiga knjiga in posetilac.ListaZelja)
            {
                Console.WriteLine(
                    $"ID: {knjiga.Id}, " +
                    $"NASLOV: {knjiga.Naziv}, " +
                    $"CENA: {knjiga.Cena:F2}");
            }
        }

        private void DodajKnjiguNaListuZelja(Posetilac posetilac)
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
                _linkService.DodajStavkuListeZelja(
                    posetilac.Id, knjigaId);

                Console.WriteLine("Uspesno dodato.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void UkloniKnjiguSaListeZelja(Posetilac posetilac)
        {
            Console.Write("ID knjige: ");

            int knjigaId = ConsoleViewUtils.SiguranUnosCelogBroja();

            StavkaListeZelja? obrisana =
                _linkService.ObrisiStavkuListeZelja(
                    posetilac.Id, knjigaId);

            Console.WriteLine(
                obrisana is null
                    ? "Ta knjiga nije na listi zelja."
                    : "Uspesno uklonjeno.");
        }

        private void PrikaziSvePosetioce()
        {
            List<Posetilac> posetioci =
                _posetilacDAO.GetAllPosetioci();

            if (posetioci.Count == 0)
            {
                Console.WriteLine("Nema posetilaca.");
                return;
            }

            foreach (Posetilac posetilac in posetioci)
            {
                Adresa? adresa =
                    _adresaDAO.GetAdresaById(posetilac.AdresaId);

                Console.WriteLine(
                    $"ID: {posetilac.Id}, " +
                    $"KARTA: {posetilac.BrojClanskeKarte}, " +
                    $"IME: {posetilac.Ime} {posetilac.Prezime}, " +
                    $"DATUM: {posetilac.DatumRodjenja:dd.MM.yyyy}, " +
                    $"ADRESA: {ConsoleViewUtils.AdresaUTekst(adresa)}, " +
                    $"TELEFON: {posetilac.KontaktTelefon}, " +
                    $"EMAIL: {posetilac.Email}, " +
                    $"GODINA CLANSTVA: {posetilac.TrenutnaGodinaClanstva}, " +
                    $"STATUS: {posetilac.Status}");
            }
        }

        private void DodajPosetioca()
        {
            if (!PostojeAdrese())
                return;

            Posetilac posetilac = UnesiPosetioca();

            try
            {
                _posetilacDAO.AddPosetilac(posetilac);
                Console.WriteLine("Uspesno dodat posetilac.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void IzmeniPosetioca()
        {
            Console.Write("ID posetioca: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            if (_posetilacDAO.GetPosetilacById(id) is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            if (!PostojeAdrese())
                return;

            Posetilac posetilac = UnesiPosetioca();
            posetilac.Id = id;

            try
            {
                Posetilac? izmenjen =
                    _posetilacDAO.UpdatePosetilac(posetilac);

                Console.WriteLine(
                    izmenjen is null
                        ? "Nije pronadjen."
                        : "Uspesno izmenjen.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ObrisiPosetioca()
        {
            Console.Write("ID posetioca: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            if (_posetilacDAO.GetPosetilacById(id) is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            if (PosetilacSeKoristi(id))
                return;

            _posetilacDAO.RemovePosetilac(id);

            Console.WriteLine("Uspesno obrisan posetilac.");
        }

        private bool PostojeAdrese()
        {
            if (_adresaDAO.GetAllAdrese().Count > 0)
                return true;

            Console.WriteLine("Prvo dodaj bar jednu adresu.");
            return false;
        }

        private bool PosetilacSeKoristi(int posetilacId)
        {
            int brojKupovina = _kupovinaDAO.GetAllKupovine()
                .Count(kupovina => kupovina.PosetilacId == posetilacId);

            int brojStavkiListeZelja = _stavkaListeZeljaDAO
                .PronadjiListuZeljaPosetioca(posetilacId).Count;

            if (brojKupovina == 0 && brojStavkiListeZelja == 0)
                return false;

            Console.WriteLine(
                "Posetilac ne moze da se obrise jer je povezan " +
                "sa drugim podacima " +
                $"(kupovine: {brojKupovina}, " +
                $"lista zelja: {brojStavkiListeZelja}).");

            return true;
        }

        private Posetilac UnesiPosetioca()
        {
            Posetilac posetilac = new Posetilac();

            Console.Write("Broj clanske karte: ");
            posetilac.BrojClanskeKarte =
                ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Ime: ");
            posetilac.Ime =
                ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Prezime: ");
            posetilac.Prezime =
                ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Datum rodjenja (dd.MM.yyyy): ");
            posetilac.DatumRodjenja =
                ConsoleViewUtils.SiguranUnosDatuma();

            Console.WriteLine("Dostupne adrese:");

            foreach (Adresa adresa in _adresaDAO.GetAllAdrese())
            {
                Console.WriteLine(
                    $"ID: {adresa.Id}, " +
                    $"{adresa.Ulica} {adresa.Broj}, " +
                    $"{adresa.Grad}, {adresa.Drzava}");
            }

            Console.Write("ID adrese: ");

            posetilac.AdresaId =
                ConsoleViewUtils.SiguranUnosPostojecegId(
                    id => _adresaDAO.GetAdresaById(id) is not null);

            posetilac.Adresa =
                _adresaDAO.GetAdresaById(posetilac.AdresaId);

            Console.Write("Kontakt telefon: ");
            posetilac.KontaktTelefon =
                ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Email: ");
            posetilac.Email =
                ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Trenutna godina clanstva: ");
            posetilac.TrenutnaGodinaClanstva =
                ConsoleViewUtils.SiguranUnosCelogBroja();

            Console.Write("Status (R - Regular, V - VIP): ");
            posetilac.Status =
                ConsoleViewUtils.SiguranUnosStatusa();

            return posetilac;
        }
    }
}