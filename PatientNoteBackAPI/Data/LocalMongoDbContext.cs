using Microsoft.EntityFrameworkCore;
using PatientNoteBackAPI.Domain;
using MongoDB.EntityFrameworkCore.Extensions;

namespace PatientNoteBackAPI.Data
{
    public class LocalMongoDbContext(DbContextOptions options) : DbContext(options)
    {
        //public LocalMongoDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Model class Note to MongoDb collection Notes.
            modelBuilder.Entity<Note>().ToCollection("Notes");
        }
        public DbSet<Note> Notes { get; init; }
    }
}
