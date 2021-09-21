using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebshopBackendApi.Repositories;
using WebshopBackendApi.Utilities;
using System.Text;
using WebshopBackendApi.Models;


namespace WebshopBackendApi.Controllers
{
    [Route("authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserRepository UserRepository;

        public AuthenticationController(UserRepository repo) => UserRepository = repo;

        [HttpPost("login")]
        public string Login()
        {
            string AuthHeader = Request.Headers["Authorization"];
            if (AuthHeader == null || !AuthHeader.StartsWith("Basic")) return null;

            string EncodedCredentials = AuthHeader.Split(" ")[1];

            byte[] data = System.Convert.FromBase64String(EncodedCredentials);
            string Credentials = System.Text.ASCIIEncoding.ASCII.GetString(data);
            string email = Credentials.Split(":")[0];
            string password = Credentials.Split(":")[1];

            if (UserRepository.Users.Exists(user => user.Email == email && user.Password == password))
            {
                UserModel user = UserRepository.Users.Find(user => user.Email == email && user.Password == password);

                return JsonWebTokenUtility.Sign(new Dictionary<string, object> {
                    {"id", user.Id },
                    {"email", user.Email },
                    {"isAdministrator", user.isAdministrator }
                });
            }
            else
            {
                return Unauthorized().ToString();
            }



        }

    }
}
