using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebshopBackendApi.Models;
using WebshopBackendApi.DTO;
using JWT;
using static BCrypt.Net.BCrypt;

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
            return Ok(DatabaseContext.Users.ToList());
        }

        [HttpGet("{uuid}")]
        public IActionResult Get(Guid uuid)
        {
            UserModel user = DatabaseContext.Users.Find(uuid);

            if (user is null) return BadRequest(new { error = "Unknown user." });

            return Ok(user);
        }

        [HttpGet("{uuid}/cart")]
        public IActionResult GetCart(Guid uuid)
        {
            UserModel user = DatabaseContext.Users.Find(uuid);

            if (user is null) return BadRequest(new { error = "Unknown user." });

            CartModel cart = DatabaseContext.Carts.FirstOrDefault(cart => cart.UserId == user.Id);

            if (cart is null) return BadRequest(new { error = "This user doesn't have a cart." });

            return Ok(new CartDTO()
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreationDate = cart.CreationDate,
                Orders = DatabaseContext.Orders.Where(order => order.CartId == cart.Id).ToList()
            });
        }

        [HttpPost]
        public IActionResult Post(UserModel userModel)
        {
            if (userModel.Email is null || userModel.Password is null) return BadRequest(new { error = "Email and password are required." });
            if (DatabaseContext.Users.Any(user => user.Email == userModel.Email)) return BadRequest(new { error = "Email is already in use." });

            userModel.Id = Guid.NewGuid();
            userModel.Password = HashPassword(userModel.Password);
            DatabaseContext.Users.Add(userModel);
            DatabaseContext.SaveChanges();

            return Ok(userModel);
        }
    }
}
