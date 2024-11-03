using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace PatientDiabeteRiskBackAPI.Models.OutputModels
{
    public class NoteOutputModel
    {        
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }        
        public int PatientId { get; set; }
        public string NoteContent { get; set; } = null!;
    }
}
