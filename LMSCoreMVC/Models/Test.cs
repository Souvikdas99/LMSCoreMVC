using System;
using System.ComponentModel.DataAnnotations;

namespace LMSCoreMVC.Models
{
    public class Test
    {
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TestDate { get; set; }

        [Required]
        public int FullMarks { get; set; }

        public string Description { get; set; }
    }
}
