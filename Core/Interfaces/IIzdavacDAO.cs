using Core.Models;

namespace Core.Interfaces
{
    public interface IIzdavacDAO
    {
        List<Izdavac> GetAll();

        void Save(List<Izdavac> izdavaci);

        void Add(Izdavac izdavac);

        void Update(Izdavac izdavac);

        void Delete(string sifra);
    }
}