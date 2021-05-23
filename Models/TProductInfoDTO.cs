using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InterviewExamWebApi.Models
{
    public partial class TProductInfoDTO
    {
        [Key]
        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Id must be positive number")]
        public int ProductId { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Length must less than 1000")]
        public string Detail { get; set; }

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
    }
}
