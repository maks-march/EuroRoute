using System.Net;
using System.Net.Http.Json;
using Application.CQRS.OrderCQ.Commands.Create;
using Application.DTO.Order;
using ApplicationTest.Extensions;
using FluentAssertions;
using WebApi.DTO;

namespace ApplicationTest.IntegreationTests.Order;

[TestFixture]
public class OrderControllerTests : OrderTestMethods
{
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
            Payment = new PaymentCreateCommand
            {
                PaymentType = tooBigString,
                TaxedByCard = -1,
                NotTaxedByCard = -1,
                ByCash = -1,
                PaymentAfterDays = -1,
                Prepayment = -1
            },
            Transport = new TransportCreateCommand
            {
                BodyType = [tooBigString],
                LoadType = [tooBigString],
                UnloadType = [tooBigString],
                Vehicles = -1,
                TemperatureFrom = -1,
                TemperatureTo = -3,
                Adr = -1
            },
            Payloads = [new PayloadCreateCommand
                {
                    Name = tooBigString,
                    Weight = -1,
                    Volume = -1,
                    Amount = -1,
                    Wrap = tooBigString
                }
            ],
            RoutePoints = [
                new RoutePointCreateCommand
                {
                    City = tooBigString,
                    Address = tooBigString,
                    LoadTimeStart = TimeSpan.MaxValue,
                    LoadTimeEnd = TimeSpan.MinValue,
                    Date = DateTime.Today.AddDays(-1),
                    IsLoad = false
                }, new RoutePointCreateCommand()
            ]
        };
        var response = await PostOrder(command);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        
        CheckProps<
            CreateOrderCommand, 
            PayloadCreateCommand, 
            PaymentCreateCommand, 
            TransportCreateCommand, 
            RoutePointCreateCommand>(error).Should().BeTrue();
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
    public async Task Delete_WithValidCredentials_ShouldBeOk()
    {
        var id = await PostValidOrder();
        var response = await _client.DeleteAsync($"/api/Order/Delete/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Delete_WithInvalidCredentials_ShouldBeNotFound()
    {
        var response = await _client.DeleteAsync($"/api/Order/Delete/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}