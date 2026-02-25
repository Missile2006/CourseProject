using CourseProject.BL.Entities;
using CourseProject.BL.Exceptions;
using CourseProject.BL;
using System.Text;
using CourseProject.BL.Comparers;
using CourseProject.BL.Repositories;
using CourseProject.BL.Services;


namespace CourseProject
{
    class Program
    {

        static CategoryRepository categoryRepo = new();
        static ProductRepository productRepo = new();
        static SupplierRepository supplierRepo = new();
        static ProductService productService = new(productRepo, categoryRepo, supplierRepo);

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;


            DataStorage.LoadAll(categoryRepo, productRepo, supplierRepo);

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Електронний облік товарів");
                Console.WriteLine("1. Управління категоріями");
                Console.WriteLine("2. Управління товарами");
                Console.WriteLine("3. Управління постачальниками");
                Console.WriteLine("4. Пошук");
                Console.WriteLine("0. Вихід");
                Console.Write("Виберіть опцію: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ManageCategories();
                        break;
                    case "2":
                        ManageProducts();
                        break;
                    case "3":
                        ManageSuppliers();
                        break;
                    case "4":
                        SearchMenu();
                        break;
                    case "0":
                        exit = true;
                        DataStorage.SaveAll(categoryRepo, productRepo, supplierRepo);
                        Console.WriteLine("Дані збережено. До побачення!");
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Натисніть будь-яку клавішу для продовження...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        #region Управління Категоріями

        static void ManageCategories()
        {
            bool back = false;

            var menu = new Dictionary<string, Action>()
    {
        {"1", AddCategory},
        {"2", RemoveCategory},
        {"3", UpdateCategory},
        {"4", ViewCategory},
        {"5", ViewAllCategories},
        {"0", () => back = true}
    };

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("Управління категоріями");
                Console.WriteLine("1. Додати категорію");
                Console.WriteLine("2. Видалити категорію");
                Console.WriteLine("3. Змінити категорію");
                Console.WriteLine("4. Переглянути категорію");
                Console.WriteLine("5. Переглянути всі категорії");
                Console.WriteLine("0. Назад");
                Console.Write("Виберіть опцію: ");

                var choice = Console.ReadLine();
                try
                {
                    if (menu.TryGetValue(choice, out var action))
                        action();
                    else
                        Console.WriteLine("Невірний вибір.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }

                if (!back)
                {
                    Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                    Console.ReadKey();
                }
            }
        }

        static Category GetCategoryByIdInteractive()
        {
            Console.Write("Введіть ID категорії: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new ArgumentException("Некоректний ID.");

            var category = categoryRepo.GetById(id);
            if (category == null)
                throw new CategoryNotFoundException("Категорія не знайдена.");
            return category;
        }

        static void AddCategory()
        {
            Console.Write("Введіть назву категорії: ");
            var name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва категорії не може бути порожньою.");

            int newId = categoryRepo.GetAll().Count > 0 ? categoryRepo.GetAll().Max(c => c.Id) + 1 : 1;
            var category = new Category(newId, name);
            categoryRepo.Add(category);
            Console.WriteLine("Категорію успішно додано.");
        }

        static void RemoveCategory()
        {
            var category = GetCategoryByIdInteractive();
            if (category.Products.Count > 0)
                throw new InvalidOperationException("Неможливо видалити категорію, оскільки в ній є товари.");

            categoryRepo.Remove(category.Id);
            Console.WriteLine("Категорію успішно видалено.");
        }

        static void UpdateCategory()
        {
            var category = GetCategoryByIdInteractive();

            Console.Write("Введіть нову назву категорії: ");
            var newName = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Назва категорії не може бути порожньою.");

            category.Name = newName;
            categoryRepo.Update(category);
            Console.WriteLine("Категорію успішно оновлено.");
        }

        static void ViewCategory()
        {
            var category = GetCategoryByIdInteractive();
            Console.WriteLine($"ID: {category.Id}, Назва: {category.Name}, Кількість товарів: {category.Products.Count}");
        }

        static void ViewAllCategories()
        {
            var categories = categoryRepo.GetAll();
            if (!categories.Any())
            {
                Console.WriteLine("Немає категорій.");
                return;
            }

            Console.WriteLine("Список категорій:");
            foreach (var category in categories)
            {
                Console.WriteLine($"ID: {category.Id}, Назва: {category.Name}, Кількість товарів: {category.Products.Count}");
            }
        }

        #endregion


        #region Управління Товарами

        static void ManageProducts()
        {
            bool back = false;

            var menu = new Dictionary<string, Action>()
    {
        {"1", AddProduct},
        {"2", RemoveProduct},
        {"3", () => productService.UpdateProductInteractive()},
        {"4", UpdateProductQuantity},
        {"5", ViewProduct},
        {"6", ViewAllProducts},
        {"0", () => back = true}
    };

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("Управління товарами");
                Console.WriteLine("1. Додати товар");
                Console.WriteLine("2. Видалити товар");
                Console.WriteLine("3. Змінити дані товару");
                Console.WriteLine("4. Змінити кількість товару на складі");
                Console.WriteLine("5. Переглянути дані конкретного товару");
                Console.WriteLine("6. Переглянути список всіх товарів");
                Console.WriteLine("0. Назад");
                Console.Write("Виберіть опцію: ");

                var choice = Console.ReadLine();
                try
                {
                    if (menu.TryGetValue(choice, out var action))
                        action();
                    else
                        Console.WriteLine("Невірний вибір.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }

                if (!back)
                {
                    Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                    Console.ReadKey();
                }
            }
        }

        static Product GetProductByIdInteractive()
        {
            Console.Write("Введіть ID товару: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new ArgumentException("Некоректний ID.");

            var product = productRepo.GetById(id);
            if (product == null)
                throw new ProductNotFoundException("Товар не знайдено.");
            return product;
        }

        static Category GetCategoryForProduct(Product product)
        {
            return categoryRepo.GetAll().FirstOrDefault(c => c.Products.Any(p => p.Id == product.Id));
        }

        static void AddProduct()
        {
            Console.Write("Введіть назву товару: ");
            var name = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Назва товару не може бути порожньою.");

            Console.Write("Введіть бренд товару: ");
            var brand = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(brand)) throw new ArgumentException("Бренд товару не може бути порожнім.");

            Console.Write("Введіть ціну товару: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
                throw new ArgumentException("Ціна повинна бути позитивним числом.");

            Console.Write("Введіть кількість товару на складі: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0 || quantity > 100)
                throw new ArgumentException("Кількість повинна бути від 0 до 100.");

            Console.Write("Введіть ID постачальника: ");
            if (!int.TryParse(Console.ReadLine(), out int supplierId))
                throw new ArgumentException("Некоректний ID постачальника.");
            var supplier = supplierRepo.GetById(supplierId) ?? throw new SupplierNotFoundException("Постачальника з таким ID не знайдено.");

            Console.Write("Введіть ID категорії: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryId))
                throw new ArgumentException("Некоректний ID категорії.");
            var category = categoryRepo.GetById(categoryId) ?? throw new CategoryNotFoundException("Категорію з таким ID не знайдено.");

            int newId = productRepo.GetAll().Count > 0 ? productRepo.GetAll().Max(p => p.Id) + 1 : 1;
            var product = new Product(newId, name, brand, price, quantity, supplier);
            productRepo.Add(product);
            category.AddProduct(product);
            Console.WriteLine("Товар успішно додано.");
        }

        static void RemoveProduct()
        {
            var product = GetProductByIdInteractive();
            var category = GetCategoryForProduct(product);
            category?.RemoveProduct(product.Id);
            productRepo.Remove(product.Id);
            Console.WriteLine("Товар успішно видалено.");
        }

        static void UpdateProductQuantity()
        {
            var product = GetProductByIdInteractive();
            Console.Write("Введіть нову кількість товару на складі: ");
            if (!int.TryParse(Console.ReadLine(), out int newQuantity) || newQuantity < 0)
                throw new ArgumentException("Кількість повинна бути позитивним числом.");

            product.UpdateQuantity(newQuantity);
            productRepo.Update(product);
            Console.WriteLine("Кількість товару успішно оновлено.");
        }

        static void ViewProduct()
        {
            var product = GetProductByIdInteractive();
            var category = GetCategoryForProduct(product);
            Console.WriteLine($"ID: {product.Id}, Назва: {product.Name}, Бренд: {product.Brand}, Ціна: {product.Price}, Кількість: {product.Quantity}, Постачальник: {product.Supplier.FirstName} {product.Supplier.LastName}, Категорія: {category?.Name ?? "—"}");
        }

        static void ViewAllProducts()
        {
            var products = productRepo.GetAll();
            if (!products.Any())
            {
                Console.WriteLine("Немає товарів.");
                return;
            }

            Console.WriteLine("Сортувати за:");
            Console.WriteLine("1. Назвою");
            Console.WriteLine("2. Брендом");
            Console.WriteLine("3. Ціною");
            Console.Write("Виберіть опцію: ");
            var choice = Console.ReadLine();

            var comparers = new Dictionary<string, Comparison<Product>>()
    {
        {"1", (a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase)},
        {"2", (a, b) => string.Compare(a.Brand, b.Brand, StringComparison.OrdinalIgnoreCase)},
        {"3", (a, b) => a.Price.CompareTo(b.Price)}
    };

            if (comparers.TryGetValue(choice, out var comparer))
                products.Sort(comparer);
            else
                Console.WriteLine("Невірний вибір. Сортування не застосовано.");

            foreach (var product in products)
                Console.WriteLine($"ID: {product.Id}, Назва: {product.Name}, Бренд: {product.Brand}, Ціна: {product.Price}, Кількість: {product.Quantity}");
        }

        #endregion


        #region Управління Постачальниками

        static void ManageSuppliers()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("Управління постачальниками");
                Console.WriteLine("1. Додати постачальника");
                Console.WriteLine("2. Видалити постачальника");
                Console.WriteLine("3. Змінити дані постачальника");
                Console.WriteLine("4. Переглянути дані конкретного постачальника");
                Console.WriteLine("5. Переглянути список всіх постачальників");
                Console.WriteLine("0. Назад");
                Console.Write("Виберіть опцію: ");

                var choice = Console.ReadLine();
                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddSupplier();
                            break;
                        case "2":
                            RemoveSupplier();
                            break;
                        case "3":
                            UpdateSupplier();
                            break;
                        case "4":
                            ViewSupplier();
                            break;
                        case "5":
                            ViewAllSuppliers();
                            break;
                        case "0":
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Невірний вибір.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
                Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        static void AddSupplier()
        {
            Console.Write("Введіть ім'я постачальника: ");
            var firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Ім'я постачальника не може бути порожнім.");

            Console.Write("Введіть прізвище постачальника: ");
            var lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Прізвище постачальника не може бути порожнім.");

            int newId = supplierRepo.GetAll().Count > 0 ? supplierRepo.GetAll().Max(s => s.Id) + 1 : 1;
            var supplier = new Supplier(newId, firstName, lastName);
            supplierRepo.Add(supplier);
            Console.WriteLine("Постачальника успішно додано.");
        }

        static void RemoveSupplier()
        {
            Console.Write("Введіть ID постачальника для видалення: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var supplier = supplierRepo.GetById(id);
                if (supplier == null)
                    throw new SupplierNotFoundException("Постачальника не знайдено.");

                var relatedProducts = productRepo.GetAll().Where(p => p.Supplier.Id == id).ToList();
                if (relatedProducts.Count > 0)
                    throw new InvalidOperationException("Неможливо видалити постачальника, оскільки з ним пов'язані товари.");

                supplierRepo.Remove(id);
                Console.WriteLine("Постачальника успішно видалено.");
            }
            else
            {
                throw new ArgumentException("Некоректний ID.");
            }
        }

        static void UpdateSupplier()
        {
            Console.Write("Введіть ID постачальника для зміни: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var supplier = supplierRepo.GetById(id);
                if (supplier == null)
                    throw new SupplierNotFoundException("Постачальника не знайдено.");

                Console.WriteLine("Виберіть поле для оновлення:");
                Console.WriteLine("1. Ім'я");
                Console.WriteLine("2. Прізвище");
                Console.Write("Виберіть опцію: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Введіть нове ім'я постачальника: ");
                        var newFirstName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newFirstName))
                            throw new ArgumentException("Ім'я постачальника не може бути порожнім.");
                        supplier.FirstName = newFirstName;
                        break;
                    case "2":
                        Console.Write("Введіть нове прізвище постачальника: ");
                        var newLastName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newLastName))
                            throw new ArgumentException("Прізвище постачальника не може бути порожнім.");
                        supplier.LastName = newLastName;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір.");
                        break;
                }

                supplierRepo.Update(supplier);
                Console.WriteLine("Дані постачальника успішно оновлено.");
            }
            else
            {
                throw new ArgumentException("Некоректний ID.");
            }
        }

        static void ViewSupplier()
        {
            Console.Write("Введіть ID постачальника для перегляду: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var supplier = supplierRepo.GetById(id);
                if (supplier == null)
                    throw new SupplierNotFoundException("Постачальника не знайдено.");

                Console.WriteLine($"ID: {supplier.Id}, Ім'я: {supplier.FirstName}, Прізвище: {supplier.LastName}");
            }
            else
            {
                throw new ArgumentException("Некоректний ID.");
            }
        }

        static void ViewAllSuppliers()
        {
            var suppliers = supplierRepo.GetAll();
            if (suppliers.Count == 0)
            {
                Console.WriteLine("Немає постачальників.");
                return;
            }

            Console.WriteLine("Сортувати за:");
            Console.WriteLine("1. Іменем");
            Console.WriteLine("2. Прізвищем");
            Console.Write("Виберіть опцію: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    suppliers.Sort(new SupplierFirstNameComparer());
                    break;
                case "2":
                    suppliers.Sort(new SupplierLastNameComparer());
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Сортування не застосовано.");
                    break;
            }

            Console.WriteLine("Список постачальників:");
            foreach (var supplier in suppliers)
            {
                Console.WriteLine($"ID: {supplier.Id}, Ім'я: {supplier.FirstName}, Прізвище: {supplier.LastName}");
            }
        }

        #endregion

        #region Пошук

        static void SearchMenu()
        {
            bool back = false;

            var menu = new Dictionary<string, Action>()
    {
        {"1", SearchProducts},
        {"2", SearchSuppliers},
        {"0", () => back = true}
    };

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("Пошук");
                Console.WriteLine("1. Пошук товарів за ключовим словом");
                Console.WriteLine("2. Пошук постачальників за ключовим словом");
                Console.WriteLine("0. Назад");
                Console.Write("Виберіть опцію: ");

                var choice = Console.ReadLine();
                if (menu.TryGetValue(choice, out var action))
                {
                    action();
                }
                else
                {
                    Console.WriteLine("Невірний вибір.");
                }

                if (!back)
                {
                    Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                    Console.ReadKey();
                }
            }
        }

        static void SearchProducts()
        {
            SearchByKeyword(
                productRepo.GetAll(),
                p => $"{p.Name} {p.Brand}",
                p =>
                {
                    var categoryName = categoryRepo.GetAll().FirstOrDefault(c => c.Products.Any(pr => pr.Id == p.Id))?.Name ?? "—";
                    Console.WriteLine($"ID: {p.Id}, Назва: {p.Name}, Бренд: {p.Brand}, Ціна: {p.Price}, Кількість: {p.Quantity}, Постачальник: {p.Supplier.FirstName} {p.Supplier.LastName}, Категорія: {categoryName}");
                },
                "товарів"
            );
        }

        static void SearchSuppliers()
        {
            SearchByKeyword(
                supplierRepo.GetAll(),
                s => $"{s.FirstName} {s.LastName}",
                s => Console.WriteLine($"ID: {s.Id}, Ім'я: {s.FirstName}, Прізвище: {s.LastName}"),
                "постачальників"
            );
        }

        /// <summary>
        /// Універсальний метод пошуку за ключовим словом.
        /// </summary>
        static void SearchByKeyword<T>(List<T> items, Func<T, string> textSelector, Action<T> displayAction, string itemName)
        {
            Console.Write($"Введіть ключове слово для пошуку {itemName}: ");
            var keyword = Console.ReadLine()?.ToLower();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("Ключове слово не може бути порожнім.");
                return;
            }

            var results = items.Where(i => textSelector(i).ToLower().Contains(keyword)).ToList();
            if (results.Count == 0)
            {
                Console.WriteLine($"{itemName.FirstCharToUpper()} не знайдено.");
                return;
            }

            Console.WriteLine("Результати пошуку:");
            foreach (var item in results)
                displayAction(item);
        }
        #endregion

    }

    // Добавьте этот статический класс в конец файла Program.cs или в отдельный файл, если требуется переиспользование.
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
