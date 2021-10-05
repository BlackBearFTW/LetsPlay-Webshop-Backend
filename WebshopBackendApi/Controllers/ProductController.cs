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
        private readonly DatabaseContext DatabaseContext;

        public ProductController(DatabaseContext DatabaseContext) => this.DatabaseContext = DatabaseContext;

        [HttpGet]
        public IActionResult GetAll([FromQuery] bool OnlyInStock = false)
        {
            if (OnlyInStock) return Ok(DatabaseContext.Products.Where(product => product.Stock > 0).ToList());

            return Ok(DatabaseContext.Products.ToList());
        }

        [HttpGet("{slug}")]
        public IActionResult GetFromSlug(string slug)
        {
            return Ok(DatabaseContext.Products.FirstOrDefault(product => product.Slug == slug));
        }

        [HttpPost]
        public IActionResult Post(ProductModel productModel)
        {
            DatabaseContext.Products.Add(productModel);
            DatabaseContext.SaveChanges();

            return Ok();
        }
    }
}
