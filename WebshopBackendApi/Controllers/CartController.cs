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
    public class CartController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public CartController(DatabaseContext databaseContext) => this._databaseContext = databaseContext;

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_databaseContext.Carts.ToList());
        }

        [HttpGet("{uuid}")]
        public IActionResult Get(Guid uuid)
        {
            CartModel cart = _databaseContext.Carts.Find(uuid);

            if (cart is null) return BadRequest("Unknown cart.");

            return Ok(new CartDTO()
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreationDate = cart.CreationDate,
                Orders = _databaseContext.Orders.Where(order => order.CartId == cart.Id).ToList()
            });
        }

        [HttpPost]
        public IActionResult Post(CartModel cartModel)
        {
            cartModel.Id = Guid.NewGuid();
            _databaseContext.Carts.Add(cartModel);
            _databaseContext.SaveChanges();

            return Ok();
        }
    }
}
