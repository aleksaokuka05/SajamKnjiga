using Core.Models;

namespace Core.Interfaces
{
    public interface IKupovinaService
    {
        List<Kupovina> GetAll();
        void Add(Kupovina kupovina);
        void Update(Kupovina kupovina);
        void Delete(Kupovina kupovina);
    }
}