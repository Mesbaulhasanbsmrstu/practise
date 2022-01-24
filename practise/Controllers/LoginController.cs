using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using practise.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using practise.IRepository;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;

namespace practise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
       public IConfiguration Configuration { get; }
        private readonly ILogin _login;
        private readonly practiseContext _db;

        public LoginController(IConfiguration config,ILogin login, practiseContext db)
        {
            Configuration = config;
            _login = login;
            _db = db;
        }
        [AllowAnonymous]
        [HttpPost(Name="login")]
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {

                return GenerateJSONWebToken(user);
               // var tokenString = GenerateJSONWebToken(user);
              // response = Ok(new { token = tokenString,UserName= login.Username });
            }

           return response;
           // return GenerateJSONWebToken(user);
        }
        [HttpPost("tokenRefresh",Name ="TokenRefresh")]
        //[Route("tokenRefresh")]
        [Authorize]
        public IActionResult renewToken()
        {
            UserModel user = new UserModel
            {
                Username = HttpContext.User.Identity.Name,
                EmailAddress=HttpContext.User.Identity.Name

            };
           return GenerateJSONWebToken(user);
       //  return Ok(new { token = tokenString});

        }
 
        private IActionResult GenerateJSONWebToken(UserModel userInfo)
        {

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Audience:Secret"]));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,userInfo.Username),
                new Claim(ClaimTypes.Email,userInfo.EmailAddress)

            };
            var expairTime = DateTime.Now.AddMinutes(2);
            var token = new JwtSecurityToken(Configuration["Audience:Iss"],
              Configuration["Audience:Iss"],
              claims,
              expires: expairTime,
              signingCredentials: credentials);

           // return new JwtSecurityTokenHandler().WriteToken(token);
           return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), ExpairTime = expairTime });
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    
            try
            {
               var verifyObject =  _db.Persons.Where(b => b.FirstName == login.Username).ToList();

                if (verifyObject.Count>0)
                {
                    user = new UserModel { Username = login.Username, EmailAddress = login.EmailAddress };
                    return user;
                }
                return user;
            }catch (Exception ex)
            {
                return null;
            }

        }
    }
}
