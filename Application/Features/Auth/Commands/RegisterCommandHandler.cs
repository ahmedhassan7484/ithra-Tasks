using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
               var IsEmailExists=await _unitOfWork.Users.GetByEmailAsync(request.RegisterDto.Email);
                if (IsEmailExists!=null)
                {
                    return new AuthResponseDto
                    {
                        IsSuccess = false,
                        Message = "Email already exists"
                    };
                }

               var IsUsernameExists=await _unitOfWork.Users.GetByUsernameAsync(request.RegisterDto.Username);
                if (IsUsernameExists!= null)
                {
                    return new AuthResponseDto
                    {
                        IsSuccess = false,
                        Message = "Username already exists"
                    };
                }

               
                var user = new ApplicationUser
                {
                    Username = request.RegisterDto.Username,
                    Email = request.RegisterDto.Email,
                    PasswordHash = request.RegisterDto.Password,
                    Role = "User",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdUser = await _unitOfWork.Users.CreateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return new AuthResponseDto
                {
                    Id = createdUser.Id,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    Role = createdUser.Role,
                    IsSuccess = true,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = $"Registration failed: {ex.Message}"
                };
            }
        }
    }
}
