using CourseProject.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BL.Repositories
{
    public abstract class Repository<T> : IRepository<T>
    {
        protected List<T> items = new List<T>();

        public virtual void Add(T item)
        {
            items.Add(item);
        }

        public virtual void Remove(int id)
        {
            var item = GetById(id);
            if (item != null)
                items.Remove(item);
            else
                throw new ItemNotFoundException($"Елемент з ID {id} не знайдено.");
        }

        public abstract void Update(T item);

        public virtual T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual List<T> GetAll()
        {
            return items;
        }
    }
}
