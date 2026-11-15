using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface ISerializable
    {
        string[] ToCSV();

        void FromCSV(string[] values);
    }
}