
using System;
using System.Collections.Generic;
using Core.DAO;
using Core.Models;

namespace ConsoleClient.Views
{
    public class KupovinaConsoleView
    {
        private readonly KupovinaDAO _kupovinaDAO;
        private readonly PosetilacDAO _posetilacDAO;
        private readonly KnjigaDAO _knjigaDAO;

        public KupovinaConsoleView(
            KupovinaDAO kupovinaDAO,
            PosetilacDAO posetilacDAO,
            KnjigaDAO knjigaDAO)
        {
            _kupovinaDAO = kupovinaDAO;
            _posetilacDAO = posetilacDAO;
            _knjigaDAO = knjigaDAO;
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
                string posetilac = kupovina.Posetilac is null
                    ? $"ID {kupovina.PosetilacId}"
                    : $"{kupovina.Posetilac.Ime} {kupovina.Posetilac.Prezime}";

                string knjiga = kupovina.Knjiga is null
                    ? $"ID {kupovina.KnjigaId}"
                    : kupovina.Knjiga.Naziv;

                Console.WriteLine(
                    $"ID: {kupovina.Id}, " +
                    $"POSETILAC: {posetilac}, " +
                    $"KNJIGA: {knjiga}, " +
                    $"DATUM: {kupovina.DatumKupovine:dd.MM.yyyy}, " +
                    $"OCENA: {kupovina.Ocena}, " +
                    $"KOMENTAR: {kupovina.Komentar}");
            }
        }

        private void DodajKupovinu()
        {
            Kupovina? kupovina = UnesiKupovinu();

            if (kupovina is null)
            {
                return;
            }

            _kupovinaDAO.AddKupovina(kupovina);

            Console.WriteLine("Uspesno dodata kupovina.");
        }

        private void IzmeniKupovinu()
        {
            Console.Write("ID kupovine: ");

            int id = ConsoleViewUtils.SiguranUnosPostojecegId(
                unos => _kupovinaDAO.GetKupovinaById(unos) is not null);

            Kupovina? kupovina = UnesiKupovinu();

            if (kupovina is null)
            {
                return;
            }

            kupovina.Id = id;

            Kupovina? izmenjena =
                _kupovinaDAO.UpdateKupovina(kupovina);

            Console.WriteLine(
                izmenjena is null
                    ? "Nije pronadjena."
                    : "Uspesno izmenjena.");
        }

        private void ObrisiKupovinu()
        {
            Console.Write("ID kupovine: ");

            int id = ConsoleViewUtils.SiguranUnosPostojecegId(
                unos => _kupovinaDAO.GetKupovinaById(unos) is not null);

            _kupovinaDAO.RemoveKupovina(id);

            Console.WriteLine("Uspesno obrisana kupovina.");
        }

        private Kupovina? UnesiKupovinu()
        {
            List<Posetilac> posetioci =
                _posetilacDAO.GetAllPosetioci();

            List<Knjiga> knjige =
                _knjigaDAO.GetAllKnjige();

            if (posetioci.Count == 0)
            {
                Console.WriteLine("Nema dostupnih posetilaca.");
                return null;
            }

            if (knjige.Count == 0)
            {
                Console.WriteLine("Nema dostupnih knjiga.");
                return null;
            }

            Console.WriteLine("Dostupni posetioci:");

            foreach (Posetilac posetilac in posetioci)
            {
                Console.WriteLine(
                    $"ID: {posetilac.Id}, " +
                    $"{posetilac.Ime} {posetilac.Prezime}");
            }

            Console.Write("ID posetioca: ");

            int posetilacId =
                ConsoleViewUtils.SiguranUnosPostojecegId(
                    unos => _posetilacDAO.GetPosetilacById(unos) is not null);

            Posetilac posetilacIzbor =
                _posetilacDAO.GetPosetilacById(posetilacId)!;

            Console.WriteLine("Dostupne knjige:");

            foreach (Knjiga knjiga in knjige)
            {
                Console.WriteLine(
                    $"ID: {knjiga.Id}, " +
                    $"NAZIV: {knjiga.Naziv}, " +
                    $"ISBN: {knjiga.ISBN}");
            }

            Console.Write("ID knjige: ");

            int knjigaId =
                ConsoleViewUtils.SiguranUnosPostojecegId(
                    unos => _knjigaDAO.GetKnjigaById(unos) is not null);

            Knjiga knjigaIzbor =
                _knjigaDAO.GetKnjigaById(knjigaId)!;

            Console.Write("Datum kupovine (dd.MM.yyyy): ");

            DateTime datumKupovine =
                ConsoleViewUtils.SiguranUnosDatuma();

            Console.Write("Ocena (1-5): ");

            int ocena =
                ConsoleViewUtils.SiguranUnosOcene();

            Console.Write("Komentar: ");

            string komentar =
                ConsoleViewUtils.SiguranUnosTeksta(false);

            return new Kupovina(
                posetilacIzbor,
                knjigaIzbor,
                datumKupovine,
                ocena,
                komentar);
        }
    }
}

