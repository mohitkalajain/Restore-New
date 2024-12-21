using API.DTOs;
using API.Entities;
using API.Helper;
using API.Helper.Constants;
using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet(Name = "getbasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBasket()
        {
            string buyerId = Request.Cookies["buyerId"];
            var results = ResponseHandler.CreateResponse(await _basketService.GetBasket(buyerId));
            return results;
        }

        [HttpPost("additemtobasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddItemToBasket(int productId, int quantity)
        {
            string buyerId = Request.Cookies["buyerId"];

            var result = await _basketService.CreateBasket(buyerId, productId, quantity);
            if (result.StatusCode == StatusCodes.Status200OK && result.Response is BasketDTO basket && !string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Append("buyerId", basket.BuyerId, new CookieOptions
                {
                    IsEssential = true,
                    Expires = DateTimeOffset.Now.AddDays(30)
                });
            }
            return ResponseHandler.CreateResponse(result);
        }

        [HttpDelete("removebasket")]
        public async Task<IActionResult> Removebasket(int productId, int quantity)
        {
            string buyerId = Request.Cookies["buyerId"];
            var result = await _basketService.RemoveBasket(buyerId, productId, quantity);


            return ResponseHandler.CreateResponse(result);
        }
    }
}