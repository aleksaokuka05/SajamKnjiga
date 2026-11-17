using System;
using System.Collections.Generic;
using System.IO;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    public class PosetilacDAO : IPosetilacDAO
    {
        private readonly string _filePath = "Data" + Path.DirectorySeparatorChar + "posetioci.csv";
        private readonly Serializer<Posetilac> _serializer = new Serializer<Posetilac>();

        // Metoda GetAll ucitava posetioce iz CSV fajla
        public List<Posetilac> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Posetilac>();
            }

            string[] lines = File.ReadAllLines(_filePath);

            return _serializer.FromCSV(lines);
        }

        // Metoda Save cuva posetioce u CSV fajl
        public void Save(List<Posetilac> posetioci)
        {
            string content = _serializer.ToCSV(posetioci);

            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            File.WriteAllText(_filePath, content);
        }

        // Dodavanje posetioca
        public void Add(Posetilac posetilac)
        {
            List<Posetilac> posetioci = GetAll();

            posetioci.Add(posetilac);

            Save(posetioci);
        }

        // Izmene podataka za posetioce
        public void Update(Posetilac posetilac)
        {
            List<Posetilac> posetioci = GetAll();

            int index = posetioci.FindIndex(p => p.BrojClanskeKarte == posetilac.BrojClanskeKarte);

            if (index != -1)
            {
                posetioci[index] = posetilac;
                Save(posetioci);
            }
        }

        // Brisanje posetioca iz podataka
        public void Delete(string brojClanskeKarte)
        {
            List<Posetilac> posetioci = GetAll();

            Posetilac posetilac = posetioci.Find(p => p.BrojClanskeKarte == brojClanskeKarte);

            if (posetilac != null)
            {
                posetioci.Remove(posetilac);
                Save(posetioci);
            }
        }
    }
}