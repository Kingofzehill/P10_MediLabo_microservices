using MongoDB.Bson;
using PatientNoteBackAPI.Domain;

namespace PatientNoteBackAPI.Repositories
{
    public interface INoteRepository
    {
        Task<List<Note>> List(int patientId);
        Task<Note?> Get(string id);
        Task<Note> Create(Note note);
        Task<Note?> Delete(string id);
    }
}
