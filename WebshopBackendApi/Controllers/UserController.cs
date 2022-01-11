using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebshopBackendApi.Models;
using WebshopBackendApi.DTO;
using static BCrypt.Net.BCrypt;
using System.Security.Claims;

namespace WebshopBackendApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext databaseContext) => this._context = databaseContext;

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(
                _context.Users.ToList().ConvertAll(user => new UserDTO()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    isAdministrator = user.isAdministrator
                })
                );
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("{uuid}")]
        public IActionResult Get(Guid uuid)
        {
            UserModel user = _context.Users.Find(uuid);

            if (user is null) return BadRequest(new { error = "Unknown user." });

            UserDTO userDTO = new()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                isAdministrator = user.isAdministrator
            };

            return Ok(userDTO);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserModel user = _context.Users.Find(Guid.Parse(userId));

            if (user is null) return BadRequest(new { error = "Unknown user." });

            UserDTO userDTO = new()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                isAdministrator = user.isAdministrator
            };

            return Ok(userDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("{uuid}/cart")]
        public IActionResult GetCart(Guid uuid)
        {
            UserModel user = _context.Users.Find(uuid);

            if (user is null) return BadRequest(new { error = "Unknown user." });

            CartModel cart = _context.Carts.FirstOrDefault(cart => cart.UserId == user.Id);

            if (cart is null) return BadRequest(new { error = "This user doesn't have a cart." });

            return Ok(new CartDTO()
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreationDate = cart.CreationDate,
                Orders = _context.Orders.Where(order => order.CartId == cart.Id).ToList()
            });
        }
    }
}
