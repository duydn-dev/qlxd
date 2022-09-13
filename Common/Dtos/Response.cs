using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos
{
    public class Response<T>
    {
        public T ResponseData { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object OtherData { get; set; }
        public Response(bool success, int statusCode, string message, T data, object otherData = null)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
            ResponseData = data;
            OtherData = otherData;
        }
        public static Response<T> CreateSuccessResponse()
        {
            return new Response<T>(true, 200, "success!", default(T));
        }
        public static Response<T> CreateSuccessResponse(T data)
        {
            return new Response<T>(true, 200, "success!", data);
        }
        public static Response<T> CreateSuccessResponse(T data, object otherData)
        {
            return new Response<T>(true, 200, "success!", data, otherData);
        }
        public static Response<T> CreateErrorResponse(Exception exception = null)
        {
            return new Response<T>(false, 500, exception.ToString(), default(T));
        }
        public static Response<T> CreateErrorResponse(string message = null)
        {
            return new Response<T>(false, 500, message, default(T));
        }
        public static Response<T> CreateErrorResponse()
        {
            return new Response<T>(false, 500, "có lỗi xảy ra, vui lòng liên hệ quản trị viên để được hỗ trợ", default(T));
        }
    }
    public class GetListResponseModel<T>
    {
        public T Data { get; set; }
        public int TotalData { get; set; }
        public int PageCount { get; set; }
        public GetListResponseModel(int totalData, int pageSize)
        {
            TotalData = totalData;
            PageCount = (int)Math.Ceiling(Convert.ToDecimal(totalData) / Convert.ToDecimal(pageSize));
        }
    }
}
