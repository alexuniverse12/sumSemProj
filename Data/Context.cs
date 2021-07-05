using Microsoft.EntityFrameworkCore;
using summerSemesterProj.Models;

namespace summerSemesterProj.Data {
    public class Context : DbContext {
        public Context(DbContextOptions<Context> opt ) : base(opt) {
            
        }

        public DbSet<User> Users {get; set;}
        public DbSet<Note> Notes {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<User>()
                        .Property(e => e.Id)
                        .ValueGeneratedOnAdd();
            modelBuilder.Entity<Note>()
                        .Property(e => e.noteId)
                        .ValueGeneratedOnAdd();
        }
    }
}