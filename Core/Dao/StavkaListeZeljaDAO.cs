using System;
using System.Collections.Generic;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
   
    public class StavkaListeZeljaDAO
    {
        private readonly List<StavkaListeZelja> _stavkeListeZelja;
        private readonly Storage<StavkaListeZelja> _storage;

        public StavkaListeZeljaDAO()
        {
            _storage = new Storage<StavkaListeZelja>("lista_zelja.csv");
            _stavkeListeZelja = _storage.Load();
        }

        public StavkaListeZelja DodajStavkuListeZelja(StavkaListeZelja stavkaListeZelja)
        {
            if (PronadjiStavkuListeZelja(stavkaListeZelja.PosetilacId, stavkaListeZelja.KnjigaId) is not null)
            {
                throw new InvalidOperationException("Ova knjiga je vec na listi zelja posetioca.");
            }

            _stavkeListeZelja.Add(stavkaListeZelja);
            _storage.Save(_stavkeListeZelja);

            return stavkaListeZelja;
        }

        public StavkaListeZelja? ObrisiStavkuListeZelja(int posetilacId, int knjigaId)
        {
            StavkaListeZelja? stavkaListeZelja = PronadjiStavkuListeZelja(posetilacId, knjigaId);

            if (stavkaListeZelja is null) return null;

            _stavkeListeZelja.Remove(stavkaListeZelja);
            _storage.Save(_stavkeListeZelja);

            return stavkaListeZelja;
        }

        public StavkaListeZelja? PronadjiStavkuListeZelja(int posetilacId, int knjigaId)
        {
            foreach (StavkaListeZelja stavkaListeZelja in _stavkeListeZelja)
            {
                if (stavkaListeZelja.PosetilacId == posetilacId &&
                    stavkaListeZelja.KnjigaId == knjigaId)
                {
                    return stavkaListeZelja;
                }
            }

            return null;
        }

        public List<StavkaListeZelja> PrikaziSveStavkeListeZelja()
        {
            return _stavkeListeZelja;
        }


        public List<StavkaListeZelja> PronadjiListuZeljaPosetioca(int posetilacId)
        {
            return _stavkeListeZelja.FindAll(stavka => stavka.PosetilacId == posetilacId);
        }

       
        public List<StavkaListeZelja> PronadjiPosetioceZaKnjigu(int knjigaId)
        {
            return _stavkeListeZelja.FindAll(stavka => stavka.KnjigaId == knjigaId);
        }
    }
}
