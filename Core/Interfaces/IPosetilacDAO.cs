using Core.Models;

namespace Core.Interfaces
{
    public interface IPosetilacDAO
    {
        List<Posetilac> GetAll();

        void Save(List<Posetilac> posetioci);

        void Add(Posetilac posetilac);

        void Update(Posetilac posetilac);

        void Delete(string brojClanskeKarte);
    }
}