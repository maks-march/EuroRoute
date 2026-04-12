using Application.DTO.Order;
using Domain.Enums;
using FluentAssertions;

namespace ApplicationTest.Extensions;

public static class OrderDetailsVmExtensions
{
    public static bool CheckFields(this OrderDetailsVm vm)
    {
        vm.Id.Should().NotBeEmpty();
        vm.Created.Should().BeBefore(DateTime.UtcNow);
        vm.Status.Should().NotBeEmpty();
        vm.StartDate.Should().BeAfter(DateTime.UtcNow);
        
        vm.Payment.Should().NotBeNull();
        
        vm.Payment.PaymentType.Should().NotBeEmpty();
        if (vm.Payment.PaymentType != nameof(PaymentType.Request))
        {
            if (vm.Payment.IsByCash)
                vm.Payment.ByCash.Should().NotBe(0);
            if (vm.Payment.IsTaxedByCard)
                vm.Payment.TaxedByCard.Should().NotBe(0);
            if (vm.Payment.IsNotTaxedByCard)
                vm.Payment.NotTaxedByCard.Should().NotBe(0);
        }
        vm.Payment.Prepayment.Should().BeInRange(0, 100);
        
        vm.Transport.Should().NotBeNull();
        vm.Transport.BodyType.Should().NotBeEmpty();
        vm.Transport.Vehicles.Should().BeGreaterThan(0);
        vm.Transport.Adr.Should().BeInRange(1, 9);
        if (vm.Transport is { TemperatureFrom: not null, TemperatureTo: not null })
            vm.Transport.TemperatureFrom.Should().BeLessThanOrEqualTo((int)vm.Transport.TemperatureTo);
        
        vm.Payloads.Should().NotBeEmpty();
        vm.Payloads.Count.Should().BeGreaterThan(0);
        foreach (var payload in vm.Payloads)
        {
            payload.Should().NotBeNull();
            payload.Name.Should().NotBeEmpty();
            payload.Volume.Should().BeGreaterThan(0);
            payload.Weight.Should().BeGreaterThan(0);
            payload.Amount.Should().BeGreaterThan(0);
        }
        
        vm.RoutePoints.Should().NotBeEmpty();
        vm.RoutePoints.Count.Should().BeGreaterThan(1);
        
        foreach (var route in vm.RoutePoints)
        {
            route.Should().NotBeNull();
            route.City.Should().NotBeEmpty();
            route.Address.Should().NotBeEmpty();
            route.LoadTimeStart.Should().BeLessThanOrEqualTo(route.LoadTimeStart);
            route.Date.Should().BeAfter(DateTime.UtcNow);
        }
        vm.RoutePoints.First().IsLoad.Should().BeTrue();
        vm.RoutePoints.Last().IsLoad.Should().BeFalse();

        return true;
    }
}