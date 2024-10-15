using MongoDB.Bson;
using PatientNoteBackAPI.Models.InputModels;
using PatientNoteBackAPI.Models.OutputModels;

namespace PatientNoteBackAPI.Services
{
    public interface INoteService
    {
        Task<List<NoteOutputModel>> List(int patientId);
        Task<NoteOutputModel?> Get(string id);
        Task<NoteOutputModel> Create(NoteInputModel input);
        Task<NoteOutputModel?> Delete(string id);
    }
}
