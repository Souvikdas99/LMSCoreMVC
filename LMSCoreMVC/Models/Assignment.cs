using System;
using System.ComponentModel.DataAnnotations;

namespace LMSCoreMVC.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        [Required]
        public string AssignmentName { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; }

        [Required]
        public string TeacherName { get; set; }

        public string? FilePath { get; set; }


        [Required]
        public string StudentName { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime SubmittedAt { get; set; }
    }
}
