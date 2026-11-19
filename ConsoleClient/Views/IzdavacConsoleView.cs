
using System;
using System.Collections.Generic;
using Core.DAO;
using Core.Models;

namespace ConsoleClient.Views
{
    public class IzdavacConsoleView
    {
        private readonly IzdavacDAO _izdavacDAO;
        private readonly AutorDAO _autorDAO;

        public IzdavacConsoleView(
            IzdavacDAO izdavacDAO,
            AutorDAO autorDAO)
        {
            _izdavacDAO = izdavacDAO;
            _autorDAO = autorDAO;
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
            Console.WriteLine("=== IZDAVACI ===");
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
                case "1": PrikaziSveIzdavace(); break;
                case "2": DodajIzdavaca(); break;
                case "3": IzmeniIzdavaca(); break;
                case "4": ObrisiIzdavaca(); break;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }

        private void PrikaziSveIzdavace()
        {
            List<Izdavac> izdavaci =
                _izdavacDAO.GetAllIzdavaci();

            if (izdavaci.Count == 0)
            {
                Console.WriteLine("Nema izdavaca.");
                return;
            }

            foreach (Izdavac izdavac in izdavaci)
            {
                string sef = izdavac.SefIzdavaca is null
                    ? $"ID {izdavac.SefIzdavacaId}"
                    : $"{izdavac.SefIzdavaca.Ime} {izdavac.SefIzdavaca.Prezime}";

                Console.WriteLine(
                    $"ID: {izdavac.Id}, " +
                    $"SIFRA: {izdavac.Sifra}, " +
                    $"NAZIV: {izdavac.Naziv}, " +
                    $"SEF IZDAVACA: {sef}");
            }
        }

        private void DodajIzdavaca()
        {
            Izdavac izdavac = UnesiIzdavaca();

            _izdavacDAO.AddIzdavac(izdavac);

            Console.WriteLine("Uspesno dodat izdavac.");
        }

        private void IzmeniIzdavaca()
        {
            Console.Write("ID izdavaca: ");

            int id = ConsoleViewUtils.SiguranUnosPostojecegId(
                unos => _izdavacDAO.GetIzdavacById(unos) is not null);

            Izdavac izdavac = UnesiIzdavaca();
            izdavac.Id = id;

            Izdavac? izmenjen =
                _izdavacDAO.UpdateIzdavac(izdavac);

            Console.WriteLine(
                izmenjen is null
                    ? "Nije pronadjen."
                    : "Uspesno izmenjen.");
        }

        private void ObrisiIzdavaca()
        {
            Console.Write("ID izdavaca: ");

            int id = ConsoleViewUtils.SiguranUnosPostojecegId(
                unos => _izdavacDAO.GetIzdavacById(unos) is not null);

            _izdavacDAO.RemoveIzdavac(id);

            Console.WriteLine("Uspesno obrisan izdavac.");
        }

        private Izdavac UnesiIzdavaca()
        {
            Console.Write("Sifra: ");
            string sifra = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Naziv: ");
            string naziv = ConsoleViewUtils.SiguranUnosTeksta();

            Console.WriteLine("Dostupni autori:");

            foreach (Autor autor in _autorDAO.GetAllAutori())
            {
                Console.WriteLine(
                    $"ID: {autor.Id}, {autor.Ime} {autor.Prezime}");
            }

            Console.Write("ID autora koji je sef izdavaca: ");

            int autorId = ConsoleViewUtils.SiguranUnosPostojecegId(
                unos => _autorDAO.GetAutorById(unos) is not null);

            Autor sefIzdavaca = _autorDAO.GetAutorById(autorId)!;

            return new Izdavac(sifra, naziv, sefIzdavaca);
        }
    }
}
