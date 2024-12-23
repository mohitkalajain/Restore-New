using API.Data;
using API.Entities;
using API.Extensions;
using API.Helper;
using API.Helper.Constants;
using API.Helper.Pagination;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Repository.Implementation
{
    public class ProductService : IProductService
    {
        private readonly StoreContext _storeContext;
        private readonly IResponseService _responseService;
        public ProductService(StoreContext storeContext, IResponseService responseService)
        {
            _storeContext = storeContext;
            _responseService = responseService;
        }
        public Task<ResponseVM> Add(ProductVM model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseVM> Delete(int id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Return All prodcct List
        /// </summary>
        /// <returns></returns>
        
        public async Task<ResponseVM> Get(ProductParams productParams)
        {

            var query =  _storeContext.Products
                                      .Sort(productParams.OrderBy)
                                      .Search(productParams.SearctTerm)
                                      .Filter(productParams.Brands, productParams.Types)
                                      .AsQueryable();
           
            var result = await PagedList<Product>.ToPagedList(query, 
                                                              productParams.PageNumber, 
                                                              productParams.PageSize);
           

            return _responseService.Success(MessageConstants.Success,result);

        }

        public async Task<ResponseVM> Get(int id)
        {

            Product product = await _storeContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
                return _responseService.NoContentFound(MessageConstants.NotFound);

            return _responseService.Success(MessageConstants.Success, product);

        }

        public Task<ResponseVM> Update(int id, ProductVM model)
        {
            throw new NotImplementedException();
        }
    }
}