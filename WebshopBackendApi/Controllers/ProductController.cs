using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebshopBackendApi.Models;

namespace WebshopBackendApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public ProductController(DatabaseContext databaseContext) => this._databaseContext = databaseContext;

        [HttpGet]
        public IActionResult GetAll([FromQuery] bool onlyInStock = false)
        {
            if (onlyInStock) return Ok(_databaseContext.Products.Where(product => product.Stock > 0).ToList());

            return Ok(_databaseContext.Products.ToList());
        }

        [HttpGet("{slug}")]
        public IActionResult GetFromSlug(string slug)
        {
            return Ok(_databaseContext.Products.FirstOrDefault(product => product.Slug == slug));
        }

        [HttpPost]
        public IActionResult Post(ProductModel productModel)
        {
            _databaseContext.Products.Add(productModel);
            _databaseContext.SaveChanges();

            return Ok();
        }
    }
}
