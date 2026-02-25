using System;
using System.Collections.Generic;
using System.Linq;
using CourseProject.BL.Entities;
using CourseProject.BL.Exceptions;
using CourseProject.BL.Repositories;

namespace CourseProject.BL.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ISupplierRepository _supplierRepo;

        // Словник дій для оновлення полів
        private readonly Dictionary<string, Action<Product, int>> _updateActions;
        private ProductRepository productRepo;
        private CategoryRepository categoryRepo;
        private SupplierRepository supplierRepo;

        public ProductService(IProductRepository productRepo,
                              ICategoryRepository categoryRepo,
                              ISupplierRepository supplierRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _supplierRepo = supplierRepo;

            _updateActions = new Dictionary<string, Action<Product, int>>()
            {
                {"1", (p, _) => UpdateName(p)},
                {"2", (p, _) => UpdateBrand(p)},
                {"3", (p, _) => UpdatePrice(p)},
                {"4", (p, _) => UpdateSupplier(p)},
                {"5", (p, id) => UpdateCategory(p, id)}
            };
        }

        public ProductService(ProductRepository productRepo, CategoryRepository categoryRepo, SupplierRepository supplierRepo)
        {
            this.productRepo = productRepo;
            this.categoryRepo = categoryRepo;
            this.supplierRepo = supplierRepo;
        }

        public void UpdateProductInteractive()
        {
            int id = ReadProductId();
            var product = GetProductById(id);

            string choice = ShowUpdateMenuAndReadChoice();

            if (_updateActions.TryGetValue(choice, out var action))
            {
                action(product, id);
                _productRepo.Update(product);
                Console.WriteLine("Дані товару успішно оновлено.");
            }
            else
            {
                Console.WriteLine("Невірний вибір.");
            }
        }

        private int ReadProductId()
        {
            Console.Write("Введіть ID товару для зміни: ");
            return ProductValidator.ParseId(Console.ReadLine(), "Некоректний ID.");
        }

        private Product GetProductById(int id)
        {
            var product = _productRepo.GetById(id);
            if (product == null)
                throw new ProductNotFoundException("Товар не знайдено.");
            return product;
        }

        private string ShowUpdateMenuAndReadChoice()
        {
            Console.WriteLine("Виберіть поле для оновлення:");
            Console.WriteLine("1. Назва");
            Console.WriteLine("2. Бренд");
            Console.WriteLine("3. Ціна");
            Console.WriteLine("4. Постачальник");
            Console.WriteLine("5. Категорія");
            Console.Write("Виберіть опцію: ");
            return Console.ReadLine();
        }

        private void UpdateName(Product product)
        {
            Console.Write("Введіть нову назву товару: ");
            string newName = Console.ReadLine();
            ProductValidator.ValidateName(newName);
            product.Name = newName;
        }

        private void UpdateBrand(Product product)
        {
            Console.Write("Введіть новий бренд товару: ");
            string newBrand = Console.ReadLine();
            ProductValidator.ValidateBrand(newBrand);
            product.Brand = newBrand;
        }

        private void UpdatePrice(Product product)
        {
            Console.Write("Введіть нову ціну товару: ");
            string s = Console.ReadLine();
            if (!decimal.TryParse(s, out decimal newPrice))
                throw new ArgumentException("Ціна повинна бути числом.");
            ProductValidator.ValidatePrice(newPrice);
            product.Price = newPrice;
        }

        private void UpdateSupplier(Product product)
        {
            Console.Write("Введіть новий ID постачальника: ");
            string s = Console.ReadLine();
            int newSupplierId = ProductValidator.ParseId(s, "Некоректний ID постачальника.");

            var newSupplier = _supplierRepo.GetById(newSupplierId)
                ?? throw new SupplierNotFoundException("Постачальника не знайдено.");

            product.Supplier = newSupplier;
        }

        private void UpdateCategory(Product product, int productId)
        {
            Console.Write("Введіть новий ID категорії: ");
            string s = Console.ReadLine();
            int newCategoryId = ProductValidator.ParseId(s, "Некоректний ID категорії.");

            var newCategory = _categoryRepo.GetById(newCategoryId)
                ?? throw new CategoryNotFoundException("Категорію не знайдено.");

            var oldCategory = _categoryRepo.GetAll().FirstOrDefault(c => c.Products.Contains(product));
            oldCategory?.RemoveProduct(productId);

            newCategory.AddProduct(product);
        }
    }
}
