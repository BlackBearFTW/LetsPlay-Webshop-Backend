using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebshopBackendApi.Models;

namespace WebshopBackendApi.DTO
{
    public class CartDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public List<OrderModel> Orders { get; set; }
    }
}
