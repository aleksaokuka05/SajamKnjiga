
using System;
using Core.Interfaces;

namespace Core.Models
{
    // Veza posetilac - knjiga (lista zelja) je many to many.
    // Serijalizujemo je pomocu ove klase, po uzoru na StudentSubject sa vezbi.
    public class StavkaListeZelja : ISerializable
    {
        public int PosetilacId { get; set; }
        public int KnjigaId { get; set; }

        public StavkaListeZelja()
        {
        }

        public StavkaListeZelja(int posetilacId, int knjigaId)
        {
            PosetilacId = posetilacId;
            KnjigaId = knjigaId;
        }

        public string[] ToCSV()
        {
            string[] niz_polja =
            {
                PosetilacId.ToString(),
                KnjigaId.ToString()
            };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            PosetilacId = int.Parse(values[0]);
            KnjigaId = int.Parse(values[1]);
        }

     
    }
}
