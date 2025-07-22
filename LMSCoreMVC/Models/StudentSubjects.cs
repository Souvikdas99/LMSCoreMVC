namespace LMSCoreMVC.Models
{
    public class StudentSubjects
    {
        public int Id { get; set; }

        public string StudentUsername { get; set; } // Comes from session

        public int SubjectId { get; set; }
        public Subjects Subject { get; set; }
    }
}
