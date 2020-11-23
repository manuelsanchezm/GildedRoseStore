using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRoseStore.Models
{
    public static class Repository
    {
        private static List<Item> items = new List<Item>()
        {
            new Item { id = 1, name = "Trouser", color = "blue", size="medium", price = 2.5M, inStock = 5, image = "trouser.jpg"},
            new Item { id = 2, name = "T-Shirt", color = "red", size="large", price = 2.5M, inStock = 2 , image = "tshirt.jpg"},
            new Item { id = 3, name = "Hoodie", color = "white", size="small", price = 2.5M, inStock = 2 , image = "hoodie.jpg"}
        };

        public static IEnumerable<Item> Items => items;

        public static Item GetItemById(int id)
        {
            return Items.FirstOrDefault(item => item.id == id);
        }

        public static void PurchaseItem(int id)
        {
            var item = items.Find(item => item.id == id);

            item.inStock--;
        }
    }
}
