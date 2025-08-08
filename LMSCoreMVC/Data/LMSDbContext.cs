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

     



    }
}
