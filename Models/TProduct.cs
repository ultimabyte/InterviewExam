using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InterviewExamWebApi.Models
{
    public partial class TProduct
    {
        public TProduct()
        {
            TProductInfos = new HashSet<TProductInfo>();
            TProductItems = new HashSet<TProductItem>();
            TShoppingItems = new HashSet<TShoppingItem>();
        }

        [Key]
        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int ProductCategoryId { get; set; }

        [StringLength(100, ErrorMessage = "Length must less than 100")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string Sku { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string CreatedBy { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Create Date must be date")]
        public DateTime CreatedDate { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string UpdatedBy { get; set; }

        [DataType(DataType.Date, ErrorMessage = "UpdateDate Must be date")]
        public DateTime? UpdatedDate { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "IsActive must be true or false")]
        public bool IsActive { get; set; }

        public virtual MstProductCategory ProductCategory { get; set; }
        public virtual ICollection<TProductInfo> TProductInfos { get; set; }
        public virtual ICollection<TProductItem> TProductItems { get; set; }
        public virtual ICollection<TShoppingItem> TShoppingItems { get; set; }
    }
}
