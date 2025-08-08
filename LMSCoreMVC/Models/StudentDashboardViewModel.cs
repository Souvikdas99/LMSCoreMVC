namespace LMSCoreMVC.Models
{
    public class StudentDashboardViewModel
    {
        public string StudentName { get; set; }
        public int TotalAssignments { get; set; }
        public int AttendanceCount { get; set; }
        public int TestAttempted { get; set; }
        public int AverageScore { get; set; }
    }
}
