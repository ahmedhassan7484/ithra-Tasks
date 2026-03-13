using Application.DTOs;
using Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ithra_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public AuthController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }
        /// <summary>
        /// Registers a new user and returns an authentication response containing a JWT token.
        /// </summary>
        /// <remarks>This method processes the registration request asynchronously and issues a JWT token
        /// upon successful registration. Ensure that the provided registration data satisfies all validation criteria
        /// before calling this method.</remarks>
        /// <param name="registerDto">An object containing the user's registration information, such as username, password, and any additional
        /// required fields. Must meet all validation requirements for successful registration.</param>
        /// <returns>An ActionResult containing an AuthResponseDto with the authentication token if registration is successful;
        /// otherwise, a BadRequest result with details about the failure.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            var command = new RegisterCommand { RegisterDto = registerDto };
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(response);

           
            response.Token = GenerateJwtToken(response);

            return Ok(response);
        }
        /// <summary>
        /// Authenticates a user based on the provided login credentials and returns an authentication response
        /// containing a JWT token if authentication is successful.
        /// </summary>
        /// <remarks>This method processes a login request by delegating authentication to a mediator
        /// command. Upon successful authentication, a JWT token is generated and included in the response. If
        /// authentication fails, an Unauthorized result is returned. The endpoint is intended for use in scenarios
        /// where clients need to obtain a JWT token for subsequent authenticated requests.</remarks>
        /// <param name="loginDto">An object containing the user's login credentials, such as username and password. This parameter is
        /// required.</param>
        /// <returns>An ActionResult containing an AuthResponseDto with a JWT token if authentication succeeds; otherwise, an
        /// Unauthorized result with error details.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var command = new LoginCommand { LoginDto = loginDto };
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return Unauthorized(response);

         
            response.Token = GenerateJwtToken(response);

            return Ok(response);
        }
        /// <summary>
        /// Retrieves information about the currently authenticated user, including their unique identifier, username,
        /// email address, and assigned role.
        /// </summary>
        /// <remarks>This method requires the user to be authenticated. It is typically used to allow
        /// clients to display or utilize the current user's profile information after login.</remarks>
        /// <returns>An IActionResult containing a JSON object with the current user's ID, username, email, and role. Returns a
        /// 200 OK response if the user is authenticated; otherwise, returns an appropriate error response.</returns>
       
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Id = userId,
                Username = username,
                Email = email,
                Role = role
            });
        }
        /// <summary>
        /// Generates a JSON Web Token (JWT) that contains claims for the specified user, including user ID, email,
        /// username, and role.
        /// </summary>
        /// <remarks>The generated token is valid for 24 hours and is signed using a symmetric key
        /// specified in the configuration settings. Ensure that the secret key is kept secure and sufficiently complex
        /// to prevent unauthorized access.</remarks>
        /// <param name="user">An object containing the user's authentication information, such as ID, email, username, and role, which are
        /// included as claims in the generated token.</param>
        /// <returns>A string representing the generated JWT, which can be used for authenticating and authorizing the user in
        /// subsequent requests.</returns>
        private string GenerateJwtToken(AuthResponseDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] ??
                "YourSuperSecretKeyHereMakeItVeryLongAndComplex123!@#");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role), 
                new Claim("userId", user.Id.ToString()),
                new Claim("userRole", user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _configuration["JwtSettings:Issuer"] ?? "Localhost",
                Audience = _configuration["JwtSettings:Audience"] ?? "Localhost",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
