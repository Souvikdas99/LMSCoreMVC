using Microsoft.EntityFrameworkCore;
using LMSCoreMVC.Models;

namespace LMSCoreMVC.Data
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions<LMSDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<StudentSubjects> StudentSubjects { get; set; }


    }
}
