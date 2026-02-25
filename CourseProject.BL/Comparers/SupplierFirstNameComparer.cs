using CourseProject.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BL.Comparers
{
    public class SupplierFirstNameComparer : IComparer<Supplier>
    {
        public int Compare(Supplier x, Supplier y)
        {
            return string.Compare(x.FirstName, y.FirstName);
        }
    }
}
