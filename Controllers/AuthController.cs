using Microsoft.AspNetCore.Mvc;
using Net8_JWT.WebAPI.DTOs;
using Net8_JWT.WebAPI.Models;
using Net8_JWT.WebAPI.Services;
using Net8_JWT.WebAPI.Utilities;

namespace Net8_JWT.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtProvider _jwtProvider;
        public AuthController(JwtProvider jwtProvider)
        {
            _jwtProvider = jwtProvider;
        }

        [HttpPost]
        public IActionResult Register(AppUserRegisterDto request)
        {
            byte[] passwordHash, passwordSalt;
            AppUser user = Mapper.Map<AppUserRegisterDto, AppUser>(request);
            HashingHelper.CreatePassord(request.Password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            GlobalData.Users.Add(user);
            return Ok(new { Message = "User created" });
        }

        [HttpPost]
        public IActionResult Login(LoginDto request)
        {
            AppUser? user = GlobalData.Users.FirstOrDefault(x => x.UserName == request.UserName);
            if (user is null)
            {
                throw new Exception("User cannot find");
            }

            bool checkPassword = HashingHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!checkPassword)
            {
                return BadRequest(new { Message = "Password is wrong" });
            }
            return Ok(new { Token = _jwtProvider.CreateToken(user) });
        }
    }
}
