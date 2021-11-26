using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult GetAll([FromQuery] string search, [FromQuery] bool onlyInStock = false, [FromQuery] int page = 1, [FromQuery] int size = 150)
        {
            List<ProductModel> products = DatabaseContext.Products.ToList();

            products = products.Skip((page - 1) * size).Take(size).ToList();

            products = search is not null ? products.Where(product => product.Name.ToLower().Contains(search.ToLower())).ToList() : products;

            products = onlyInStock ? products.Where(product => product.Stock > 0).ToList() : products;

            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            ProductModel product = DatabaseContext.Products.FirstOrDefault(product => product.Id == id);

            if (product is null) return BadRequest(new { error = "Unknown product." });

            return Ok(product);
        }

        [HttpGet("{slug}")]
        public IActionResult GetBySlug(string slug)
        {
            ProductModel product = DatabaseContext.Products.FirstOrDefault(product => product.Slug == slug);

            if (product is null) return BadRequest(new { error = "Unknown product." });

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post(ProductModel productModel)
        {
            productModel.Id = Guid.NewGuid();
            DatabaseContext.Products.Add(productModel);
            DatabaseContext.SaveChanges();

            return Ok(productModel);
        }


        [HttpPut("{id:guid}")]
        public IActionResult PutById(Guid id, ProductModel updatedProductModel)
        {
            ProductModel productModel = DatabaseContext.Products.FirstOrDefault(product => product.Id == id);

            Console.WriteLine(updatedProductModel.Stock);

            if (productModel is null) return BadRequest(new { error = "Unknown product." });

            productModel = updatedProductModel;
            productModel.Id = id;

            DatabaseContext.SaveChanges();

            return Ok(productModel);
        }

        [HttpPut("{slug}")]
        public IActionResult PutBySlug(string slug, ProductModel updatedProductModel)
        {
            ProductModel productModel = DatabaseContext.Products.FirstOrDefault(product => product.Slug == slug);

            if (productModel is null) return BadRequest(new { error = "Unknown product." });

            Guid id = productModel.Id;
            productModel = updatedProductModel;
            productModel.Id = id;
            productModel.Slug = slug;

            DatabaseContext.SaveChanges();

            return Ok(productModel);
        }

        // TODO: Add patch for partial updates
    }
}
