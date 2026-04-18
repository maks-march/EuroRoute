namespace Domain.Models.Order;

public class Transport : OrderField
{
    public required IList<string> BodyType {get; set;}
    public IList<string> LoadType {get; set;} = [];
    public IList<string> UnloadType {get; set;} = [];
    
    public int Vehicles { get; set; } = 1;
    public int? TemperatureFrom { get; set; } = null;
    public int? TemperatureTo { get; set; } = null;
    public bool IsCrewFull { get; set; } = false;
    public int Adr { get; set; } = 1;
    
    public bool IsHitch { get; set; } = false;
    public bool IsPneumaticVehicle { get; set; } = false;
    public bool IsStakes { get; set; } = false;
    
    public bool IsTir { get; set; } = false;
    public bool IsT1 { get; set; } = false;
    public bool IsCmr { get; set; } = false;
    public bool IsMedicalBook { get; set; } = false;
}