using PatientNoteBackAPI.Domain;
using PatientNoteBackAPI.Models.InputModels;
using PatientNoteBackAPI.Models.OutputModels;
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

        /// <summary>Patient Service (business rules). ToOutputModel. 
        /// Load Note DTO object into POCO NoteOutputModel object.</summary>  
        /// <param name="note">Patient Note DTO object.</param>
        /// <remarks></remarks>
        private NoteOutputModel ToOutputModel(Note note)
        {
            return new NoteOutputModel
            {
                Id = note.Id,
                NoteContent = note.NoteContent,
                PatientId = note.PatientId,
            };
        }

        /// <summary>Patient Note Service (business rules). List.</summary>      
        /// <return>List of Patient Notes POCO objects.</return>         
        /// <remarks></remarks>
        public async Task<List<NoteOutputModel>> List(int patientId)
        {
            try
            {
                var notes = await _repositoryNote.List(patientId);
                var output = new List<NoteOutputModel>();
                foreach (var note in notes)
                {
                    output.Add(ToOutputModel(note));
                }
                return output.OrderByDescending(n => n.Id.CreationTime).ToList();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Patient Note Service (Business rules). Creation.</summary>      
        /// <param name="input">POCO InputModel Patient Note object.</param>
        /// <return>Returns POCO object of the created Patient Note.</return>         
        /// <remarks></remarks>
        public async Task<NoteOutputModel> Create(NoteInputModel input)
        {
            try
            {
                return ToOutputModel(await _repositoryNote.Create(new Note
                {
                    NoteContent = input.NoteContent,
                    PatientId = input.PatientId,
                }));
            }
            catch
            {
                throw;
            }
        }
    }
}
