using CourseProject.BL.Entities;
using CourseProject.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BL.Repositories
{
    public class SupplierRepository : Repository<Supplier>
    {
        public override void Update(Supplier supplier)
        {
            var existing = GetById(supplier.Id);
            if (existing != null)
            {
                existing.FirstName = supplier.FirstName;
                existing.LastName = supplier.LastName;
            }
            else
            {
                throw new SupplierNotFoundException($"Постачальник з ID {supplier.Id} не знайдений.");
            }
        }

        public override Supplier GetById(int id)
        {
            return items.Find(s => s.Id == id);
        }
    }
}
