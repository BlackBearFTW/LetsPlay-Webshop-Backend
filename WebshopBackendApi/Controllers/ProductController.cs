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
        public IActionResult GetAll([FromQuery] string search, [FromQuery] bool OnlyInStock = false)
        {
            List<ProductModel> products = DatabaseContext.Products.ToList();

            products = search is not null ? products.Where(product => product.Name.Contains(search)).ToList() : products;

            products = OnlyInStock ? products.Where(product => product.Stock > 0).ToList() : products;

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

            if (productModel is null) return BadRequest(new { error = "Unknown product." });

            productModel.Slug = updatedProductModel.Slug ?? productModel.Slug;
            productModel.Name = updatedProductModel.Name ?? productModel.Name;
            if (updatedProductModel.Price is not null) productModel.Price = updatedProductModel.Price;
            if (updatedProductModel.Stock is not null) productModel.Stock = updatedProductModel.Stock;

            DatabaseContext.SaveChanges();

            return Ok(productModel);
        }

        [HttpPut("{slug}")]
        public IActionResult PutBySlug(string slug, ProductModel updatedProductModel)
        {
            ProductModel productModel = DatabaseContext.Products.FirstOrDefault(product => product.Slug == slug);

            if (productModel is null) return BadRequest(new { error = "Unknown product." });

            productModel.Name = updatedProductModel.Name ?? productModel.Name;
            if (updatedProductModel.Price is not null) productModel.Price = updatedProductModel.Price;
            if (updatedProductModel.Stock is not null) productModel.Stock = updatedProductModel.Stock;

            DatabaseContext.SaveChanges();

            return Ok(productModel);
        }
    }
}
