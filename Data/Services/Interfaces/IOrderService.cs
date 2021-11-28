﻿using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IOrderService
    {
        public bool AddToOrder(Orders orders);

        public Task<IEnumerable<Orders>> GetAllOrders();

        public Task<IEnumerable<Orders>> GetAllOrdersByUser(); //temporary parameterless

    }
}
