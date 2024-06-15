using BlazorComponent;
using DonkeyLive.Shared.Helpers;
using Masa.Blazor;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DonkeyLive.Front.Services.Global;

public class AlertService
{
    private readonly IPopupService _popupService;

    public AlertService(IPopupService popupService)
    {
        _popupService = popupService;
    }

    public async Task DefaultAsync(string message)
    {
        await _popupService.EnqueueSnackbarAsync(message, AlertTypes.None, false, 2000);
    }

    public async Task InfoAsync(string message)
    {
        await _popupService.EnqueueSnackbarAsync(message, AlertTypes.Info, false, 2000);
    }

    public async Task SuccessAsync(string message)
    {
        await _popupService.EnqueueSnackbarAsync(message, AlertTypes.Success, false, 2000);
    }

    public async Task WarningAsync(string message)
    {
        await _popupService.EnqueueSnackbarAsync(message, AlertTypes.Warning, false, 2000);
    }

    public async Task ErrorAsync(string message)
    {
        await _popupService.EnqueueSnackbarAsync(message, AlertTypes.Error, false, 2000);
    }

    public async Task ConfirmAsync(string message, Func<Task> onConfirm)
    {
        var result = await _popupService.ConfirmAsync(message, "确定", "取消");

        if (result)
        {
            await onConfirm();
        }
    }

    public async Task<TResult> ProcessResultAsync<TResult>(ApiResponse<TResult>? result, [NotNull] TResult defaultValue, bool alert = true)
    {
        Debug.Assert(defaultValue != null, $"defaultValue != null");

        if (result == null)
        {
            await ErrorAsync("获取数据失败");
            return defaultValue;
        }

        if (result.Data is IPagedList list)
        {
            list.TotalCount = result.TotalCount;
        }

        if (alert)
        {
            if (result.Success)
            {
                await SuccessAsync(result.Message);
            }
            else
            {
                await ErrorAsync(result.Message);
            }
        }

        return result.Data ?? defaultValue;
    }
}
