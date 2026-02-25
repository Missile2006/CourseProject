using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.BL.Entities;

namespace CourseProject.BL.Repositories
{
    public interface ISupplierRepository
    {
        Supplier GetById(int id);  
        IEnumerable<Supplier> GetAll();
        void Add(Supplier supplier);
        void Delete(int id);
    }
}
