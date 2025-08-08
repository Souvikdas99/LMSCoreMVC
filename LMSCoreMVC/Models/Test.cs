using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSCoreMVC.Models
{
    public class Test
    {
        [Key] public int Id { get; set; }

        [Required] public string Subject { get; set; } // AI, DataStructures, Cloud computing, Core Java, Machine Learning

        [Required] public DateTime ScheduledAt { get; set; } // UTC recommended

        [Required] public int DurationInMinutes { get; set; }

        [Required] public int FullMarks { get; set; }

        public ICollection<TestQuestion> Questions { get; set; }
        public ICollection<StudentTest> StudentTests { get; set; }
    }
}
