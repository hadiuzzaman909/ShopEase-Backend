﻿namespace ShopEase.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public decimal TotalCost { get; set; } // Computed in EF Core

        //public User User { get; set; }
        public Product Product { get; set; }
    }
}
