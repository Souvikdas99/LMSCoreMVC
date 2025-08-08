using System;
using System.ComponentModel.DataAnnotations;

namespace LMSCoreMVC.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        public string StudentName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string Status { get; set; } // Present or Absent

        public bool IsPresent { get; set; }

    }
}
