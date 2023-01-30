using DutchTreat.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int ProductId { get; set; }

        // prefic the members with the name of the entity that it refers to, the mapper takes them directly
        public string ProductCategory { get; set; }
        public string ProductSize { get; set; }
        public string ProductTitle { get; set; }
        public string ProductArtist { get; set; }
    }
}
