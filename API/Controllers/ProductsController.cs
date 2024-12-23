using API.Extensions;
using API.Helper;
using API.Helper.Constants;
using API.Helper.Pagination;
using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("getproducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] ProductParams productParams)
        {
            var result = await _productService.Get(productParams);
            if (result.StatusCode == StatusCodes.Status200OK && result.Response is not null)
            {
                var responseObject = result.Response as dynamic;
                if (responseObject?.MetaData is not null)
                {
                    Response.AddPaginationHeader(responseObject.MetaData as MetaData);
                }

            }
            return ResponseHandler.CreateResponse(result);
        }

        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {

            if (id == 0)
                return ResponseHandler.CreateResponse(ResponseVM.InvalidRequest(MessageConstants.InvalidValue,HttpContext?.TraceIdentifier));

            return ResponseHandler.CreateResponse(await _productService.Get(id));
        }
    }
}