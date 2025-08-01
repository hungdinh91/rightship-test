using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderService.ConsoleApp.Helpers;

public class HttpClientService
{
    private readonly IHttpClientFactory _clientFactory;

    public HttpClientService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<string> GetAsync(string clientName, string endpoint)
    {
        var client = _clientFactory.CreateClient(clientName);
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PostAsync(string clientName, string endpoint, object data)
    {
        var client = _clientFactory.CreateClient(clientName);
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}