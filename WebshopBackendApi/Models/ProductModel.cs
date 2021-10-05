using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebshopBackendApi.Models
{
    public class ProductModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public int? Stock { get; set; }
    }
}
