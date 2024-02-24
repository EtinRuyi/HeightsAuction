using AutoMapper;
using HeightsAuction.Application.DTOs;
using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Domain;
using HeightsAuction.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HeightsAuction.Application.ServicesImplementations
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AuthenticationServices> _logger;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationServices(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AuthenticationServices> logger,
            IConfiguration config,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (existingUser == null)
                {
                    return ApiResponse<LoginResponseDto>.Failed(false, "User not found.", StatusCodes.Status404NotFound, new List<string>());
                }
                var result = await _signInManager.CheckPasswordSignInAsync(existingUser, loginRequest.Password, lockoutOnFailure: false);

                switch (result)
                {
                    case { Succeeded: true }:
                        var role = (await _userManager.GetRolesAsync(existingUser)).FirstOrDefault();

                        var response = _mapper.Map<LoginResponseDto>(existingUser);
                        response.Token = GenerateJwtToken(existingUser, role);
                       
                        return ApiResponse<LoginResponseDto>.Success(response, "Logged In Successfully", StatusCodes.Status200OK);

                    case { IsLockedOut: true }:
                        return ApiResponse<LoginResponseDto>.Failed(false, $"Account is locked out. Please try again later or contact support." +
                            $" You can unlock your account after {_userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes} minutes.", StatusCodes.Status403Forbidden, new List<string>());

                    case { RequiresTwoFactor: true }:
                        return ApiResponse<LoginResponseDto>.Failed(false, "Two-factor authentication is required.", StatusCodes.Status401Unauthorized, new List<string>());

                    case { IsNotAllowed: true }:
                        return ApiResponse<LoginResponseDto>.Failed(false,"Login failed. Email confirmation is required.", StatusCodes.Status401Unauthorized, new List<string>());

                    default:
                        return ApiResponse<LoginResponseDto>.Failed(false, "Login failed. Invalid email or password.", StatusCodes.Status401Unauthorized, new List<string>());
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseDto>.Failed(false, "Some error occurred while loggin in." + ex.InnerException, StatusCodes.Status500InternalServerError, new List<string>());
            }
        }

        //public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequest)
        //{
        //    try
        //    {
        //        var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
        //        if (existingUser != null)
        //        {
        //            return ApiResponse<RegisterResponseDto>.Failed(false, "User with this email already exists.", StatusCodes.Status400BadRequest, new List<string>());
        //        }

        //        var appUser = _mapper.Map<AppUser>(registerRequest);
        //        appUser.NormalizedUserName = _userManager.NormalizeName(registerRequest.Email);
        //        appUser.NormalizedEmail = _userManager.NormalizeEmail(registerRequest.Email);

        //        var result = await _userManager.CreateAsync(appUser, registerRequest.Password);

        //        if (result.Succeeded)
        //        {
        //            await _userManager.AddToRoleAsync(appUser, "User");
        //        }
        //        await _unitOfWork.Users.AddAsync(appUser);
        //        await _unitOfWork.SaveChangesAsync();
        //        var response = _mapper.Map<RegisterResponseDto>(appUser);

        //        return ApiResponse<RegisterResponseDto>.Success(response, "User registered successfully.", StatusCodes.Status201Created);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while adding a manager " + ex.InnerException);
        //        return ApiResponse<RegisterResponseDto>.Failed(false, "Error creating user.", StatusCodes.Status500InternalServerError, new List<string>() { ex.InnerException.ToString() });
        //    }
        //}

        public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequest)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
                if (existingUser != null)
                {
                    return ApiResponse<RegisterResponseDto>.Failed(false, "User with this email already exists.", StatusCodes.Status400BadRequest, new List<string>());
                }

                var appUser = _mapper.Map<AppUser>(registerRequest);
                var result = await _userManager.CreateAsync(appUser, registerRequest.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "User");
                }
                await _unitOfWork.Users.AddAsync(appUser);
                await _unitOfWork.SaveChangesAsync();
                var response = _mapper.Map<RegisterResponseDto>(appUser);

                return ApiResponse<RegisterResponseDto>.Success(response, "User registered successfully.", StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a manager " + ex.InnerException);
                return ApiResponse<RegisterResponseDto>.Failed(false, "Error creating user.", StatusCodes.Status500InternalServerError, new List<string>() { ex.InnerException.ToString() });
            }
        }



        private string GenerateJwtToken(AppUser contact, string roles)
        {
            var jwtSettings = _config.GetSection("JwtSettings:Secret").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, contact.UserName),
                new Claim(JwtRegisteredClaimNames.Email, contact.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, roles)
            };

            var token = new JwtSecurityToken(
                issuer: _config.GetValue<string>("JwtSettings:ValidIssuer"),
            audience: _config.GetValue<string>("JwtSettings:ValidAudience"),
            //issuer: null,
            //audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config.GetSection("JwtSettings:AccessTokenExpiration").Value)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
