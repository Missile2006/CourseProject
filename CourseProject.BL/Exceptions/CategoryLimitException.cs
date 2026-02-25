using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BL.Exceptions
{
    [Serializable]
    public class CategoryLimitException : Exception
    {
        public CategoryLimitException(string message) : base(message) { }
    }
}
