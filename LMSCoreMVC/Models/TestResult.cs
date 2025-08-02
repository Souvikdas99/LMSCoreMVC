using System.ComponentModel.DataAnnotations;

namespace LMSCoreMVC.Models
{
    public class TestResult
    {
        public int Id { get; set; }

        [Required]
        public string StudentName { get; set; }

        [Required]
        public int TestId { get; set; }

        [Required]
        public int Score { get; set; }
    }
}
