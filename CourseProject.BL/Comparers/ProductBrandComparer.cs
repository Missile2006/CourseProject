using CourseProject.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BL.Comparers
{
    public class ProductBrandComparer : IComparer<Product>
    {
        public int Compare(Product x, Product y)
        {
            return string.Compare(x.Brand, y.Brand);
        }
    }
}
