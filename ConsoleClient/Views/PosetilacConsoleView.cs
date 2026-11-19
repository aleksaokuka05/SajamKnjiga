
using System;
using System.Collections.Generic;
using Core.DAO;
using Core.Models;

namespace ConsoleClient.Views
{
    public class PosetilacConsoleView
    {
        private readonly PosetilacDAO _posetilacDAO;
        private readonly AdresaDAO _adresaDAO;

        public PosetilacConsoleView(
            PosetilacDAO posetilacDAO,
            AdresaDAO adresaDAO)
        {
            _posetilacDAO = posetilacDAO;
            _adresaDAO = adresaDAO;
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
            Console.WriteLine("=== POSETIOCI ===");
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
                case "1": PrikaziSvePosetioce(); break;
                case "2": DodajPosetioca(); break;
                case "3": IzmeniPosetioca(); break;
                case "4": ObrisiPosetioca(); break;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
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
                Console.WriteLine(
                    $"ID: {posetilac.Id}, IME: {posetilac.Ime}, " +
                    $"PREZIME: {posetilac.Prezime}, " +
                    $"ADRESA: {ConsoleViewUtils.AdresaUTekst(posetilac.Adresa)}, " +
                    $"TELEFON: {posetilac.KontaktTelefon}, " +
                    $"EMAIL: {posetilac.Email}, " +
                    $"CLANSKA KARTA: {posetilac.BrojClanskeKarte}, " +
                    $"GODINA CLANSTVA: {posetilac.TrenutnaGodinaClanstva}, " +
                    $"STATUS: {posetilac.Status}");
            }
        }

        private void DodajPosetioca()
        {
            Posetilac posetilac = UnesiPosetioca();

            _posetilacDAO.AddPosetilac(posetilac);

            Console.WriteLine("Uspesno dodat posetilac.");
        }

        private void IzmeniPosetioca()
        {
            Console.Write("ID posetioca: ");

            int id = ConsoleViewUtils.SiguranUnosPostojecegId(
                unos => _posetilacDAO.GetPosetilacById(unos) is not null);

            Posetilac posetilac = UnesiPosetioca();
            posetilac.Id = id;

            Posetilac? izmenjen =
                _posetilacDAO.UpdatePosetilac(posetilac);

            Console.WriteLine(
                izmenjen is null
                    ? "Nije pronadjen."
                    : "Uspesno izmenjen.");
        }

        private void ObrisiPosetioca()
        {
            Console.Write("ID posetioca: ");

            int id = ConsoleViewUtils.SiguranUnosPostojecegId(
                unos => _posetilacDAO.GetPosetilacById(unos) is not null);

            _posetilacDAO.RemovePosetilac(id);

            Console.WriteLine("Uspesno obrisan posetilac.");
        }

        private Posetilac UnesiPosetioca()
        {
            Console.Write("Ime: ");
            string ime = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Prezime: ");
            string prezime = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Datum rodjenja (dd.MM.yyyy): ");
            DateTime datumRodjenja =
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

            int adresaId = ConsoleViewUtils.SiguranUnosPostojecegId(
                unos => _adresaDAO.GetAdresaById(unos) is not null);

            Adresa adresaIzbor = _adresaDAO.GetAdresaById(adresaId)!;

            Console.Write("Kontakt telefon: ");
            string kontaktTelefon =
                ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Email: ");
            string email = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Broj clanske karte: ");
            string brojClanskeKarte =
                ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Trenutna godina clanstva: ");
            int trenutnaGodinaClanstva =
                ConsoleViewUtils.SiguranUnosCelogBroja();

            Console.Write("Status (R - Regular, V - VIP): ");
            StatusPosetioca status =
                ConsoleViewUtils.SiguranUnosStatusa();

            return new Posetilac(
                ime,
                prezime,
                datumRodjenja,
                adresaIzbor,
                kontaktTelefon,
                email,
                brojClanskeKarte,
                trenutnaGodinaClanstva,
                status);
        }
    }
}
