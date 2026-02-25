namespace CourseProject.BL
{
    internal class ObjectCollection<T>
    {
        private ICollection<T> items;

        public ObjectCollection()
        {
            items = new List<T>();
        }

        public void AddItem(T item)
        {
            items.Add(item);
        }

        public void RemoveItem(T item)
        {
            items.Remove(item);
        }

        public ICollection<T> GetAllItems()
        {
            return items;
        }
    }
}
