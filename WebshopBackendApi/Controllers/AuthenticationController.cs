using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebshopBackendApi.Utilities;
using System.Text;
using WebshopBackendApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebshopBackendApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DatabaseContext DatabaseContext;

        public AuthenticationController(DatabaseContext DatabaseContext) => this.DatabaseContext = DatabaseContext;

        [HttpPost("login")]
        public IActionResult Login()
        {
            string AuthHeader = Request.Headers["Authorization"];
            if (AuthHeader == null || !AuthHeader.StartsWith("Basic")) return null;

            string EncodedCredentials = AuthHeader.Split(" ")[1];

            byte[] data = System.Convert.FromBase64String(EncodedCredentials);
            string Credentials = System.Text.ASCIIEncoding.ASCII.GetString(data);
            string email = Credentials.Split(":")[0];
            string password = Credentials.Split(":")[1];

            if (DatabaseContext.Users.Any(user => user.Email == email && user.Password == password))
            {
                UserModel user = DatabaseContext.Users.FirstOrDefault(user => user.Email == email && user.Password == password);

                string token = JsonWebTokenUtility.Sign(new Dictionary<string, object> {
                    {"id", user.Id },
                    {"email", user.Email },
                    {"isAdministrator", user.isAdministrator }
                });


                return Ok(new { token });
            }
            else
            {
                return Unauthorized("Invalid credentials.");
            }



        }

    }
}
