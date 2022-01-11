using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebshopBackendApi.Models;
using WebshopBackendApi.DTO;
using Microsoft.AspNetCore.Authorization;

namespace WebshopBackendApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CartController(DatabaseContext databaseContext) => this._context = databaseContext;

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Carts.ToList());
        }

        [HttpGet("{uuid}")]
        public IActionResult Get(Guid uuid)
        {
            CartModel cart = _context.Carts.Find(uuid);

            if (cart is null) return BadRequest(new { error = "Unknown cart." });

            return Ok(new CartDTO()
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreationDate = cart.CreationDate,
                Orders = _context.Orders.Where(order => order.CartId == cart.Id).ToList()
            });
        }

        [HttpPost]
        public IActionResult Post(CartModel cartModel)
        {
            cartModel.Id = Guid.NewGuid();
            _context.Carts.Add(cartModel);
            _context.SaveChanges();

            return Ok(cartModel);
        }
    }
}
