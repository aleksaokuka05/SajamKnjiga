using System;
using System.Collections.Generic;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{

    // Izvor: OISISI, Vezbe 3, CRUDExample (DAO pattern)
    public class KnjigaAutorDAO
    {
        private readonly List<KnjigaAutor> _knjigeAutori;
        private readonly Storage<KnjigaAutor> _storage;

        public KnjigaAutorDAO()
        {
            _storage = new Storage<KnjigaAutor>("knjige_autori.csv");
            _knjigeAutori = _storage.Load();
        }

        public KnjigaAutor DodajKnjigaAutor(KnjigaAutor knjigaAutor)
        {
            if (PronadjiKnjigaAutor(knjigaAutor.KnjigaId, knjigaAutor.AutorId) is not null)
            {
                throw new InvalidOperationException("Ova veza knjiga - autor vec postoji.");
            }

            _knjigeAutori.Add(knjigaAutor);
            _storage.Save(_knjigeAutori);

            return knjigaAutor;
        }

        public KnjigaAutor? ObrisiKnjigaAutor(int knjigaId, int autorId)
        {
            KnjigaAutor? knjigaAutor = PronadjiKnjigaAutor(knjigaId, autorId);

            if (knjigaAutor is null) return null;

            _knjigeAutori.Remove(knjigaAutor);
            _storage.Save(_knjigeAutori);

            return knjigaAutor;
        }

        public KnjigaAutor? PronadjiKnjigaAutor(int knjigaId, int autorId)
        {
            foreach (KnjigaAutor knjigaAutor in _knjigeAutori)
            {
                if (knjigaAutor.KnjigaId == knjigaId &&
                    knjigaAutor.AutorId == autorId)
                {
                    return knjigaAutor;
                }
            }

            return null;
        }

        public List<KnjigaAutor> PrikaziSveVeze()
        {
            return _knjigeAutori;
        }

        // Veze jedne knjige (njeni autori)
        public List<KnjigaAutor> PronadjiAutoreKnjige(int knjigaId)
        {
            return _knjigeAutori.FindAll(veza => veza.KnjigaId == knjigaId);
        }

        // Veze jednog autora (njegove knjige)
        public List<KnjigaAutor> PronadjiKnjigeAutora(int autorId)
        {
            return _knjigeAutori.FindAll(veza => veza.AutorId == autorId);
        }
    }
}
