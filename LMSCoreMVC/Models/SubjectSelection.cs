using System.ComponentModel.DataAnnotations;

namespace LMSCoreMVC.Models
{
    public class SubjectSelection
    {
        public int Id { get; set; }

        [Required]
        public string SubjectName { get; set; }

        public int CreditPoints { get; set; }

        [Required]
        public string Username { get; set; } // Set from logged-in student
    }
}
