using System;
using Core.Interfaces;

namespace Core.Models
{
    // Veza knjiga - autor je many to many.
    // Serijalizujemo je pomocu ove klase, po uzoru na StudentSubject sa vezbi.
    public class KnjigaAutor : ISerializable
    {
        public int KnjigaId { get; set; }
        public int AutorId { get; set; }

        public KnjigaAutor()
        {
        }

        public KnjigaAutor(int knjigaId, int autorId)
        {
            KnjigaId = knjigaId;
            AutorId = autorId;
        }

        public string[] ToCSV()
        {
            string[] niz_polja =
            {
                KnjigaId.ToString(),
                AutorId.ToString()
            };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            KnjigaId = int.Parse(values[0]);
            AutorId = int.Parse(values[1]);
        }

    
    }
}
