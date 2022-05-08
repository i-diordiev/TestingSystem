using System.Collections.Generic;
using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Find(int id);
        bool Add(T item);
        T Update(T item);
        bool Delete(int id);
        IEnumerable<T> GetAll();
    }
}