using CourseProject.BL.Entities;
using CourseProject.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BL.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        public override void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Brand = product.Brand;
                existing.Price = product.Price;
                existing.Quantity = product.Quantity;
                existing.Supplier = product.Supplier;
            }
            else
            {
                throw new ProductNotFoundException($"Товар з ID {product.Id} не знайдений.");
            }
        }

        public override Product GetById(int id)
        {
            return items.Find(p => p.Id == id);
        }
    }
}
