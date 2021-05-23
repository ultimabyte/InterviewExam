using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InterviewExamWebApi.Models
{
    public partial class TShoppingCartDTO
    {

        [Key]
        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int Id { get; private set; }

        public ICollection<TShoppingItemDTO> TShoppingItems { get; set; }
    }
}
