namespace API.Helper
{
    public class ResponseVM
    {
        public int StatusCode { get; set; }
        public bool Flag { get; set; }
        public string Message { get; set; }
        public dynamic Response { get; set; }
        public string TraceId { get; set; }

        public ResponseVM(bool flag, int statusCode, string message, dynamic response,string traceId)
        {
            this.Flag = flag;
            this.StatusCode = statusCode;
            this.Message = message;
            this.Response = response;
            this.TraceId = traceId;
        }
     
        public static ResponseVM InvalidRequest(string errorMessage, string traceId)
        {
            return new ResponseVM(true, StatusCodes.Status400BadRequest, errorMessage, new { },traceId);
        }
        public static ResponseVM NoContentFound(string message, string traceId)
        {
            return new ResponseVM(true, StatusCodes.Status204NoContent, message, new { }, traceId);
        }
    }
}