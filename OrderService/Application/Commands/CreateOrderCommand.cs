using MediatR;

namespace OrderService.Application.Commands;

public record CreateOrderCommand(Guid CustomerId, decimal Amount, List<CreateOrderItemDto> Items) : IRequest<Guid>;

public record CreateOrderItemDto(Guid ProductId, int Quantity, decimal UnitPrice);
