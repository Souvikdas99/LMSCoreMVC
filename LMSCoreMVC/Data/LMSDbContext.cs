using Microsoft.EntityFrameworkCore;
using LMSCoreMVC.Models;

namespace LMSCoreMVC.Data
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions<LMSDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }

       
        public DbSet<Assignment> Assignment { get; set; }

        public DbSet<SubjectSelection> SubjectSelections { get; set; }

        public DbSet<Attendance> Attendance { get; set; }

        public DbSet<Test> Tests { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<StudentTest> StudentTests { get; set; }
        public DbSet<TestSubmission> TestSubmissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fix multiple cascade path issue
            modelBuilder.Entity<TestSubmission>()
                .HasOne(ts => ts.Question)
                .WithMany()
                .HasForeignKey(ts => ts.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete loop
        }

    }
}
