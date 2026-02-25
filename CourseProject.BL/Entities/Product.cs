namespace CourseProject.BL.Entities
{
    [Serializable]
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Supplier Supplier { get; set; }

        public Product(int id, string name, string brand, decimal price, int quantity, Supplier supplier)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Price = price;
            Quantity = quantity;
            Supplier = supplier;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Кількість не може бути від'ємною.");

            Quantity = newQuantity;
        }
    }
}
