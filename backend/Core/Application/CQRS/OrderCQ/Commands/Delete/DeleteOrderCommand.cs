using MediatR;

namespace Application.CQRS.OrderCQ.Commands.Delete;

public record DeleteOrderCommand(Guid Id, Guid UserId) : IRequest;