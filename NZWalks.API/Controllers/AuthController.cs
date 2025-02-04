using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using NZWalks.API.Models.DTO.AccountDtos;
using NZWalks.API.Repositories.AccountRepos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        public async Task<IActionResult> Login([FromBody]LoginRequestDto loginRequestDto) 
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    // Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Create Token

                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponceDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or password incorrect");
        }


    }
}
