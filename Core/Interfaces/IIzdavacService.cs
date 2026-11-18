using Core.Models;

namespace Core.Interfaces
{
    public interface IIzdavacService
    {
        List<Izdavac> GetAll();
        void Add(Izdavac izdavac);
        void Update(Izdavac izdavac);
        void Delete(int id);
    }
    }