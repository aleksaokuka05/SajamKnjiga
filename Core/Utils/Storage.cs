using System;
using System.Collections.Generic;
using System.IO;
using Core.Utils;
using Core.Interfaces;

namespace Core.Utils
{
    public class Storage<T> where T : ISerializable, new()
    {

        private readonly string _imeFajla;
        private readonly Serializer<T> _serijalizator = new();


        public Storage(string imeFajla)
        {
            _imeFajla = DataPaths.GetFilePath(imeFajla);
            Directory.CreateDirectory(DataPaths.DataDirectory);
        }

        public List<T> Load()
        {
            if (!File.Exists(_imeFajla))
            {
                FileStream tokFajla = File.Create(_imeFajla);
                tokFajla.Close();
            }

            IEnumerable<string> linije = File.ReadLines(_imeFajla);
            List<T> objekti = _serijalizator.FromCSV(linije);

            return objekti;
        }


        public void Save(List<T> objekti)
        {
            string serijalizovaniObjekti = _serijalizator.ToCSV(objekti);
            Directory.CreateDirectory(DataPaths.DataDirectory);

            using (StreamWriter pisac = new StreamWriter(_imeFajla))
            {
                pisac.Write(serijalizovaniObjekti);
            }
        }

    }
}