using Application.Common.Mappings;

namespace Application.DTO.Order;

public record OrderDetailsVm : IMapWith<Domain.Models.Order.Order>
{
    
}