using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NZWalks.API.Models.DTO.AccountDtos;
using NZWalks.API.Repositories.AccountRepos;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        
        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Registrer([FromBody] RegisterRequestDto registerRequestDto) 
        {
            var identityuser = new IdentityUser 
            {
            UserName=registerRequestDto.Username,
            Email=registerRequestDto.Username
            };
            var identityResult=await userManager.CreateAsync(identityuser,registerRequestDto.Password);
            if (identityResult.Succeeded) 
            {
                //add roles to the user
                if (registerRequestDto.Roles != null&& registerRequestDto.Roles.Any())
                {
                    identityResult= await userManager.AddToRolesAsync(identityuser, registerRequestDto.Roles);
                    if (identityResult.Succeeded) 
                    {
                        return Ok("User is Registered Login pls");
                    }
                }
            }
            return BadRequest("Somthing went Wrong");

        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDto loginRequestDto) 
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if(user != null) 
            {
               var checkRasswordResult= await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkRasswordResult) 
                {
                    //Getting user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null) 
                    {
                        //CreteToken
                       var jwttoken= tokenRepository.CreateJWTToken(user, roles.ToList());
                        var responce= new LoginResponceDto { JwtToken = jwttoken };
                        return Ok(responce);
                    }                  
                }
            }
            return BadRequest("UserName or Password was incorrect");
        }


    }
}
