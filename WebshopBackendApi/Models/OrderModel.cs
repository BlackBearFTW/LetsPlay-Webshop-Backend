using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebshopBackendApi.Models
{
    public class OrderModel
    {
        [Key]
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
    }
}
