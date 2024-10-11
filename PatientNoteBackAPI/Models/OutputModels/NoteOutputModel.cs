using MongoDB.Bson;

namespace PatientNoteBackAPI.Models.OutputModels
{
    public class NoteOutputModel
    {
        //public string? Id { get; set; }
        public ObjectId Id { get; set; }
        public int PatientId { get; set; }
        public string NoteContent { get; set; } = null!;        
    }
}
