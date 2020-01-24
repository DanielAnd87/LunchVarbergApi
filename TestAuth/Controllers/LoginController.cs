﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestAuth.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using TestAuth.Data;

namespace TestAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration configuration;
        
        public UserCredentialsContext context { get; set; }
        public LoginController(IConfiguration configuration, UserCredentialsContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            UserCredentials userLoging = new UserCredentials();
            userLoging.UserName = username;
            userLoging.Password = password;
            IActionResult response = Unauthorized();
            UserCredentials user = await AuthenticateUserAsync(userLoging);
            if (user != null)
            {
                string tokenStr = GenerateWebToken(user);
                return Ok(new { token = tokenStr });
            }
            return response;
        }
        
        public UserCredentials GetUserCredentials(string username){
            IQueryable<UserCredentials> queryForUsername = context.UserCredentials
                .Where(credential => credential.UserName == username);
            UserCredentials credentials = null;
            foreach (UserCredentials item in queryForUsername)
            {
                credentials = item;
            }

            return credentials;

            //return context.UserCredential
            //    .Where(credential => credential.UserName == "Daniel");

        }

        private string GenerateWebToken(UserCredentials user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            

            int isAdmin = 0;
            if (user.IsAdmin)
            {
                isAdmin = 1;
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, "my_email@gmail.com"),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Gender, isAdmin.ToString()),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            JwtSecurityToken token = new JwtSecurityToken( 
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);
            string encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }

        private async Task<UserCredentials> AuthenticateUserAsync(UserCredentials userLoging)
        {
            UserCredentials credentials = GetUserCredentials(userLoging.UserName);
            
            UserCredentials user = null;
            if (userLoging.UserName == credentials.UserName || Encryption.DecryptString("kljsdkkdlo4454GG00155sajuklmbkdl", userLoging.Password) == credentials.Password)
            {
                user = new UserCredentials() { 
                    Id = credentials.Id, 
                    Password = credentials.Password, 
                    UserName = credentials.UserName, 
                    IsAdmin = credentials.IsAdmin };
            }
            return user;
        }
        [HttpPost("CreateUser")]
        [Authorize]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCredentials user)
        {
            if (!IsAdmin())
            {
                return Unauthorized("Only administrators is allowed to create users.");
            }
            IActionResult response = Unauthorized();
            if (user != null)
            {
                user.Password = Encryption.EncryptString("kljsdkkdlo4454GG00155sajuklmbkdl", user.Password);
                //user.Password = Encryption.EncryptString(configuration["Jwt:Key"], user.Password);
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<UserCredentials> entityEntry = context.UserCredentials.Add(user);
                await context.SaveChangesAsync();
                return Ok("Success!");
            }
            return response;
        }

        private bool IsAdmin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            bool isAdmin = claim[3].Value == "1" ? true : false;
            return isAdmin;
        }



        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var email = User.FindFirst("Daniel")?.Value;
            
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            string userName = claim[2].Value;
            return "Welcom To: " + userName;
        }
        [Authorize]

        [HttpGet("GetValue")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new String[]
            {
                "Value1",
                "Value2",
                "Value3"
            };
        }

    }
} 