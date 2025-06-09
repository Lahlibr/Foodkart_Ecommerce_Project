namespace Foodkart.Models.Entities.Main
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }
        public ApiResponse(int statusCode, string message)
        : this(statusCode, message, default, null) { }

        public ApiResponse(int statusCode, string message, T data)
            : this(statusCode, message, data, null) { }

        public ApiResponse(int statuscode, string message, T data = default(T), string error = null)
        {
            StatusCode = statuscode;
            Message = message;
            Data = data;
            Error = error;
        }
    }
}
