using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Application.CQRS.AuthCQ.Refresh;
using Application.CQRS.AuthCQ.Register;
using Application.DTO.Auth;
using FluentAssertions;

namespace ApplicationTest.Common;

[TestFixture]
public abstract class BaseIntegrationTest
{
    private TestWebApplicationFactory<Program> _factory = null!;
    protected HttpClient _client = null!;
    protected JsonSerializerOptions _jsonOptions = null!;
    protected const string Name = "Test";
    protected const string Surname = "User";
    protected const string Password = "Password123!";
    protected string Login = "RequestAuthor";
    protected AuthResponse Tokens;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new TestWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        Tokens = Register(Login).Result!;
        Tokens.Should().NotBeNull();
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", Tokens.AccessToken);
    }
    
    protected async Task<AuthResponse?> Register(string login)
    {
        var command = new RegisterCommand
        {
            Name = Name,
            Surname = Surname,
            Login = login,
            Password = Password
        };
        var response = await _client.PostAsJsonAsync("/api/Auth/register", command);
        
        response.IsSuccessStatusCode.Should().BeTrue();
        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }
    
    protected async Task<AuthResponse?> Refresh(AuthResponse? authResponse)
    {
        var command = new RefreshCommand
        {
            AccessToken = authResponse!.AccessToken,
            RefreshToken = authResponse.RefreshToken,
        };
        var response = await _client.PostAsJsonAsync("/api/Auth/refresh", command);
        
        response.IsSuccessStatusCode.Should().BeTrue();
        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }
    
    protected static async Task<T?> ExtractFromResponse<T>(HttpResponseMessage response)
    {
        response.IsSuccessStatusCode.Should().BeTrue();
        var result = await response.Content.ReadFromJsonAsync<T>();
        return result;
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}