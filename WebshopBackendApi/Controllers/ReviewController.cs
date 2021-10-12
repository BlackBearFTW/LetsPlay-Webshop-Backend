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
        private readonly DatabaseContext DatabaseContext;

        public ReviewController(DatabaseContext DatabaseContext) => this.DatabaseContext = DatabaseContext;

        [HttpGet]
        public IActionResult GetAll(Guid productId)
        {
            return Ok(DatabaseContext.Reviews.Where(review => review.ProductId == productId));
        }

        [HttpGet("{reviewId:guid}")]
        public IActionResult Get(Guid productId, Guid reviewId)
        {
            return Ok(DatabaseContext.Reviews.Where(review => review.ProductId == productId && review.Id == reviewId));
        }

        [HttpPost]
        public IActionResult Post(Guid productId, ReviewModel reviewModel)
        {
            reviewModel.Id = Guid.NewGuid();
            reviewModel.ProductId = productId;
            DatabaseContext.Reviews.Add(reviewModel);
            DatabaseContext.SaveChanges();

            return Ok(reviewModel);
        }
    }
}
