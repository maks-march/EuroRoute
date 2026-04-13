using System.Net;
using System.Net.Http.Json;
using Application.CQRS.OrderCQ.Commands.Create;
using Application.DTO.Order;
using ApplicationTest.Common;
using ApplicationTest.Extensions;
using FluentAssertions;
using WebApi.DTO;

namespace ApplicationTest.IntegreationTests;

[TestFixture]
public class OrderControllerTests : BaseIntegrationTest
{
    private async Task<Guid> PostValidOrder(CreateOrderCommand? command = null)
    {
        var response = await PostOrder(command);
        
        response.IsSuccessStatusCode.Should().BeTrue();
        var id = await response.Content.ReadFromJsonAsync<Guid>();
        return id;
    }

    private async Task<HttpResponseMessage> PostOrder(CreateOrderCommand? createDto = null)
    {
        createDto = createDto ?? new CreateOrderCommand()
        {
            Payloads = [new PayloadCommandDto()],
            RoutePoints = [new RoutePointCommandDto(), new RoutePointCommandDto()]
        };
        
        return await _client.PostAsJsonAsync($"/api/Order/Post", createDto);
    }
    
    [Test]
    public async Task Post_WithValidCredentials_ShouldBeOk()
    {
        PostValidOrder().Should().NotBe(Guid.Empty);
    }
    
    [Test]
    public async Task Post_WithInvalidCredentials_ShouldBeBadRequest()
    {
        var tooBigString = string.Concat(Enumerable.Repeat("Post_WithInvalidCredentials_ShouldBeBadRequest", 20));
        var command = new CreateOrderCommand
        {
            UserId = Guid.Empty,
            StartDate = DateTime.Today.AddDays(-1),
            Status = tooBigString,
            About = tooBigString,
            SpecNumber = -1,
            Payment = new PaymentCommandDto
            {
                PaymentType = tooBigString,
                TaxedByCard = -1,
                NotTaxedByCard = -1,
                ByCash = -1,
                PaymentAfterDays = -1,
                Prepayment = -1
            },
            Transport = new TransportCommandDto
            {
                BodyType = [tooBigString],
                LoadType = [tooBigString],
                UnloadType = [tooBigString],
                Vehicles = -1,
                TemperatureFrom = -1,
                TemperatureTo = -3,
                Adr = -1
            },
            Payloads = [new PayloadCommandDto
                {
                    Name = tooBigString,
                    Weight = -1,
                    Volume = -1,
                    Amount = -1,
                    Wrap = tooBigString
                }
            ],
            RoutePoints = [
                new RoutePointCommandDto
                {
                    City = tooBigString,
                    Address = tooBigString,
                    LoadTimeStart = TimeSpan.MaxValue,
                    LoadTimeEnd = TimeSpan.MinValue,
                    Date = DateTime.Today.AddDays(-1),
                    IsLoad = false
                }, new RoutePointCommandDto()
            ]
        };
        var response = await PostOrder(command);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        
        error.Error.Should().NotBeEmpty();
        var props = typeof(CreateOrderCommand).GetProperties()
            .Concat(typeof(CreateOrderCommand).GetProperties()).Concat(typeof(PayloadCommandDto).GetProperties())
            .Concat(typeof(CreateOrderCommand).GetProperties()).Concat(typeof(PaymentCommandDto).GetProperties())
            .Concat(typeof(CreateOrderCommand).GetProperties()).Concat(typeof(TransportCommandDto).GetProperties())
            .Concat(typeof(CreateOrderCommand).GetProperties()).Concat(typeof(RoutePointCommandDto).GetProperties());
        props
            .Where(p => p.PropertyType != typeof(Guid) && p.PropertyType != typeof(bool))
            .Select(p => p.Name)
            .Where(name => !name.Contains("Temperature") && !name.Contains("LoadTime"))
            .All(name =>
            {
                Console.WriteLine(name);
                return error.Error.Contains(name);
            })
            .Should()
            .BeTrue();
    }

    [Test]
    public async Task Get_WithValidCredentials_ShouldBeOk()
    {
        var id = await PostValidOrder();
        var response = await _client.GetAsync($"/api/Order/Get/{id}");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        var vm = await response.Content.ReadFromJsonAsync<OrderDetailsVm>();
        
        vm.Should().NotBeNull();
        vm.CheckFields().Should().BeTrue();
    }
}