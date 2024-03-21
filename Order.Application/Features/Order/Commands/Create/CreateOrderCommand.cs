

using MediatR;

namespace Order.Application.Features.Order.Commands.Create;

public class CreateOrderCommand : IRequest<bool>
{
    public OrderCreateDto Order { get; set; }
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {

        public CreateOrderCommandHandler()
        {
        }


        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            return false;
        }
    }
}
