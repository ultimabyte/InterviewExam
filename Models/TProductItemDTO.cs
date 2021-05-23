using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InterviewExamWebApi.Models
{
    public partial class TProductItemDTO
    {
        [Key]
        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int ProductId { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string Barcode { get; set; }

        [DataType(DataType.Date, ErrorMessage = "DateIn must be date")]
        public DateTime DateIn { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive number")]
        public int Quantity { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be positive number")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive number")]
        public int QuantityRemain { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive number")]
        public int QuantityMinimum { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive number")]
        public int QuantityMaximum { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string CreatedBy { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Create Date Must be date")]
        public DateTime CreatedDate { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string UpdatedBy { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Update Date Must be date")]
        public DateTime? UpdatedDate { get; set; }
    }
}
