using System.ComponentModel.DataAnnotations;

namespace LMSCoreMVC.Models
{
    public class Subjects
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CreditPoints { get; set; }
    }
}
