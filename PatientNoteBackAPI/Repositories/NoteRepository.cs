using PatientNoteBackAPI.Data;

namespace PatientNoteBackAPI.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly LocalMongoDbContext _localMongoDb;
        public NoteRepository(LocalMongoDbContext localMongoDb)
        {
            _localMongoDb = localMongoDb;
        }
    }
}
