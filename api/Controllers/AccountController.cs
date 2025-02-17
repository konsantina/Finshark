using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{ //Αυτός ο controller είναι υπεύθυνος για το endpoint εγγραφής ενός νέου χρήστη.
    [Route("api/account")]  //[Route("api/account")] → Όλα τα requests που ξεκινούν με /api/account θα φτάνουν σε αυτόν τον controller.
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager; //UserManager<AppUser> → Το Identity framework διαχειρίζεται τους χρήστες.
         public AccountController(UserManager<AppUser> userManager)
         {
            _userManager = userManager;
         }
      
      [HttpPost("register")]

      public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
      {
        try
        {
             if(!ModelState.IsValid)
             return BadRequest(ModelState);

             var appUser = new AppUser
             {
                UserName = registerDto.Username,
                Email = registerDto.Email    
             };

             var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

             if(createUser.Succeeded)
             {

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if(roleResult.Succeeded)
                {
                    return Ok("User created");
                }
                else 
                return StatusCode(500, roleResult.Errors);
             }
             else
             {
                return StatusCode(500, createUser.Errors);
             }   
        }
         
        catch (Exception e) 
        {
            return StatusCode(500, e);
        }


      }
    }
        
    
}