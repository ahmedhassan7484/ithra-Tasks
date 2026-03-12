using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
       
        private readonly IUnitOfWork _unitOfWork;
        public LoginCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(request.LoginDto.Email);

                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        IsSuccess = false,
                        Message = "Invalid email or password"
                    };
                }

               
                if (!user.IsActive)
                {
                    return new AuthResponseDto
                    {
                        IsSuccess = false,
                        Message = "Account is deactivated"
                    };
                }

              
                var isPasswordValid = await _unitOfWork.Users.CheckPasswordAsync(user, request.LoginDto.Password);

                if (!isPasswordValid)
                {
                    return new AuthResponseDto
                    {
                        IsSuccess = false,
                        Message = "Invalid email or password"
                    };
                }

                return new AuthResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = "User",
                    IsSuccess = true,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }
    }
    
}
