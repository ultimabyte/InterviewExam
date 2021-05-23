using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InterviewExamWebApi.Models
{
    public partial class TProductDTO
    {
        [Key]
        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int Id { get; private set; }

        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int ProductCategoryId { get; set; }

        [StringLength(100, ErrorMessage = "Length must less than 100")]
        public string ProductCategory { get; private set; }

        [StringLength(100, ErrorMessage = "Length must less than 100")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string Title { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string Sku { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string Barcode { get; set; }

        [StringLength(1000, ErrorMessage = "Length must less than 1000")]
        public string Detail { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be positive number")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive number")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive number")]
        public int QuantityRemain { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive number")]
        public int QuantityMinimum { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive number")]
        public int QuantityMaximum { get; set; }

        [DataType(DataType.Date, ErrorMessage = "DateIn must be date")]
        public DateTime DateIn { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Effect Date must be date")]
        public DateTime? EffectDate { get; set; }

        [Url(ErrorMessage = "Must be image url format")]
        public string Image1400x400 { get; set; }

        [Url(ErrorMessage = "Must be image url format")]
        public string Image2400x400 { get; set; }

        [Url(ErrorMessage = "Must be image url format")]
        public string Image3400x400 { get; set; }

        [Url(ErrorMessage = "Must be image url format")]
        public string Image4400x400 { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "IsActive must be true or false")]
        public bool IsActive { get; set; }
    }
}
