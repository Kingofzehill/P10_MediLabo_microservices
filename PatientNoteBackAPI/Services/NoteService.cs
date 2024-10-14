using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
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
            var objectId = (ObjectId)note.Id;
            var strId = (objectId != ObjectId.Empty ? objectId.ToString() : string.Empty);
            return new NoteOutputModel
            {
                //Id = note.Id,
                
                Id = strId,
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
                notes = notes.OrderByDescending(n => n.Id.CreationTime).ToList();
                var output = new List<NoteOutputModel>();
                foreach (var note in notes)
                {
                    output.Add(ToOutputModel(note));
                }
                return output;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Patient Note Service (Business rules). Get.</summary>      
        /// <param name="id">Object Id (mongoDb Id) of Note to get.</param>
        /// <return>Returns POCO object of the Patient Note.</return>         
        /// <remarks></remarks>
        public async Task<NoteOutputModel?> Get(string id)
        {
            try
            {
                var note = await _repositoryNote.Get(id);
                if (note is not null)
                {
                    return ToOutputModel(note);
                }
                return null;
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

        /// <summary>Patient Note Service (Business rules). Delete.</summary>      
        /// <param name="id">Object Id (mongoDb Id) of Note to delete.</param>
        /// <return>Returns POCO object of the created Patient Note.</return>         
        /// <remarks></remarks>
        public async Task<NoteOutputModel?> Delete(string id)
        {
            try
            {
                var note = await _repositoryNote.Delete(id);
                if (note is null)
                {
                    return null;
                }
                else
                { 
                    return ToOutputModel(note);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
