using API.Data;
using API.DTOs;
using API.Entities;
using API.Enums;
using API.Helper;
using API.Helper.Constants;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Repository.Implementation
{
    public class BasketService : IBasketService
    {
        private readonly StoreContext _storeContext;
        private readonly IResponseService _responseService;
        public BasketService(StoreContext storeContext, IResponseService responseService)
        {
            _storeContext = storeContext;
            _responseService = responseService;
        }
        public async Task<ResponseVM> GetBasket(string buyerId)
        {
            var basket = await _storeContext.Baskets
                                        .Include(i => i.Items)
                                        .ThenInclude(p => p.Product)
                                        .FirstOrDefaultAsync(x => x.BuyerId == buyerId);

            if (basket is null) return _responseService.NoContentFound(MessageConstants.NotFound);

            return _responseService.Success(MessageConstants.Success, MapBasketToDto(basket));
        }

        private static BasketDTO MapBasketToDto(Basket basket)
        {
            BasketDTO basketDTO = new()
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemDTO
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity
                }).ToList()
            };
            return basketDTO;
        }

        public Task<ResponseVM> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseVM> CreateBasket(string buyerId, int productId, int quantity)
        {
            // Fetch the product early to ensure it exists before processing further
            var product = await _storeContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (product is null) return _responseService.NoContentFound(MessageConstants.NotFound);

            var basket = await _storeContext.Baskets
                                         .Include(i => i.Items)
                                         .ThenInclude(p => p.Product)
                                         .FirstOrDefaultAsync(x => x.BuyerId == buyerId);

            //check basket is empty
            if (basket is null)
            {
                //create new basket
                buyerId = Guid.NewGuid().ToString();
                basket = new Basket { BuyerId = buyerId };
                _storeContext.Baskets.Add(basket);
            }

            // Add the item to the basket
            basket.AddItem(product, quantity);

            // Save changes to the database
            await _storeContext.SaveChangesAsync();
            var res = await this.GetBasket(buyerId);
            return _responseService.Success(MessageConstants.Success, res.Response);
        }

        public async Task<ResponseVM> RemoveBasket(string buyerId, int productId, int quantity)
        {
            // Fetch the basket with its items and related product details
            var basket = await _storeContext.Baskets
                                         .Include(i => i.Items)
                                         .ThenInclude(p => p.Product)
                                         .FirstOrDefaultAsync(x => x.BuyerId == buyerId);

            if (basket is null) return _responseService.NoContentFound(MessageConstants.NotFound);

            // Remove the item and handle the result
            var result = basket.RemoveItem(productId, quantity);

            // Map the result to an appropriate response
            var responseMessage = result switch
            {
                RemoveBasketItem.Success => MessageConstants.BasketSuccess,
                RemoveBasketItem.ItemNotFound => MessageConstants.BasketItemNotFound,
                RemoveBasketItem.InsufficientQuantity => MessageConstants.BasketInsufficientQuantity,
                _ => MessageConstants.UnexpectedError
            };

            if (result == RemoveBasketItem.Success)
                await _storeContext.SaveChangesAsync();


            return result == RemoveBasketItem.Success ?
                    _responseService.Success(responseMessage, string.Empty)
                    : _responseService.BadRequest(responseMessage);
        }
    }
}