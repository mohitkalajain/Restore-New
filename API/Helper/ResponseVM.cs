namespace API.Helper
{
    public class ResponseVM
    {
        public int StatusCode { get; set; }
        public bool Flag { get; set; }
        public string Message { get; set; }
        public dynamic Response { get; set; }

        public ResponseVM(bool flag, int statusCode, string message, dynamic response)
        {
            this.Flag = flag;
            this.StatusCode = statusCode;
            this.Message = message;
            this.Response = response;
        }
     
        public static ResponseVM InvalidRequest(string errorMessage)
        {
            return new ResponseVM(true, StatusCodes.Status400BadRequest, errorMessage, null);
        }
    }
}