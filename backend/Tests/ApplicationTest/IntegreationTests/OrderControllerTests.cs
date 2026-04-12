using System.Net.Http.Json;
using Application.CQRS.OrderCQ.Commands.Create;
using Application.DTO.Order;
using ApplicationTest.Common;
using ApplicationTest.Extensions;
using FluentAssertions;

namespace ApplicationTest.IntegreationTests;

[TestFixture]
public class OrderControllerTests : BaseIntegrationTest
{
    private async Task<Guid> PostOrder()
    {
        var createDto = new CreateOrderCommand()
        {
            Payloads = [new PayloadCommandDto()],
            RoutePoints = [new RoutePointCommandDto(), new RoutePointCommandDto()]
        };
        
        var response = await _client.PostAsJsonAsync($"/api/Order/Post", createDto);
        
        response.IsSuccessStatusCode.Should().BeTrue();
        var id = await response.Content.ReadFromJsonAsync<Guid>();
        return id;
    }
    
    [Test]
    public async Task Post_WithValidCredentials_ShouldBeOk()
    {
        PostOrder().Should().NotBe(Guid.Empty);
    }

    [Test]
    public async Task Get_WithValidCredentials_ShouldBeOk()
    {
        var id = await PostOrder();
        var response = await _client.GetAsync($"/api/Order/Get/{id}");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        var vm = await response.Content.ReadFromJsonAsync<OrderDetailsVm>();
        
        vm.Should().NotBeNull();
        vm.CheckFields().Should().BeTrue();
    }
}