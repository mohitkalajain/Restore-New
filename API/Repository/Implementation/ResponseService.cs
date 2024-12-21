using API.Helper;
using API.Repository.Interface;

namespace API.Repository.Implementation
{
    public class ResponseService : IResponseService
    {
        public ResponseVM Success(string message, dynamic response = null)
        {
            return new ResponseVM(true, StatusCodes.Status200OK, message, response);
        }
        public ResponseVM NoContentFound(string message)
        {
            return new ResponseVM(true, StatusCodes.Status204NoContent, message, null);
        }

    }
}