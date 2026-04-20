using System.Linq.Expressions;
using Application.DTO.Order;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.OrderCQ.Queries.GetOrdersList;

public class GetOrderListQueryHandler(
    IAppDbContext dbContext, IMapper mapper) : IRequestHandler<GetOrderListQuery, IList<OrderListVm>>
{
    public async Task<IList<OrderListVm>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Orders.AsNoTracking();

        if (request.Status.HasValue)
        {
            query = query.Where(o => o.Status == request.Status.Value);
        }
        if (!string.IsNullOrWhiteSpace(request.StartCity))
        {
            query = query.Where(o => o.RoutePoints.OrderBy(rp => rp.OrderIndex).FirstOrDefault().City.Contains(request.StartCity));
        }
        if (!string.IsNullOrWhiteSpace(request.EndCity))
        {
            query = query.Where(o => o.RoutePoints.OrderByDescending(rp => rp.OrderIndex).FirstOrDefault().City.Contains(request.EndCity));
        }
        if (request.MinStartDate.HasValue)
        {
            query = query.Where(o => o.StartDate >= request.MinStartDate.Value);
        }
        if (request.MaxWeight.HasValue)
        {
            query = query.Where(o => o.Payloads.Sum(p => p.Weight) <= request.MaxWeight.Value);
        }
        
        var columnsMap = new Dictionary<string, Expression<Func<Order, object>>>
        {
            ["date"] = o => o.StartDate,
            ["specnumber"] = o => o.SpecNumber,
            ["weight"] = o => o.Payloads.Sum(p => p.Weight),
            ["cost"] = o => Math.Min(
                Math.Min(o.Payment.NotTaxedByCard, o.Payment.TaxedByCard), 
                o.Payment.ByCash)
        };

        var sortByColumn = request.SortBy?.ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(sortByColumn) && columnsMap.ContainsKey(sortByColumn))
        {
            query = request.IsAscending 
                ? query.OrderBy(columnsMap[sortByColumn])
                : query.OrderByDescending(columnsMap[sortByColumn]);
        }
        else
        {
            query = query.OrderByDescending(o => o.Created);
        }
        
        // ProjectTo - это метод AutoMapper, который преобразует IQueryable<Order> в IQueryable<OrderListItemDto>
        // и выполняет запрос на стороне SQL, выбирая только нужные поля.
        var result = await query
            .ProjectTo<OrderListVm>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
            
        return result;
    }
}