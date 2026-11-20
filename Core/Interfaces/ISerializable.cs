using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{

    // Izvor: OISISI, Vezbe 3, SerializationExample/Serializable.cs
    public interface ISerializable
    {
        string[] ToCSV();

        void FromCSV(string[] values);
    }
}