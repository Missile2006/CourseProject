using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.BL.Entities;

namespace CourseProject.BL.Repositories
{
    public interface ICategoryRepository
    {
        Category GetById(int id);    
        IEnumerable<Category> GetAll(); 
        void Add(Category category);
        void Delete(int id);
    }
}
