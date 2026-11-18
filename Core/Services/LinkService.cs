using Core.DAO;
using Core.Models;

namespace Core.Services
{
    public class LinkService
    {
        private readonly AdresaDAO _adresaDAO;
        private readonly AutorDAO _autorDAO;
        private readonly KnjigaDAO _knjigaDAO;
        private readonly PosetilacDAO _posetilacDAO;
        private readonly KupovinaDAO _kupovinaDAO;
        private readonly IzdavacDAO _izdavacDAO;
        private readonly KnjigaAutorDAO _knjigaAutorDAO;
        private readonly StavkaListeZeljaDAO _stavkaListeZeljaDAO;

        public LinkService(
            AdresaDAO adresaDAO,
            AutorDAO autorDAO,
            KnjigaDAO knjigaDAO,
            PosetilacDAO posetilacDAO,
            KupovinaDAO kupovinaDAO,
            IzdavacDAO izdavacDAO,
            KnjigaAutorDAO knjigaAutorDAO,
            StavkaListeZeljaDAO stavkaListeZeljaDAO)
        {
            _adresaDAO = adresaDAO;
            _autorDAO = autorDAO;
            _knjigaDAO = knjigaDAO;
            _posetilacDAO = posetilacDAO;
            _kupovinaDAO = kupovinaDAO;
            _izdavacDAO = izdavacDAO;
            _knjigaAutorDAO = knjigaAutorDAO;
            _stavkaListeZeljaDAO = stavkaListeZeljaDAO;
        }

        public void LinkAll()
        {
            OcistiVeze();
            PoveziAdrese();
            PoveziIzdavace();
            PoveziKnjigeAutore();
            PoveziKupovine();
            PoveziListeZelja();
            PoveziAutoreIzdavaca();
        }

        public void DodajKnjigaAutor(int knjigaId, int autorId)
        {
            _knjigaAutorDAO.DodajKnjigaAutor(
                new KnjigaAutor(knjigaId, autorId));

            LinkAll();
        }

        public KnjigaAutor? ObrisiKnjigaAutor(int knjigaId, int autorId)
        {
            KnjigaAutor? obrisana =
                _knjigaAutorDAO.ObrisiKnjigaAutor(knjigaId, autorId);

            LinkAll();
            return obrisana;
        }

        public void DodajStavkuListeZelja(int posetilacId, int knjigaId)
        {
            _stavkaListeZeljaDAO.DodajStavkuListeZelja(
                new StavkaListeZelja(posetilacId, knjigaId));

            LinkAll();
        }

        public StavkaListeZelja? ObrisiStavkuListeZelja(
            int posetilacId, int knjigaId)
        {
            StavkaListeZelja? obrisana =
                _stavkaListeZeljaDAO.ObrisiStavkuListeZelja(
                    posetilacId, knjigaId);

            LinkAll();
            return obrisana;
        }

        private void OcistiVeze()
        {
            foreach (Autor autor in _autorDAO.GetAllAutori())
            {
                autor.Adresa = null;
                autor.Knjige.Clear();
            }

            foreach (Knjiga knjiga in _knjigaDAO.GetAllKnjige())
            {
                knjiga.Izdavac = null;
                knjiga.Autori.Clear();
                knjiga.Kupci.Clear();
                knjiga.ListaZeljaPosetilaca.Clear();
            }

            foreach (Posetilac posetilac in _posetilacDAO.GetAllPosetioci())
            {
                posetilac.Adresa = null;
                posetilac.Kupovine.Clear();
                posetilac.ListaZelja.Clear();
            }

            foreach (Izdavac izdavac in _izdavacDAO.GetAllIzdavaci())
            {
                izdavac.SefIzdavaca = null;
                izdavac.Knjige.Clear();
                izdavac.Autori.Clear();
            }

            foreach (Kupovina kupovina in _kupovinaDAO.GetAllKupovine())
            {
                kupovina.Posetilac = null;
                kupovina.Knjiga = null;
            }
        }

        private void PoveziAdrese()
        {
            foreach (Autor autor in _autorDAO.GetAllAutori())
            {
                autor.Adresa = _adresaDAO.GetAdresaById(autor.AdresaId);
            }

            foreach (Posetilac posetilac in _posetilacDAO.GetAllPosetioci())
            {
                posetilac.Adresa = _adresaDAO.GetAdresaById(posetilac.AdresaId);
            }
        }

        private void PoveziIzdavace()
        {
            foreach (Izdavac izdavac in _izdavacDAO.GetAllIzdavaci())
            {
                izdavac.SefIzdavaca =
                    _autorDAO.GetAutorById(izdavac.SefIzdavacaId);
            }

            foreach (Knjiga knjiga in _knjigaDAO.GetAllKnjige())
            {
                Izdavac? izdavac =
                    _izdavacDAO.GetIzdavacById(knjiga.IzdavacId);

                knjiga.Izdavac = izdavac;
                izdavac?.Knjige.Add(knjiga);
            }
        }

        private void PoveziKnjigeAutore()
        {
            foreach (KnjigaAutor veza in _knjigaAutorDAO.PrikaziSveVeze())
            {
                Knjiga? knjiga =
                    _knjigaDAO.GetKnjigaById(veza.KnjigaId);

                Autor? autor =
                    _autorDAO.GetAutorById(veza.AutorId);

                if (knjiga is null || autor is null)
                {
                    continue;
                }

                knjiga.Autori.Add(autor);
                autor.Knjige.Add(knjiga);
            }
        }

        private void PoveziKupovine()
        {
            foreach (Kupovina kupovina in _kupovinaDAO.GetAllKupovine())
            {
                Posetilac? posetilac =
                    _posetilacDAO.GetPosetilacById(kupovina.PosetilacId);

                Knjiga? knjiga =
                    _knjigaDAO.GetKnjigaById(kupovina.KnjigaId);

                kupovina.Posetilac = posetilac;
                kupovina.Knjiga = knjiga;

                posetilac?.Kupovine.Add(kupovina);

                if (posetilac is not null &&
                    knjiga is not null &&
                    !knjiga.Kupci.Contains(posetilac))
                {
                    knjiga.Kupci.Add(posetilac);
                }
            }
        }

        private void PoveziListeZelja()
        {
            foreach (StavkaListeZelja stavka in
                _stavkaListeZeljaDAO.PrikaziSveStavkeListeZelja())
            {
                Posetilac? posetilac =
                    _posetilacDAO.GetPosetilacById(stavka.PosetilacId);

                Knjiga? knjiga =
                    _knjigaDAO.GetKnjigaById(stavka.KnjigaId);

                if (posetilac is null || knjiga is null)
                {
                    continue;
                }

                posetilac.ListaZelja.Add(knjiga);
                knjiga.ListaZeljaPosetilaca.Add(posetilac);
            }
        }

        private void PoveziAutoreIzdavaca()
        {
            foreach (Izdavac izdavac in _izdavacDAO.GetAllIzdavaci())
            {
                foreach (Knjiga knjiga in izdavac.Knjige)
                {
                    foreach (Autor autor in knjiga.Autori)
                    {
                        if (!izdavac.Autori.Contains(autor))
                        {
                            izdavac.Autori.Add(autor);
                        }
                    }
                }
            }
        }
    }
}
