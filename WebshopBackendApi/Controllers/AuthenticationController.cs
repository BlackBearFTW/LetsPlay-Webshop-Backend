using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopBackendApi.Models;
using WebshopBackendApi.Utilities;

namespace WebshopBackendApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public AuthenticationController(DatabaseContext databaseContext) => _databaseContext = databaseContext;

        [HttpPost("login")]
        public IActionResult Login()
        {
            string authHeader = Request.Headers["Authorization"];
            if (authHeader == null || !authHeader.StartsWith("Basic")) return null;

            string encodedCredentials = authHeader.Split(" ")[1];

            byte[] data = Convert.FromBase64String(encodedCredentials);
            string credentials = ASCIIEncoding.ASCII.GetString(data);
            string email = credentials.Split(":")[0];
            string password = credentials.Split(":")[1];

            if (_databaseContext.Users.Any(user => user.Email == email && user.Password == password))
            {
                UserModel user = _databaseContext.Users.FirstOrDefault(user => user.Email == email && user.Password == password);

                return Ok(JsonWebTokenUtility.Sign(new Dictionary<string, object> {
                    {"id", user.Id },
                    {"email", user.Email },
                    {"isAdministrator", user.IsAdministrator }
                }));
            }

            return Unauthorized();



        }

    }
}
