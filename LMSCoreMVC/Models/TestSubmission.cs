using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSCoreMVC.Models
{
    public class TestSubmission
    {
        [Key] public int Id { get; set; }

        [Required] public int StudentTestId { get; set; }
        [ForeignKey("StudentTestId")] public StudentTest StudentTest { get; set; }

        [Required] public int QuestionId { get; set; }
        [ForeignKey("QuestionId")] public TestQuestion Question { get; set; }

        [Required] public string SelectedAnswer { get; set; } // "A"/"B"/"C"/"D"
    }
}
