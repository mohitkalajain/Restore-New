using API.Helper;
using API.Helper.Pagination;
using API.Models;


namespace API.Repository.Interface
{
    public interface IProductService
    {
        public Task<ResponseVM> Get(ProductParams productParams);
        public Task<ResponseVM> Get(int id);
        public Task<ResponseVM> Add(ProductVM model);
        public Task<ResponseVM> Update(int id,ProductVM model);
        public Task<ResponseVM> Delete(int id);
    }
}