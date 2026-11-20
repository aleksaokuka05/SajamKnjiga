
using System;
using System.Collections.Generic;
using Core.DAO;
using Core.Models;

namespace ConsoleClient.Views
{
    public class AdresaConsoleView
    {
        private readonly AdresaDAO _adresaDAO;
        private readonly PosetilacDAO _posetilacDAO;
        private readonly AutorDAO _autorDAO;

        public AdresaConsoleView(
            AdresaDAO adresaDAO,
            PosetilacDAO posetilacDAO,
            AutorDAO autorDAO)
        {
            _adresaDAO = adresaDAO;
            _posetilacDAO = posetilacDAO;
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
            Console.WriteLine("=== ADRESE ===");
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
                case "1": PrikaziSveAdrese(); break;
                case "2": DodajAdresu(); break;
                case "3": IzmeniAdresu(); break;
                case "4": ObrisiAdresu(); break;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }

        private void PrikaziSveAdrese()
        {
            List<Adresa> adrese = _adresaDAO.GetAllAdrese();

            if (adrese.Count == 0)
            {
                Console.WriteLine("Nema adresa.");
                return;
            }

            foreach (Adresa adresa in adrese)
            {
                Console.WriteLine(
                    $"ID: {adresa.Id}, ULICA: {adresa.Ulica}, BROJ: {adresa.Broj}, GRAD: {adresa.Grad}, DRZAVA: {adresa.Drzava}");
            }
        }

        private void DodajAdresu()
        {
            Adresa adresa = UnesiAdresu();
            _adresaDAO.AddAdresa(adresa);
            Console.WriteLine("Uspesno dodato.");
        }

        private void IzmeniAdresu()
        {
            Console.Write("ID adrese: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            if (_adresaDAO.GetAdresaById(id) is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            Adresa adresa = UnesiAdresu();
            adresa.Id = id;

            Adresa? izmenjena = _adresaDAO.UpdateAdresa(adresa);

            Console.WriteLine(
                izmenjena is null ? "Nije pronadjeno." : "Uspesno izmenjeno.");
        }

        private void ObrisiAdresu()
        {
            Console.Write("ID adrese: ");
            int id = ConsoleViewUtils.SiguranUnosCelogBroja();

            if (_adresaDAO.GetAdresaById(id) is null)
            {
                Console.WriteLine("Nije pronadjeno.");
                return;
            }

            if (DaLiSeAdresaKoristi(id))
            {
                return;
            }

            _adresaDAO.RemoveAdresa(id);
            Console.WriteLine("Uspesno obrisano.");
        }

        // Adresa se ne brise dok je koristi neki posetilac ili autor,
        // da oni ne bi ostali sa adresom koja ne postoji
        private bool DaLiSeAdresaKoristi(int adresaId)
        {
            int brojPosetilaca = _posetilacDAO.GetAllPosetioci().FindAll(p => p.AdresaId == adresaId).Count;

            int brojAutora = _autorDAO.GetAllAutori().FindAll(a => a.AdresaId == adresaId).Count;

            if (brojPosetilaca == 0 && brojAutora == 0)
            {
                return false;
            }

            Console.WriteLine(
                $"Adresa ne moze da se obrise jer je koriste drugi podaci (posetioci: {brojPosetilaca}, autori: {brojAutora}).");

            return true;
        }

        private Adresa UnesiAdresu()
        {
            Adresa adresa = new Adresa();

            Console.Write("Ulica: ");
            adresa.Ulica = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Broj: ");
            adresa.Broj = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Grad: ");
            adresa.Grad = ConsoleViewUtils.SiguranUnosTeksta();

            Console.Write("Drzava: ");
            adresa.Drzava = ConsoleViewUtils.SiguranUnosTeksta();

            return adresa;
        }
    }
}
