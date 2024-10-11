using PatientNoteBackAPI.Models.InputModels;
using PatientNoteBackAPI.Models.OutputModels;

namespace PatientNoteBackAPI.Services
{
    public interface INoteService
    {
        Task<List<NoteOutputModel>> List(int patientId);
        Task<NoteOutputModel> Create(NoteInputModel input);
    }
}
