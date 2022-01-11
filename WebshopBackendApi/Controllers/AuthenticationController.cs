using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebshopBackendApi.Utilities;
using System.Text;
using WebshopBackendApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using static BCrypt.Net.BCrypt;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;


namespace WebshopBackendApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AuthenticationController(DatabaseContext databaseContext) => this._context = databaseContext;

        [HttpPost("login")]
        public IActionResult Login()
        {
            string AuthHeader = Request.Headers["Authorization"];
            if (AuthHeader == null || !AuthHeader.StartsWith("Basic")) return BadRequest(new { error = "Unknown authorization method." });

            string EncodedCredentials = AuthHeader.Split(" ")[1];

            byte[] data = System.Convert.FromBase64String(EncodedCredentials);
            string Credentials = System.Text.ASCIIEncoding.ASCII.GetString(data);
            string email = Credentials.Split(":")[0];
            string password = Credentials.Split(":")[1];

            UserModel user = _context.Users.FirstOrDefault(user => user.Email == email);

            if (user is null) return BadRequest(new { error = "Unknown email." });

            if (!Verify(password, user.Password)) return BadRequest(new { error = "Invalid credentials." });

            string token = JsonWebTokenUtility.Sign(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Email, $"{user.Email}"),
                new Claim(ClaimTypes.Role, user.isAdministrator ? "Administrator" : "User")
            });


            return Ok(new { token });
        }

        [HttpPost("register")]
        public IActionResult Register(UserModel userModel)
        {
            if (userModel.Email is null || userModel.Password is null) return BadRequest(new { error = "Email and password are required." });
            if (_context.Users.Any(user => user.Email == userModel.Email)) return BadRequest(new { error = "Email is already in use." });

            userModel.Id = Guid.NewGuid();
            userModel.Password = HashPassword(userModel.Password);
            _context.Users.Add(userModel);
            _context.SaveChanges();

            return Ok(userModel);
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] String email)
        {
            if (email is null || !new EmailAddressAttribute().IsValid(email)) return BadRequest(new { error = "Please supply a valid email adress." });



            return Ok(new { token = "DummyToken"});
        }
    }
}
