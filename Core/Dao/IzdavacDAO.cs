using System;
using System.Collections.Generic;
using System.IO;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    public class IzdavacDAO
    {
        // Putanja
        private readonly string _filePath = "Data" + Path.DirectorySeparatorChar + "izdavaci.csv";

        private readonly Serializer<Izdavac> _serializer = new Serializer<Izdavac>();

        // Ucitavanje izdavaca
        public List<Izdavac> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Izdavac>();
            }

            string[] lines = File.ReadAllLines(_filePath);

            return _serializer.FromCSV(lines);
        }

        // Cuvanje izdavaca
        public void Save(List<Izdavac> izdavaci)
        {
            string content = _serializer.ToCSV(izdavaci);

            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            File.WriteAllText(_filePath, content);
        }

        // Dodavanje novog izdavaca
        public void Add(Izdavac izdavac)
        {
            List<Izdavac> izdavaci = GetAll();

            izdavaci.Add(izdavac);

            Save(izdavaci);
        }

        // Izmena
        public void Update(Izdavac izdavac)
        {
            List<Izdavac> izdavaci = GetAll();

            int index = izdavaci.FindIndex(i =>
                i.Sifra == izdavac.Sifra);

            if (index != -1)
            {
                izdavaci[index] = izdavac;
                Save(izdavaci);
            }
        }

        // Brisanje
        public void Delete(string sifra)
        {
            List<Izdavac> izdavaci = GetAll();

            Izdavac izdavac = izdavaci.Find(i =>
                i.Sifra == sifra);

            if (izdavac != null)
            {
                izdavaci.Remove(izdavac);
                Save(izdavaci);
            }
        }


    }
}
