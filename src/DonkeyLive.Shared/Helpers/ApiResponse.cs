using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyLive.Shared.Helpers;

public class ApiResponse<TData>: ApiResponse
{
    public new TData? Data { get; set; }
}

public class ApiResponse
{
    public enum HttpCode
    {
        NotFound = 404,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        InternalServerError = 500,
        Ok = 200
    }

    public HttpCode Code { get; set; }
    public bool Success => Code == HttpCode.Ok;
    private string? _message;
    public string Message 
    { 
        get
        {
            if(string.IsNullOrWhiteSpace(_message))
            {
                return Code switch
                {
                    HttpCode.Ok => "请求成功",
                    HttpCode.BadRequest => "请求参数异常",
                    HttpCode.Unauthorized => "权限验证失败",
                    HttpCode.Forbidden => "访问被拒绝",
                    HttpCode.InternalServerError => "服务器异常",
                    HttpCode.NotFound => "找不到资源",
                    _ => "未知错误"
                };
            }

            return _message;
        } 
        set => _message = value; 
    }
    public object? Data { get; set; }

    private int _totalCount;
    public int TotalCount 
    { 
        get 
        { 
            if(Data is IPagedList list)
            {
                return list.TotalCount;
            }
            return _totalCount;
        }
        set => _totalCount = value; 
    }

    public ApiResponse(HttpCode code, string message, object? data = null)
    {
        Code = code;
        Message = message;
        Data = data;
    }

    public ApiResponse(HttpCode code, object? data = null)
    {
        Code = code;
        Data = data;
    }

    public ApiResponse(HttpCode code, string message)
    {
        Code = code;
        Message = message;
    }

    public ApiResponse()
    {
    }
}
