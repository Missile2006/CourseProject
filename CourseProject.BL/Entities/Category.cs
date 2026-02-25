using CourseProject.BL.Exceptions;

namespace CourseProject.BL.Entities
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public List<Product> Products { get; private set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
            Products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            if (Products.Count >= 100)
                throw new CategoryLimitException("Перевищено ліміт кількості товарів у категорії.");

            Products.Add(product);
        }

        public void RemoveProduct(int productId)
        {
            var product = Products.Find(p => p.Id == productId);
            if (product == null)
                throw new ProductNotFoundException("Товар не знайдено.");

            Products.Remove(product);
        }
    }
}
