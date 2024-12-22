using API.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        [Required]
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new();

        public void AddItem(Product product, int quantity)
        {
            if (Items.All(item => item.ProductId != product.Id))
            {
                Items.Add(new BasketItem { Product = product, Quantity = quantity });
            }

            var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);
            if (existingItem != null) existingItem.Quantity += quantity;
        }

        public RemoveBasketItem RemoveItem(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(item => item.ProductId == productId);

            if (item is null) return RemoveBasketItem.ItemNotFound;

            if (quantity > item.Quantity) return RemoveBasketItem.InsufficientQuantity; 

            if(item.Quantity <= quantity)
            {
                Items.Remove(item);//Remove item if quantity becomes 0 or less
            }
            else
            {
                item.Quantity -= quantity;//Reduce quantity safely
            }
            return RemoveBasketItem.Success;
        }

    }
}