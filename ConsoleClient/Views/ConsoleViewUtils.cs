
using System;
using System.Globalization;
using Core.Models;

namespace ConsoleClient.Views
{
    public static class ConsoleViewUtils
    {
        private const char ZabranjeniZnak = '|';

        public static string SiguranUnosTeksta(bool obavezno = true)
        {
            while (true)
            {
                string? linija = Console.ReadLine();

                if (linija is null)
                {
                    return string.Empty;
                }

                string unos = linija.Trim();

                if (unos.Contains(ZabranjeniZnak))
                {
                    Console.Write($"Tekst ne sme da sadrzi znak '{ZabranjeniZnak}'. Unesi ponovo: ");
                    continue;
                }

                if (obavezno && unos.Length == 0)
                {
                    Console.Write("Polje je obavezno. Unesi ponovo: ");
                    continue;
                }

                return unos;
            }
        }

        public static int SiguranUnosCelogBroja()
        {
            while (true)
            {
                string unos = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(unos, out int broj))
                {
                    return broj;
                }

                Console.Write("Unesi ceo broj: ");
            }
        }

        public static int SiguranUnosPostojecegId(Func<int, bool> postoji)
        {
            while (true)
            {
                int id = SiguranUnosCelogBroja();

                if (postoji(id))
                {
                    return id;
                }

                Console.Write("Ne postoji zapis sa tim ID. Unesi ponovo: ");
            }
        }

        public static string AdresaUTekst(Adresa? adresa)
        {
            if (adresa is null)
            {
                return "nepoznata";
            }

            return $"{adresa.Ulica} {adresa.Broj}, {adresa.Grad}";
        }

        public static int? SiguranUnosOpcionogCelogBroja()
        {
            while (true)
            {
                string unos = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(unos))
                {
                    return null;
                }

                if (int.TryParse(unos, out int broj))
                {
                    return broj;
                }

                Console.Write("Unesi ceo broj ili ostavi prazno: ");
            }
        }

        public static decimal SiguranUnosDecimalnogBroja()
        {
            while (true)
            {
                string unos = (Console.ReadLine() ?? string.Empty).Replace(',', '.');

                if (decimal.TryParse(
                    unos,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out decimal broj))
                {
                    return broj;
                }

                Console.Write("Unesi decimalni broj: ");
            }
        }

        public static DateTime SiguranUnosDatuma()
        {
            while (true)
            {
                string unos = Console.ReadLine() ?? string.Empty;

                if (DateTime.TryParseExact(
                    unos,
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime datum))
                {
                    return datum;
                }

                Console.Write("Unesi datum u obliku dd.MM.yyyy: ");
            }
        }

        public static int SiguranUnosOcene()
        {
            while (true)
            {
                int ocena = SiguranUnosCelogBroja();

                if (ocena >= 1 && ocena <= 5)
                {
                    return ocena;
                }

                Console.Write("Ocena mora biti od 1 do 5: ");
            }
        }

        public static StatusPosetioca SiguranUnosStatusa()
        {
            while (true)
            {
                string unos = (Console.ReadLine() ?? string.Empty).Trim();

                if (unos.Equals("R", StringComparison.OrdinalIgnoreCase) ||
                    unos.Equals("Regular", StringComparison.OrdinalIgnoreCase))
                {
                    return StatusPosetioca.Regular;
                }

                if (unos.Equals("V", StringComparison.OrdinalIgnoreCase) ||
                    unos.Equals("VIP", StringComparison.OrdinalIgnoreCase))
                {
                    return StatusPosetioca.VIP;
                }

                Console.Write("Unesi R ili V: ");
            }
        }
    }
}
