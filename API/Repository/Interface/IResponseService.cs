using API.Helper;

namespace API.Repository.Interface
{
    public interface IResponseService
    {
        ResponseVM Success(string message, dynamic response = null);
        ResponseVM NoContentFound(string message);
    }
}