using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InterviewExamWebApi.Models
{
    public partial class TShoppingItemDTO
    {
        [Key]
        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int Id { get; private set; }

        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int ProductId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive number")]
        public int Quantity { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be positive number")]
        public decimal Price { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be positive number")]
        public decimal Total { get; set; }
    }
}
