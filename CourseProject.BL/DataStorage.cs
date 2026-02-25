using CourseProject.BL.Entities;
using CourseProject.BL.Repositories;
using Newtonsoft.Json;

namespace CourseProject.BL
{
    public static class DataStorage
    {
        private static readonly string CategoryFile = "categories.json";
        private static readonly string ProductFile = "products.json";
        private static readonly string SupplierFile = "suppliers.json";
        private static readonly object _lock = new object();

        public static void SaveData<T>(List<T> data, string fileName)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(fileName, json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving data to {fileName}: {ex.Message}");
                throw;
            }
        }

        public static List<T> LoadData<T>(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                    return new List<T>();

                var json = File.ReadAllText(fileName);
                var a = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading data from {fileName}: {ex.Message}");
                return new List<T>();
            }
        }

        public static void SaveAll(IRepository<Category> categoryRepo, IRepository<Product> productRepo, IRepository<Supplier> supplierRepo)
        {
            lock (_lock)
            {
                SaveData(categoryRepo.GetAll(), CategoryFile);
                SaveData(productRepo.GetAll(), ProductFile);
                SaveData(supplierRepo.GetAll(), SupplierFile);
            }
        }

        public static void LoadAll(IRepository<Category> categoryRepo, IRepository<Product> productRepo, IRepository<Supplier> supplierRepo)
        {
            lock (_lock)
            {
                var categories = LoadData<Category>(CategoryFile);
                var products = LoadData<Product>(ProductFile);
                var suppliers = LoadData<Supplier>(SupplierFile);

                categories.ForEach(c => categoryRepo.Add(c));
                products.ForEach(p => productRepo.Add(p));
                suppliers.ForEach(s => supplierRepo.Add(s));
            }
        }
    }
}
