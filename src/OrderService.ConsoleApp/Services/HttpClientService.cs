using Microsoft.Extensions.Logging;
using OrderService.SharedKernel.Common;
using OrderService.SharedKernel.Extensions;
using System.Net.Http.Json;
using System.Text.Json;

namespace OrderService.ConsoleApp.Helpers;

public interface IHttpClientService
{
    Task<Result<T>> GetAsync<T>(string clientName, string endpoint);
    Task<Result> PatchAsync(string clientName, string endpoint, object data);
    Task<Result<T>> PatchAsync<T>(string clientName, string endpoint, object data);
    Task<Result> PostAsync(string clientName, string endpoint, object data);
    Task<Result<T>> PostAsync<T>(string clientName, string endpoint, object data);
    Task<Result> PutAsync(string clientName, string endpoint, object data);
    Task<Result<T>> PutAsync<T>(string clientName, string endpoint, object data);
}

public class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<HttpClientService> _logger;

    public HttpClientService(IHttpClientFactory clientFactory, ILogger<HttpClientService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<Result<T>> GetAsync<T>(string clientName, string endpoint)
    {
        var client = _clientFactory.CreateClient(clientName);
        var apiUrlLog = $"GET:{client.BaseAddress?.Host}/{endpoint}";
        HttpResponseMessage response;

        try
        {
            response = await client.GetAsync(endpoint);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fail to connect API {apiUrlLog}");
            throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var resultData = GetResult<T>(jsonContent, apiUrlLog);
            if (resultData == null)
            {
                _logger.LogError($"Fail to get data from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Ok(resultData);
        }
        else
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Result.Fail<T>(ErrorCode.ItemNotFound, "Item not found");
            }

            var errorData = GetResult<ErrorObject>(jsonContent, apiUrlLog);
            if (errorData == null)
            {
                _logger.LogError($"Fail to get error from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Fail<T>((ErrorCode)errorData.ErrorCode, errorData.ErrorMessage);
        }
    }

    public async Task<Result> PostAsync(string clientName, string endpoint, object data)
    {
        var client = _clientFactory.CreateClient(clientName);
        var apiUrlLog = $"POST:{client.BaseAddress?.Host}/{endpoint}";
        HttpResponseMessage response;

        try
        {
            response = await client.PostAsJsonAsync(endpoint, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fail to connect API {apiUrlLog}");
            throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return Result.Ok();
        }
        else
        {
            var errorData = GetResult<ErrorObject>(jsonContent, apiUrlLog);
            if (errorData == null)
            {
                _logger.LogError($"Fail to get error from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Fail((ErrorCode)errorData.ErrorCode, errorData.ErrorMessage);
        }
    }

    public async Task<Result<T>> PostAsync<T>(string clientName, string endpoint, object data)
    {
        var client = _clientFactory.CreateClient(clientName);
        var apiUrlLog = $"POST:{client.BaseAddress?.Host}/{endpoint}";
        HttpResponseMessage response;

        try
        {
            response = await client.PostAsJsonAsync(endpoint, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fail to connect API {apiUrlLog}");
            throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var resultData = GetResult<T>(jsonContent, apiUrlLog);
            if (resultData == null)
            {
                _logger.LogError($"Fail to get data from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Ok(resultData);
        }
        else
        {
            var errorData = GetResult<ErrorObject>(jsonContent, apiUrlLog);
            if (errorData == null)
            {
                _logger.LogError($"Fail to get error from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Fail<T>((ErrorCode)errorData.ErrorCode, errorData.ErrorMessage);
        }
    }

    public async Task<Result> PutAsync(string clientName, string endpoint, object data)
    {
        var client = _clientFactory.CreateClient(clientName);
        var apiUrlLog = $"PUT:{client.BaseAddress?.Host}/{endpoint}";
        HttpResponseMessage response;

        try
        {
            response = await client.PutAsJsonAsync(endpoint, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fail to connect API {apiUrlLog}");
            throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return Result.Ok();
        }
        else
        {
            var errorData = GetResult<ErrorObject>(jsonContent, apiUrlLog);
            if (errorData == null)
            {
                _logger.LogError($"Fail to get error from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Fail((ErrorCode)errorData.ErrorCode, errorData.ErrorMessage);
        }
    }

    public async Task<Result<T>> PutAsync<T>(string clientName, string endpoint, object data)
    {
        var client = _clientFactory.CreateClient(clientName);
        var apiUrlLog = $"PUT:{client.BaseAddress?.Host}/{endpoint}";
        HttpResponseMessage response;

        try
        {
            response = await client.PutAsJsonAsync(endpoint, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fail to connect API {apiUrlLog}");
            throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var resultData = GetResult<T>(jsonContent, apiUrlLog);
            if (resultData == null)
            {
                _logger.LogError($"Fail to get data from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Ok(resultData);
        }
        else
        {
            var errorData = GetResult<ErrorObject>(jsonContent, apiUrlLog);
            if (errorData == null)
            {
                _logger.LogError($"Fail to get error from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Fail<T>((ErrorCode)errorData.ErrorCode, errorData.ErrorMessage);
        }
    }

    public async Task<Result> PatchAsync(string clientName, string endpoint, object data)
    {
        var client = _clientFactory.CreateClient(clientName);
        var apiUrlLog = $"PATCH:{client.BaseAddress?.Host}/{endpoint}";
        HttpResponseMessage response;

        try
        {
            response = await client.PatchAsJsonAsync(endpoint, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fail to connect API {apiUrlLog}");
            throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return Result.Ok();
        }
        else
        {
            var errorData = GetResult<ErrorObject>(jsonContent, apiUrlLog);
            if (errorData == null)
            {
                _logger.LogError($"Fail to get error from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Fail((ErrorCode)errorData.ErrorCode, errorData.ErrorMessage);
        }
    }

    public async Task<Result<T>> PatchAsync<T>(string clientName, string endpoint, object data)
    {
        var client = _clientFactory.CreateClient(clientName);
        var apiUrlLog = $"PATCH:{client.BaseAddress?.Host}/{endpoint}";
        HttpResponseMessage response;

        try
        {
            response = await client.PatchAsJsonAsync(endpoint, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fail to connect API {apiUrlLog}");
            throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
        }

        var jsonContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var resultData = GetResult<T>(jsonContent, apiUrlLog);
            if (resultData == null)
            {
                _logger.LogError($"Fail to get data from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Ok(resultData);
        }
        else
        {
            var errorData = GetResult<ErrorObject>(jsonContent, apiUrlLog);
            if (errorData == null)
            {
                _logger.LogError($"Fail to get error from API {apiUrlLog}");
                throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
            }

            return Result.Fail<T>((ErrorCode)errorData.ErrorCode, errorData.ErrorMessage);
        }
    }

    private T? GetResult<T>(string jsonContent, string apiUrl)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(jsonContent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fail to deserialize api response body of API {apiUrl}");
            throw new Exception(ErrorCode.ApiConnectionError.GetDescription());
        }
    }
}