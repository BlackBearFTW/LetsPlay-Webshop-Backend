using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebshopBackendApi.Models;
using WebshopBackendApi.Repositories;
using WebshopBackendApi.DTO;

namespace WebshopBackendApi.Controllers
{
    [Route("[controller]s")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext DatabaseContext;

        public UserController(DatabaseContext DatabaseContext) => this.DatabaseContext = DatabaseContext;

        [HttpGet]
        public IActionResult Get()
        {
            List<UserDTO> users = DatabaseContext.Users.ToList().ConvertAll(user => new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                isAdministrator = user.isAdministrator
            });

            return Ok(users);
        }

        [HttpGet("{uuid}")]
        public IActionResult Get(Guid uuid)
        {
            UserModel user = DatabaseContext.Users.Find(uuid);

            if (user is null) return BadRequest("Unknown user");

            return Ok(new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                isAdministrator = user.isAdministrator
            });
        }

/*        [HttpGet("{uuid}/cart")]
        public IActionResult GetCart(Guid uuid)
        {
            UserModel user = UserRepository.Users.Find(item => item.Id == uuid);

            if (user is null) return BadRequest("Unknown user");

            CartModel cart = CartRepository.Carts.Find(item => item.Id == user.CartId);

            if (cart is null) return StatusCode(500);

            return Ok(cart);
        }*/

    }
}
