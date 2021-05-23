using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace InterviewExamWebApi.Models
{
    public partial class TOrder
    {
        public TOrder()
        {
            TOrderItems = new HashSet<TOrderItem>();
        }

        [Key]
        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int ShoppingCartId { get; set; }

        [StringLength(100, ErrorMessage = "Length must less than 100")]
        public string CustomerName { get; set; }

        [StringLength(100, ErrorMessage = "Length must less than 100")]
        public string Address1 { get; set; }

        [StringLength(10, ErrorMessage = "Length must less than 10")]
        public string Address2 { get; set; }

        [Phone(ErrorMessage = "Invalid Telephone number")]
        public string Telephone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public virtual TShoppingCart ShoppingCart { get; set; }

        public virtual ICollection<TOrderItem> TOrderItems { get; set; }
    }
}
