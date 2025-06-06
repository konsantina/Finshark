using System;
using System.Threading.Tasks;
using api.Dtos;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;


        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> singInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager =  singInManager;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDto loginDto)
        {
           if (!ModelState.IsValid)
           return BadRequest(ModelState);

           var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

           if (user ==null) return Unauthorized("Invalid username!");

           var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password,false);

           if(!result.Succeeded) return Unauthorized("Username not founs and/or password incorrect");
           return Ok(
             new NewUserDto
             {
                UserName =user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
             }

           );
         }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (!createUser.Succeeded)
                {
                    return StatusCode(500, createUser.Errors);
                }

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (!roleResult.Succeeded)
                {
                    return StatusCode(500, roleResult.Errors);
                }

                // Αν φτάσουμε εδώ, όλα πήγαν καλά. Επιστρέφουμε το token.
                return Ok(new NewUserDto
                {
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Token = _tokenService.CreateToken(appUser)
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}
