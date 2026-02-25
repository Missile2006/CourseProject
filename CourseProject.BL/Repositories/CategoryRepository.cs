using CourseProject.BL.Entities;
using CourseProject.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BL.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public override void Update(Category category)
        {
            var existing = GetById(category.Id);
            if (existing != null)
            {
                existing.Name = category.Name;
            }
            else
            {
                throw new CategoryNotFoundException($"Категорія з ID {category.Id} не знайдена.");
            }
        }

        public override Category GetById(int id)
        {
            return items.Find(c => c.Id == id);
        }
    }
}
