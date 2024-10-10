using MongoDB.Bson;

namespace PatientNoteBackAPI.Models.OutputModels
{
    public class NoteOutputModel
    {
        public string? Id { get; set; }
        public int PatientId { get; set; }
        public string Content { get; set; } = null!;        
    }
}
