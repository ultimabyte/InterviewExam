using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace InterviewExamWebApi.Models
{
    public partial class MstProductCategory
    {
        public MstProductCategory()
        {
            TProducts = new HashSet<TProduct>();
        }

        [Key]
        [Range(0, int.MaxValue, ErrorMessage = "Id positive number")]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Length must less than 100")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Sequence positive number")]
        public int? Sequence { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string CreateBy { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Create Date must be date")]
        public DateTime CreateDate { get; set; }

        [StringLength(50, ErrorMessage = "Length must less than 50")]
        public string UpdateBy { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Create Date must be date")]
        public DateTime? UpdateDate { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "IsActive must be true or false")]
        public bool IsActive { get; set; }

        public virtual ICollection<TProduct> TProducts { get; set; }
    }
}
