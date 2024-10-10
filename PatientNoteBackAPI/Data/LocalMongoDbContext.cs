using Microsoft.EntityFrameworkCore;
using PatientNoteBackAPI.Domain;
using MongoDB.EntityFrameworkCore.Extensions;

namespace PatientNoteBackAPI.Data
{
    public class LocalMongoDbContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public LocalMongoDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            // Model class Note to MongoDb collection Notes.
            model.Entity<Note>().ToCollection("Notes");
        }        
    }
}
