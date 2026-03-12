using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands
{
    public class LoginCommand:IRequest<AuthResponseDto>
    {
        public LoginDto LoginDto { get; set; } = null!;
    }
}
