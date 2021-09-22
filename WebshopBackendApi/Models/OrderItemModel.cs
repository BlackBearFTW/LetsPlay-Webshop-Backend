﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebshopBackendApi.Models
{
    public class OrderItemModel
    {
        public OrderItemModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public object Product { get; set; }
        public int Amount { get; set; }
    }
}