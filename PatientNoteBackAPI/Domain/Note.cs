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
        //[BsonId]
        // Annotated with[BsonId] to make this property the document's primary key.
        //[BsonRepresentation(BsonType.ObjectId)]        
        // Annotated with[BsonRepresentation(BsonType.ObjectId)] to allow passing the parameter as type string instead of an ObjectId structure.Mongo handles the conversion from string to ObjectId.
        //public string? Id { get; set; }
        // ObjectId: mongoDb Id allows to use id properties such as creationTime.
        public ObjectId Id { get; set; }
        public int PatientId { get; set; }
        //[BsonElement("NoteContent")]
        // The NoteContent property is annotated with the [BsonElement] attribute. The attribute's value of Name represents the property name in the MongoDB collection.
        public string NoteContent { get; set; }
    }
}
