using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSCoreMVC.Models
{
    public class TestQuestion
    {
        [Key] public int Id { get; set; }

        [Required] public int TestId { get; set; }
        [ForeignKey("TestId")] public Test Test { get; set; }

        [Required] public string QuestionText { get; set; }
        [Required] public string OptionA { get; set; }
        [Required] public string OptionB { get; set; }
        [Required] public string OptionC { get; set; }
        [Required] public string OptionD { get; set; }

        /// Correct option as "A" / "B" / "C" / "D"
        [Required] public string CorrectAnswer { get; set; }
    }
}
