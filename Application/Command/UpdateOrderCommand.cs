using Application.Common;
using Infrastructure.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Command
{
    public record UpdateOrderCommand() : IRequest<Result<UpdateOrderResponseDTO>>;
}
