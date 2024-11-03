using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PatientNoteBackAPI.Domain
{
    /// <summary>
    /// DTO Note Class.
    /// </summary>
    /// <remarks></remarks>    
    public class Note
    {        
        public ObjectId Id { get; set; }
        public int PatientId { get; set; }                
        public string NoteContent { get; set; }
    }
}
