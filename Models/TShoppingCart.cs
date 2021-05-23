using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InterviewExamWebApi.Models
{
    public partial class TShoppingCart
    {
        public TShoppingCart()
        {
            TOrders = new HashSet<TOrder>();
            TShoppingItems = new HashSet<TShoppingItem>();
        }

        [Key]
        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int Id { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Create Date must be date")]
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<TOrder> TOrders { get; set; }
        public virtual ICollection<TShoppingItem> TShoppingItems { get; set; }
    }
}
