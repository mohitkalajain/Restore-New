using API.Helper;
using API.Helper.Constants;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("not-found")]
        public async Task<IActionResult> GetNotFound()
        {
            var response = new ResponseVM(
                    true,
                    statusCode: StatusCodes.Status404NotFound,
                    message: "Resource not found",
                    new { }
            );
            return await Task.FromResult(ResponseHandler.CreateResponse(response));
        }
        [HttpGet("bad-request")]
        public async Task<IActionResult> GetBadRequest()
        {
            var response = new ResponseVM(
                true,
                statusCode: StatusCodes.Status400BadRequest,
                message: "This is a bad request",
               new { });
            return await Task.FromResult(ResponseHandler.CreateResponse(response));
        }
        [HttpGet("unauthorized")]
        public async Task<IActionResult> GetUnauthorized()
        {
            var response = new ResponseVM(
               true,
               statusCode: StatusCodes.Status401Unauthorized,
               message: "unauthorized request",
              new { });
            return await Task.FromResult(ResponseHandler.CreateResponse(response));
        }
        [HttpGet("validation-error")]
        public async Task<IActionResult> GetValidationError()
        {
            ModelState.AddModelError("Problem1", "This is the first error");
            ModelState.AddModelError("Problem2", "This is the second error");

            var response = new ResponseVM
           (
            true,
                statusCode: StatusCodes.Status400BadRequest,
                message: "Validation errors occurred",
                response: ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .ToDictionary(
                        ms => ms.Key,
                        ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
           );
            return await Task.FromResult(ResponseHandler.CreateResponse(response));
        }
        [HttpGet("server-error")]
        public async Task<IActionResult> GetServerError()
        {
            try
            {
                throw new Exception("This is a server error");
            }
            catch (Exception ex)
            {
                var response = new ResponseVM
                (
                    true,
                    statusCode: StatusCodes.Status500InternalServerError,
                    message: "Internal server error",
                    response: ex.Message
                );
                return await Task.FromResult(ResponseHandler.CreateResponse(response));
            }
        }
    }
}