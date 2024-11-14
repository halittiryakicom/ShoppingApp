using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Operations.User;
using ShoppingApp.Business.Operations.User.Dtos;
using ShoppingApp.WebApi.Jwt;
using ShoppingApp.WebApi.Models;
using LoginRequest = ShoppingApp.WebApi.Models.LoginRequest;
using RegisterRequest = ShoppingApp.WebApi.Models.RegisterRequest;

namespace ShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Password = request.Password,
                BirthDate = request.BirthDate,
            };

            var result = await _userService.AddUser(addUserDto);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password });
            if (!result.IsSucceed) return BadRequest(result.Message);

            //Bilgiler doğru ise ---> JWT
            var user = result.Data;
            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });

            return Ok(new LoginResponse
            {
                Message = "Giriş başarılı ile tamamlandı",
                Token = token,
            });

            
        }

        [HttpGet("me")]
        [Authorize] //token yoksa cevap yok
        public IActionResult GetMyUser()
        {
            return Ok();
        }
    }
}
