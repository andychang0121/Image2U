namespace Image2U.Web.Models
{
    public class ResponseResult
    {
        public bool IsOk { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }

    public class ResponseResult<T>
    {
        public bool IsOk { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }
    }
}