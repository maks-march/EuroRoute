using System.Net;
using System.Net.Http.Json;
using Application.CQRS.OrderCQ.Commands.Create;
using Application.CQRS.OrderCQ.Commands.Update;
using Application.DTO.Order;
using ApplicationTest.Common;
using ApplicationTest.Extensions;
using Domain.Enums;
using FluentAssertions;
using WebApi.DTO;

namespace ApplicationTest.IntegreationTests;

[TestFixture]
public class OrderControllerTests : BaseIntegrationTest
{
    private async Task<Guid> PostValidOrder(CreateOrderCommand? command = null)
    {
        var response = await PostOrder(command);
        return await ExtractFromResponse<Guid>(response);
    }

    private async Task<HttpResponseMessage> PostOrder(CreateOrderCommand? createDto = null)
    {
        createDto = createDto ?? new CreateOrderCommand()
        {
            Payloads = [new PayloadCreateCommandDto()],
            RoutePoints = [new RoutePointCreateCommandDto(), new RoutePointCreateCommandDto()]
        };
        
        return await _client.PostAsJsonAsync($"/api/Order/Post", createDto);
    }
    
    private async Task<(OrderDetailsVm order, OrderDetailsVm updated)> PrepareVm(UpdateOrderCommand command)
    {
        var id = await PostValidOrder();
        
        var orderResponse = await _client.GetAsync($"/api/Order/Get/{id}");
        var orderVm = await ExtractFromResponse<OrderDetailsVm>(orderResponse);

        var updateResponse = await _client.PatchAsJsonAsync($"/api/Order/Update/{id}", command);
        var updateId = await ExtractFromResponse<Guid>(updateResponse);
        
        var updatedResponse = await _client.GetAsync($"/api/Order/Get/{updateId}");
        var updatedVm = await ExtractFromResponse<OrderDetailsVm>(updatedResponse);
        
        orderVm.Should().NotBeNull();
        updatedVm.Should().NotBeNull();
        return (orderVm, updatedVm);
    }
    
    [Test]
    public void Post_WithValidCredentials_ShouldBeOk()
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
            Payment = new PaymentCreateCommandDto
            {
                PaymentType = tooBigString,
                TaxedByCard = -1,
                NotTaxedByCard = -1,
                ByCash = -1,
                PaymentAfterDays = -1,
                Prepayment = -1
            },
            Transport = new TransportCreateCommandDto
            {
                BodyType = [tooBigString],
                LoadType = [tooBigString],
                UnloadType = [tooBigString],
                Vehicles = -1,
                TemperatureFrom = -1,
                TemperatureTo = -3,
                Adr = -1
            },
            Payloads = [new PayloadCreateCommandDto
                {
                    Name = tooBigString,
                    Weight = -1,
                    Volume = -1,
                    Amount = -1,
                    Wrap = tooBigString
                }
            ],
            RoutePoints = [
                new RoutePointCreateCommandDto
                {
                    City = tooBigString,
                    Address = tooBigString,
                    LoadTimeStart = TimeSpan.MaxValue,
                    LoadTimeEnd = TimeSpan.MinValue,
                    Date = DateTime.Today.AddDays(-1),
                    IsLoad = false
                }, new RoutePointCreateCommandDto()
            ]
        };
        var response = await PostOrder(command);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        
        error.Error.Should().NotBeEmpty();
        var props = typeof(CreateOrderCommand).GetProperties()
            .Concat(typeof(CreateOrderCommand).GetProperties()).Concat(typeof(PayloadCreateCommandDto).GetProperties())
            .Concat(typeof(CreateOrderCommand).GetProperties()).Concat(typeof(PaymentCreateCommandDto).GetProperties())
            .Concat(typeof(CreateOrderCommand).GetProperties()).Concat(typeof(TransportCreateCommandDto).GetProperties())
            .Concat(typeof(CreateOrderCommand).GetProperties()).Concat(typeof(RoutePointCreateCommandDto).GetProperties());
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
        
        var vm = await ExtractFromResponse<OrderDetailsVm>(response);
        
        vm.Should().NotBeNull();
        vm.CheckFields().Should().BeTrue();
    }

    [Test]
    public async Task Get_WithInvalidCredentials_ShouldBeNotFound()
    {
        var response = await _client.GetAsync($"/api/Order/Get/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
   
    [Test]
    public async Task Update_WithEmptyCredentials_ShouldBeOk()
    {
        var (orderVm, updatedVm) = await PrepareVm(new UpdateOrderCommand());
        orderVm.Updated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        updatedVm.Should().BeEquivalentTo(orderVm, options => 
            options.Excluding(x => x.Updated));
    }
    
    [Test]
    public async Task Update_WithValidCredentials_ShouldBeOk()
    {
        var str = "Update_WithValidCredentials_ShouldBeOk";
        var (orderVm, updatedVm) = await PrepareVm(new UpdateOrderCommand
        {
            StartDate = DateTime.Today.AddDays(1),
            Status = nameof(OrderStatus.NoCargo),
            About = str,
            SpecNumber = 100,
            Payment = new PaymentUpdateCommandDto
            {
                PaymentType = nameof(PaymentType.Negotiable),
                IsTaxedByCard = true,
                IsNotTaxedByCard = true,
                IsByCash = true,
                TaxedByCard = 100,
                NotTaxedByCard = 100,
                ByCash = 100,
                IsVisible = true,
                PaymentAfterDays = 1,
                Prepayment = 10.5,
                IsPrepaymentByFuel = true
            },
            Transport = new TransportUpdateCommandDto
            {
                BodyType = [str],
                LoadType = [],
                UnloadType = [],
                Vehicles = 2,
                TemperatureFrom = -5,
                TemperatureTo = 10,
                IsCrewFull = true,
                Adr = 5,
                IsHitch = true,
                IsPneumaticVehicle = true,
                IsStakes = true,
                IsTir = true,
                IsT1 = true,
                IsCmr = true,
                IsMedicalBook = true
            },
            Payloads = [
                new PayloadUpdateCommandDto
                {
                    Name = str,
                    Weight = 2,
                    Volume = 3,
                    Amount = 4,
                    Wrap = nameof(Wrap.Barrels)
                }
            ],
            RoutePoints = [
                new RoutePointUpdateCommandDto
                {
                    City = str,
                    Address = str,
                    LoadTimeStart = TimeSpan.MinValue,
                    LoadTimeEnd = TimeSpan.MaxValue,
                    Date = DateTime.Today.AddDays(-1),
                    IsLoad = true
                },
                new RoutePointUpdateCommandDto
                {
                    City = str,
                    Address = str,
                    LoadTimeStart = TimeSpan.MinValue,
                    LoadTimeEnd = TimeSpan.MaxValue,
                    Date = DateTime.Today.AddDays(1),
                    IsLoad = false
                },
            ]
        });
        orderVm.Updated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        updatedVm.Should().NotBeEquivalentTo(orderVm);
    }
}