using API.Helper;
using API.Repository.Interface;

namespace API.Repository.Implementation
{
    public class ResponseService : IResponseService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResponseService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetTraceId()
        {
            return _httpContextAccessor?.HttpContext?.TraceIdentifier ?? string.Empty;
        }
        public ResponseVM Success(string message, dynamic response = null)
        {
            return new ResponseVM(true, StatusCodes.Status200OK, message, response,GetTraceId());
        }
        public ResponseVM NoContentFound(string message)
        {
            return new ResponseVM(true, StatusCodes.Status404NotFound, message, new { }, GetTraceId());
        }
        public ResponseVM BadRequest(string message)
        {
            return new ResponseVM(true, StatusCodes.Status400BadRequest, message, new { }, GetTraceId());
        }
    }
}