using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebshopBackendApi.Models;
using WebshopBackendApi.DTO;

namespace WebshopBackendApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext DatabaseContext;

        public UserController(DatabaseContext DatabaseContext) => this.DatabaseContext = DatabaseContext;

        [HttpGet]
        public IActionResult GetAll()
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
            UserModel user = DatabaseContext.Users.Find(uuid);

            if (user is null) return BadRequest("Unknown user");

            CartModel cart = DatabaseContext.Carts.FirstOrDefault(cart => cart.Id == user.CartId);

            if (cart is null) return StatusCode(500);

            return Ok(cart);
        }*/

    }
}
