using DonkeyLive.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DonkeyLive.WebApi.Controllers.Base;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class ApiControllerBase : ControllerBase
{
    [NonAction]
    public IActionResult Success(string message, object? data = null)
    {
        return Result(ApiResponse.HttpCode.Ok, message, data);
    }

    [NonAction]
    public IActionResult Success(object? data = null)
    {
        return Result(ApiResponse.HttpCode.Ok, "", data);
    }

    [NonAction]
    public IActionResult Error(string message, object? data = null)
    {
        return Result(ApiResponse.HttpCode.InternalServerError, message, data);
    }


    [NonAction]
    public IActionResult Error(string message)
    {
        return Result(ApiResponse.HttpCode.InternalServerError, message);
    }

    [NonAction]
    public IActionResult Error()
    {
        return Result(ApiResponse.HttpCode.InternalServerError);
    }

    [NonAction]
    public IActionResult Result(bool success, string message = "", object? data = null)
    {
        return Result(success ? ApiResponse.HttpCode.Ok : ApiResponse.HttpCode.InternalServerError, message, data);
    }

    private IActionResult Result(ApiResponse.HttpCode code, string message = "", object? data = null)
    {
        return Ok(new ApiResponse(code, message, data));
    }
}
