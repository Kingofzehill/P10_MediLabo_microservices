using PatientNoteBackAPI.Domain;

namespace PatientNoteBackAPI.Repositories
{
    public interface INoteRepository
    {
        Task<List<Note>> List(int patientId);
        Task<Note> Create(Note note);
    }
}
