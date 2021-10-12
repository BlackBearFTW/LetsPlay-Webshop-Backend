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
        private readonly DatabaseContext DatabaseContext;

        public CartController(DatabaseContext DatabaseContext) => this.DatabaseContext = DatabaseContext;

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(DatabaseContext.Carts.ToList());
        }

        [HttpGet("{uuid}")]
        public IActionResult Get(Guid uuid)
        {
            CartModel cart = DatabaseContext.Carts.Find(uuid);

            if (cart is null) return BadRequest(new { error = "Unknown cart." });

            return Ok(new CartDTO()
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreationDate = cart.CreationDate,
                Orders = DatabaseContext.Orders.Where(order => order.CartId == cart.Id).ToList()
            });
        }

        [HttpPost]
        public IActionResult Post(CartModel cartModel)
        {
            cartModel.Id = Guid.NewGuid();
            DatabaseContext.Carts.Add(cartModel);
            DatabaseContext.SaveChanges();

            return Ok(cartModel);
        }
    }
}
