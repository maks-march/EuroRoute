using System.Net;
using System.Net.Http.Json;
using Application.CQRS.UserCQ.Commands.Update;
using Application.DTO.User;
using ApplicationTest.Common;
using FluentAssertions;
using WebApi.DTO;

namespace ApplicationTest.IntegrationTests;

[TestFixture]
public class UserControllerTests : BaseIntegrationTest
{
    private const string BaseUrl = "api/User/";
    private async Task<UserDetailsVm> CheckGet_User(Guid id)
    {
        var response = await _client.GetAsync($"{BaseUrl}{id}");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        var vm = await response.Content.ReadFromJsonAsync<UserDetailsVm>();
        
        vm.Should().NotBeNull();
        return vm;
    }
    
    [Test]
    public async Task Get_WithValidCredentials_ShouldBeOk()
    {
        var vm = await CheckGet_User(Tokens.UserId);
        vm.Name.Should().Be(Name);
        vm.Surname.Should().Be(Surname);
    }
    
    
    [Test]
    public async Task GetMe_WithValidCredentials_ShouldBeOk()
    {
        var response = await _client.GetAsync($"{BaseUrl}me");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        var vm = await response.Content.ReadFromJsonAsync<UserDetailsVm>();
        
        vm.Should().NotBeNull();
        vm.Name.Should().Be(Name);
        vm.Surname.Should().Be(Surname);
    }
    
    [Test]
    public async Task Get_WithInvalidCredentials_ShouldNotBeFound()
    {
        var response = await _client.GetAsync($"{BaseUrl}{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var vm = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        
        vm.Should().NotBeNull();
        vm.Error.Should().NotBeNullOrEmpty();
        vm.Details.Should().NotBeNullOrEmpty();
    }
    
    [Test]
    public async Task GetList_WithValidCredentials_ShouldBeOk()
    {
        var response = await _client.GetAsync(BaseUrl);
        
        response.IsSuccessStatusCode.Should().BeTrue();
        var vm = await response.Content.ReadFromJsonAsync<UserListVm>();
        
        vm.Should().NotBeNull();
        vm.Users.Should().NotBeEmpty();
        var first = vm.Users.First(user => user.Id == Tokens.UserId);
        first.Name.Should().Be(Name);
        first.Surname.Should().Be(Surname);
    }

    [Test]
    public async Task Update_WithValidCredentials_ShouldBeOk()
    {
        var request = new UpdateUserCommand()
        {
            Name = "NewName",
            Surname = "NewSurname"
        };
        var response = await _client.PatchAsJsonAsync($"{BaseUrl}me", request);
        
        response.IsSuccessStatusCode.Should().BeTrue();
        var id = await response.Content.ReadFromJsonAsync<Guid>();

        var getResponse = await CheckGet_User(id);
        
        getResponse.Name.Should().Be(request.Name);
        getResponse.Surname.Should().Be(request.Surname);
        getResponse.Updated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
    
    [Test]
    public async Task Update_WithInvalidCredentials_ShouldBeBadRequest()
    {
        var request = new UpdateUserCommand()
        {
            Name = string.Concat(
                Enumerable.Repeat("Update_WithInvalidCredentials_ShouldBeBadRequest", 20)
            ),
            Surname = string.Concat(
                Enumerable.Repeat("Update_WithInvalidCredentials_ShouldBeBadRequest", 20)
            )
        };
        var response = await _client.PatchAsJsonAsync($"{BaseUrl}me", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var vm = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        
        vm.Should().NotBeNull();
        vm.Error.Should().ContainAll(["Name", "Surname"], "All fields not valid.");
        vm.Details.Should().NotBeNullOrEmpty();
    }
}