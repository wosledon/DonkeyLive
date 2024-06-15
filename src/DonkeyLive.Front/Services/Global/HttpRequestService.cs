using DonkeyLive.Shared.Extensions;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace DonkeyLive.Front.Services.Global;

public class HttpRequestService
{
    private readonly HttpClient _httpClient;

    public string? GetBaseAddress()
    {
        return _httpClient.BaseAddress?.ToString();
    }

    public HttpRequestService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Donkey");
    }


    private string BuildUri(string uri, object? obj)
    {
        if (obj.IsNull()) return uri;

        var properties = obj.GetType().GetProperties();

        var queryString = string.Join("&", properties.Select(p => $"{p.Name}={p.GetValue(obj)}"));

        uri += $"?{queryString}";

        return uri;
    }

    public StringContent? BuildBody(object? obj)
    {
        if (obj.IsNull())
        {
            return default;
        }

        return new StringContent(obj.ToJson(), System.Text.Encoding.UTF8, "application/json");
    }

    public async Task<T?> GetAsync<T>(string uri, object? @params = null)
    {
        var response = await _httpClient.GetAsync(BuildUri(uri, @params));

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        return content.ToObject<T>();
    }

    public async Task<T?> PostAsync<T>(string uri, object? @params = null, object? body = null)
    {
        var response = await _httpClient.PostAsync(BuildUri(uri, @params), BuildBody(body));

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        return content.ToObject<T>();
    }

    public async Task<T?> PutAsync<T>(string uri, object? @params = null, object? body = null)
    {
        var response = await _httpClient.PutAsync(BuildUri(uri, @params), BuildBody(body));

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        return content.ToObject<T>();
    }

    public async Task<T?> DeleteAsync<T>(string uri, object? @params = null)
    {
        var response = await _httpClient.DeleteAsync(BuildUri(uri, @params));

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        return content.ToObject<T>();
    }

    public async Task DownloadAsync(string uri, string filePath, object? @params = null)
    {
        var response = await _httpClient.GetAsync(BuildUri(uri, @params));

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();

        await using var fileStream = new FileStream(filePath, FileMode.Create);

        await stream.CopyToAsync(fileStream);
    }
}
