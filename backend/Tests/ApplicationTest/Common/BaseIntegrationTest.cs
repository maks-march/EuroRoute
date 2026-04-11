using System.Net.Http.Json;
using System.Text.Json;

namespace ApplicationTest.Common;

[TestFixture] // Говорим NUnit, что это класс с тестами
public abstract class BaseIntegrationTest
{
    private TestWebApplicationFactory<Program> _factory = null!;
    protected HttpClient _client = null!;
    protected JsonSerializerOptions _jsonOptions = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new TestWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    
    protected async Task<TResponse?> PostAsJsonAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var response = await _client.PostAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(content)) return default;
        
        return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}