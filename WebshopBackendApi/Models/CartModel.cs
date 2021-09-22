﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebshopBackendApi.Models
{
    public class CartModel
    {
        public CartModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
    }
}
