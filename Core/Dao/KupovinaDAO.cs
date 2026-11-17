using System;
using System.Collections.Generic;
using System.IO;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    public class KupovinaDAO
    {
        // Putanja do CSV fajla
        private readonly string _filePath = "Data" + Path.DirectorySeparatorChar + "kupovine.csv";
        private readonly Serializer<Kupovina> _serializer = new Serializer<Kupovina>();

        // Ucitavanje svih kupovina
        public List<Kupovina> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Kupovina>();
            }

            string[] lines = File.ReadAllLines(_filePath);

            return _serializer.FromCSV(lines);
        }

        // Cuvanje kupovina u CSV
        public void Save(List<Kupovina> kupovine)
        {
            string content = _serializer.ToCSV(kupovine);

            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            File.WriteAllText(_filePath, content);
        }

        //Dodavanje nove kupovine
        public void Add(Kupovina kupovina)
        {
            List<Kupovina> kupovine = GetAll();

            kupovine.Add(kupovina);

            Save(kupovine);
        }

        // Izmena
        public void Update(Kupovina kupovina)
        {
            List<Kupovina> kupovine = GetAll();

            int index = kupovine.FindIndex(k =>
                k.Posetilac.BrojClanskeKarte == kupovina.Posetilac.BrojClanskeKarte &&
                k.Knjiga.ISBN == kupovina.Knjiga.ISBN &&
                k.DatumKupovine == kupovina.DatumKupovine);

            if (index != -1)
            {
                kupovine[index] = kupovina;
                Save(kupovine);
            }
        }

        // Brisanje kupovine
        public void Delete(Kupovina kupovina)
        {
            List<Kupovina> kupovine = GetAll();

            Kupovina postojeca = kupovine.Find(k =>
                k.Posetilac.BrojClanskeKarte == kupovina.Posetilac.BrojClanskeKarte &&
                k.Knjiga.ISBN == kupovina.Knjiga.ISBN &&
                k.DatumKupovine == kupovina.DatumKupovine);

            if (postojeca != null)
            {
                kupovine.Remove(postojeca);
                Save(kupovine);
            }
        }
    }
}
