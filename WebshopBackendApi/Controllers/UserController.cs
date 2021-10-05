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
        private readonly DatabaseContext _databaseContext;

        public UserController(DatabaseContext databaseContext) => this._databaseContext = databaseContext;

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<UserDTO> users = _databaseContext.Users.ToList().ConvertAll(user => new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdministrator = user.IsAdministrator
            });

            return Ok(users);
        }

        [HttpGet("{uuid}")]
        public IActionResult Get(Guid uuid)
        {
            UserModel user = _databaseContext.Users.Find(uuid);

            if (user is null) return BadRequest("Unknown user.");

            return Ok(new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdministrator = user.IsAdministrator
            });
        }

        [HttpGet("{uuid}/cart")]
        public IActionResult GetCart(Guid uuid)
        {
            UserModel user = _databaseContext.Users.Find(uuid);

            if (user is null) return BadRequest("Unknown user.");

            CartModel cart = _databaseContext.Carts.FirstOrDefault(cart => cart.UserId == user.Id);

            if (cart is null) return BadRequest("This user doesn't have a cart.");

            return Ok(new CartDTO()
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreationDate = cart.CreationDate,
                Orders = _databaseContext.Orders.Where(order => order.CartId == cart.Id).ToList()
            });
        }

        [HttpPost]
        public IActionResult Post(UserModel userModel)
        {
            userModel.Id = Guid.NewGuid();
            _databaseContext.Users.Add(userModel);
            _databaseContext.SaveChanges();

            return Ok();
        }
    }
}
