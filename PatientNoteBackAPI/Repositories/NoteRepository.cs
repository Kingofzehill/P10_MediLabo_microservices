using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using PatientNoteBackAPI.Data;
using PatientNoteBackAPI.Domain;

namespace PatientNoteBackAPI.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly LocalMongoDbContext _localMongoDb;
        public NoteRepository(LocalMongoDbContext localMongoDb)
        {
            _localMongoDb = localMongoDb;
        }

        /// <summary>Patient Note Repository (CRUD operation). List.</summary>      
        /// <return>List of Patient Notes DTO objects.</return>         
        /// <remarks></remarks>
        public async Task<List<Note>> List(int patientId)
        {
            try
            {
                return await _localMongoDb.Notes.Where(note => note.PatientId == patientId).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Patient Note Repository (CRUD operation). Create.</summary>      
        /// <return>Returns Patient Note DTO object.</return> 
        /// <param name="note">Note DTO object.</param>
        /// <remarks></remarks>
        public async Task<Note> Create(Note note)
        {
            try
            {
                await _localMongoDb.Notes.AddAsync(note);
                await _localMongoDb.SaveChangesAsync();
                return note;
            }
            catch
            {
                throw;
            }
        }
    }
}
