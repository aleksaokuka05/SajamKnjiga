using Core.Models;

namespace Core.Interfaces
{
    public interface IPosetilacService
    {
        List<Posetilac> GetAll();

        void Add(Posetilac posetilac);

        void Update(Posetilac posetilac);

        void Delete(string brojClanskeKarte);
    }
}