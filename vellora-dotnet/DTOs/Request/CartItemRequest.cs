﻿namespace Vellora.ECommerce.API.DTOs.Request
{
    public class CartItemRequest
    {
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
    }
}