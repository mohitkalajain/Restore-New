using API.Helper;

namespace API.Repository.Interface
{
    public interface IBasketService
    {
        Task<ResponseVM> GetBasket(string basketId);
        Task<ResponseVM> Get();
        Task<ResponseVM> CreateBasket(string buyerId,int productId,int quantity);
        Task<ResponseVM> RemoveBasket(string buyerId,int productId,int quantity);
    }
}