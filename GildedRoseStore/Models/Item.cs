namespace GildedRoseStore.Models
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public string color{ get; set; }
        public decimal price { get; set; }
        public int inStock { get; set; }
        public string image { get; set; }
    }
}
