namespace MoviesAPI.Application.DTOs.Common
{
    /// <summary>
    /// Base response wrapper for all API responses
    /// </summary>
    public class BaseResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }

        public BaseResponse()
        {
            Errors = new List<string>();
        }

        public BaseResponse(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            Errors = new List<string>();
        }

        public BaseResponse(string message)
        {
            Succeeded = false;
            Message = message;
            Errors = new List<string> { message };
        }

        public static BaseResponse<T> Success(T data, string message = "Operation completed successfully")
        {
            return new BaseResponse<T>(data, message);
        }

        public static BaseResponse<T> Failure(string error)
        {
            return new BaseResponse<T>
            {
                Succeeded = false,
                Message = "Operation failed",
                Errors = new List<string> { error }
            };
        }

        public static BaseResponse<T> Failure(List<string> errors)
        {
            return new BaseResponse<T>
            {
                Succeeded = false,
                Message = "Operation failed",
                Errors = errors
            };
        }
    }

    /// <summary>
    /// Paged response wrapper for list endpoints
    /// </summary>
    public class PagedResponse<T> : BaseResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            Data = data;
            Succeeded = true;
            Errors = new List<string>();
        }
    }
}
