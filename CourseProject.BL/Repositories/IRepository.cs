using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BL.Repositories
{
    public interface IRepository<T>
    {
        void Add(T item);
        void Remove(int id);
        void Update(T item);
        T GetById(int id);
        List<T> GetAll();
    }
}
