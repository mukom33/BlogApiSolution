using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BlogApi.Business.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.DataAccess.Abstract;
using BlogApi.Business.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;


namespace BlogApi.Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
       
        public UserService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IMapper mapper,IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task<bool> CreateUser(UserDTO dto)
        {
           
            var user = _mapper.Map<AppUser>(dto);

            var controlUser = await _userManager.FindByEmailAsync(dto.Email);

            bool alreadyExist = controlUser != null;

            if(alreadyExist)
                return false;

            var result = await _userManager.CreateAsync(user,dto.Password);
            
            if (!result.Succeeded)
            {
                List<string> errorList = result.Errors.Select(s => s.Description).ToList();

                return false;
            }

            return true;
        }

        public async Task<string?> LoginAsync(LoginDTO dto)
        {
            var user = await  _userManager.FindByEmailAsync(dto.Email);
            if(user == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user,dto.Password,false);
            if (!result.Succeeded)
            {
                return null;
            }

            return GenerateJWT(user);
        }
        private string GenerateJWT(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Secret").Value ?? "");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName ?? ""),
                        new Claim(ClaimTypes.Email, user.Email ?? "")
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature),
                Issuer = "muhammetyetimcok"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 