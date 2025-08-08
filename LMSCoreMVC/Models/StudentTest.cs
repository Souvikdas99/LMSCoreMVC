using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSCoreMVC.Models
{
    public class StudentTest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TestId { get; set; }
        [ForeignKey("TestId")]
        public Test Test { get; set; }

        // Link to User table's Id column
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User Student { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        public DateTime? SubmittedDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // "Pending" or "Submitted"

        public int? Score { get; set; }

        public ICollection<TestSubmission> Submissions { get; set; }
    }
}
