using Domain.Enums;

namespace Domain.Models.Order;

public class Payment : OrderField
{
    public required PaymentType PaymentType { get; set; } = PaymentType.NoNegotiable;
    public required bool IsTaxedByCard {get; set;}
    public required bool IsNotTaxedByCard {get; set;}
    public required bool IsByCash {get; set;}
    
    public double TaxedByCard {get; set;}
    public double NotTaxedByCard { get; set; }
    public double ByCash {get; set;}
    
    public bool IsVisible { get; set; } = false;
    public int PaymentAfterDays { get; set; } = 0;
    public double Prepayment {get; set;}
    public bool IsPrepaymentByFuel {get; set;}
}