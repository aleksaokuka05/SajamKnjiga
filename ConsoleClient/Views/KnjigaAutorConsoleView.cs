
using System;
using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Services;

namespace ConsoleClient.Views
{
    public class KnjigaAutorConsoleView
    {
        private readonly KnjigaAutorDAO _knjigaAutorDAO;
        private readonly KnjigaDAO _knjigaDAO;
        private readonly AutorDAO _autorDAO;
        private readonly LinkService _linkService;

        public KnjigaAutorConsoleView(
            KnjigaAutorDAO knjigaAutorDAO,
            KnjigaDAO knjigaDAO,
            AutorDAO autorDAO,
            LinkService linkService)
        {
            _knjigaAutorDAO = knjigaAutorDAO;
            _knjigaDAO = knjigaDAO;
            _autorDAO = autorDAO;
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
            Console.WriteLine("=== KNJIGA - AUTOR ===");
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
                case "1": PrikaziSveVeze(); break;
                case "2": DodajVezu(); break;
                case "3": ObrisiVezu(); break;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }

        private void PrikaziSveVeze()
        {
            List<KnjigaAutor> veze = _knjigaAutorDAO.PrikaziSveVeze();

            if (veze.Count == 0)
            {
                Console.WriteLine("Nema veza.");
                return;
            }

            foreach (KnjigaAutor veza in veze)
            {
                string nazivKnjige = _knjigaDAO
                    .GetKnjigaById(veza.KnjigaId)?.Naziv ?? "nepoznata";

                Autor? autor = _autorDAO.GetAutorById(veza.AutorId);

                string imeAutora = autor is null
                    ? "nepoznat"
                    : $"{autor.Ime} {autor.Prezime}";

                Console.WriteLine(
                    $"KNJIGA: {nazivKnjige} (ID {veza.KnjigaId}), AUTOR: {imeAutora} (ID {veza.AutorId})");
            }
        }

        private void DodajVezu()
        {
            if (_knjigaDAO.GetAllKnjige().Count == 0)
            {
                Console.WriteLine("Prvo dodaj bar jednu knjigu.");
                return;
            }

            if (_autorDAO.GetAllAutori().Count == 0)
            {
                Console.WriteLine("Prvo dodaj bar jednog autora.");
                return;
            }

            Console.Write("ID knjige: ");
            int knjigaId = ConsoleViewUtils.SiguranUnosPostojecegId(
                id => _knjigaDAO.GetKnjigaById(id) is not null);

            Console.Write("ID autora: ");
            int autorId = ConsoleViewUtils.SiguranUnosPostojecegId(
                id => _autorDAO.GetAutorById(id) is not null);

            try
            {
                _linkService.DodajKnjigaAutor(knjigaId, autorId);
                Console.WriteLine("Uspesno dodato.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ObrisiVezu()
        {
            Console.Write("ID knjige: ");
            int knjigaId = ConsoleViewUtils.SiguranUnosCelogBroja();

            Console.Write("ID autora: ");
            int autorId = ConsoleViewUtils.SiguranUnosCelogBroja();

            KnjigaAutor? obrisana =
                _linkService.ObrisiKnjigaAutor(knjigaId, autorId);

            Console.WriteLine(
                obrisana is null ? "Nije pronadjeno." : "Uspesno obrisano.");
        }
    }
}
