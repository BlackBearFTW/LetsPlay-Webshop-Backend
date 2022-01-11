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
        private readonly DatabaseContext _context;

        public ProductController(DatabaseContext databaseContext) => this._context = databaseContext;

        [HttpGet]
        public IActionResult GetAll([FromQuery] string search, [FromQuery] bool onlyInStock = false, [FromQuery] int page = 1, [FromQuery] int size = 150)
        {
            List<ProductModel> products = _context.Products.ToList();

            products = products.Skip((page - 1) * size).Take(size).ToList();

            products = search is not null ? products.Where(product => product.Name.ToLower().Contains(search.ToLower())).ToList() : products;

            products = onlyInStock ? products.Where(product => product.Stock > 0).ToList() : products;

            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            ProductModel product = _context.Products.FirstOrDefault(product => product.Id == id);

            if (product is null) return BadRequest(new { error = "Unknown product." });

            return Ok(product);
        }

        [HttpGet("{slug}")]
        public IActionResult Get(string slug)
        {
            ProductModel product = _context.Products.FirstOrDefault(product => product.Slug == slug);

            if (product is null) return BadRequest(new { error = "Unknown product." });

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post(ProductModel productModel)
        {
            productModel.Id = Guid.NewGuid();
            _context.Products.Add(productModel);
            _context.SaveChanges();

            return Ok(productModel);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Put(Guid id, ProductModel updatedProductModel)
        {
            bool doesExist = _context.Products.Any(product => product.Id == id);

            if (!doesExist) return BadRequest(new { error = "Unknown product." });

            updatedProductModel.Id = id;

            _context.Products.Update(updatedProductModel);
            _context.SaveChanges();

            return Ok(updatedProductModel);
        }

        [HttpPut("{slug}")]
        public IActionResult Put(string slug, ProductModel updatedProductModel)
        {
            ProductModel productModel = _context.Products.FirstOrDefault(product => product.Slug == slug);

            if (productModel is null) return BadRequest(new { error = "Unknown product." });

            updatedProductModel.Id = productModel.Id;
            updatedProductModel.Slug = productModel.Slug;
            productModel = updatedProductModel;


            _context.SaveChanges();

            return Ok(productModel);
        }

        // TODO: Add patch for partial updates
    }
}
