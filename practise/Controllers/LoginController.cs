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
        [HttpPost]
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
               var tokenString = GenerateJSONWebToken(user);
               response = Ok(new { token = tokenString,UserName= login.Username });
            }

            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Audience:Secret"]));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Configuration["Audience:Iss"],
              Configuration["Audience:Iss"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
