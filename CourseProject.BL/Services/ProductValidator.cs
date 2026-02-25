// Path: CourseProject.BL/Services/ProductValidator.cs
using System;

namespace CourseProject.BL.Services
{
    public static class ProductValidator
    {
        public static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва товару не може бути порожньою.");
        }

        public static void ValidateBrand(string brand)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Бренд товару не може бути порожнім.");
        }

        public static void ValidatePrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Ціна повинна бути позитивним числом.");
        }

        public static int ParseId(string input, string errorMessage = "Некоректний ID.")
        {
            if (!int.TryParse(input, out int id))
                throw new ArgumentException(errorMessage);
            return id;
        }
    }
}
