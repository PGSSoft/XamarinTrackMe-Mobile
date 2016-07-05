using Newtonsoft.Json;

namespace TrackMe.Core.Models
{
    public class WebResult
    {
        private WebResult()
        {
        }

        public string ErrorMessage { get; private set; }
        public bool IsError { get; private set; }

        public static WebResult Error(string message)
        {
            return new WebResult()
            {
                IsError = true,
                ErrorMessage = message
            };
        }

        public static WebResult Success()
        {
            return new WebResult();
        }
    }

    public class WebResult<T>
    {
        public WebResult()
        {
        }

        [JsonConstructor]
        public WebResult(T data, bool isError, string errorMessage)
        {
            Result = data;
            IsError = isError;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; }
        public bool IsError { get; set; }
        public T Result { get; set; }

        public static WebResult<T> Success(T data)
        {
            return new WebResult<T>()
            {
                Result = data
            };
        }

        public static WebResult<T> Error(string message)
        {
            return new WebResult<T>()
            {
                IsError = true,
                ErrorMessage = message
            };
        }
    }
}