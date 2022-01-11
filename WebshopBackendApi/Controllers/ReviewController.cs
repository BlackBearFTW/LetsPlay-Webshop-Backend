using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebshopBackendApi.Models;

namespace WebshopBackendApi.Controllers
{
    [Route("api/products/{productId:guid}/[controller]s")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ReviewController(DatabaseContext databaseContext) => this._context = databaseContext;

        [HttpGet]
        public IActionResult GetAll(Guid productId)
        {
            return Ok(_context.Reviews.Where(review => review.ProductId == productId));
        }

        [HttpGet("{reviewId:guid}")]
        public IActionResult Get(Guid productId, Guid reviewId)
        {
            return Ok(_context.Reviews.Where(review => review.ProductId == productId && review.Id == reviewId));
        }

        [HttpPost]
        public IActionResult Post(Guid productId, ReviewModel reviewModel)
        {
            reviewModel.Id = Guid.NewGuid();
            reviewModel.ProductId = productId;
            _context.Reviews.Add(reviewModel);
            _context.SaveChanges();

            return Ok(reviewModel);
        }
    }
}
