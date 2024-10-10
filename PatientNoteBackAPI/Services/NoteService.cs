using PatientNoteBackAPI.Repositories;

namespace PatientNoteBackAPI.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _repositoryNote;
        public NoteService(INoteRepository repositoryNote)
        {
            _repositoryNote = repositoryNote;
        }
    }
}
