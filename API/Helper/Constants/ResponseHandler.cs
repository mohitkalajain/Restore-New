using Microsoft.AspNetCore.Mvc;

namespace API.Helper.Constants
{
    public static class ResponseHandler
    {
        public static IActionResult CreateResponse(ResponseVM response)
        {
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }
    }
}