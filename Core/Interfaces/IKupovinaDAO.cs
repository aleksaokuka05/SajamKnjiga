using Core.Models;

namespace Core.Interfaces
{
    public interface IKupovinaDAO
    {
        List<Kupovina> GetAll();

        void Save(List<Kupovina> kupovine);

        void Add(Kupovina kupovina);

        void Update(Kupovina kupovina);

        void Delete(Kupovina kupovina);
    }
}