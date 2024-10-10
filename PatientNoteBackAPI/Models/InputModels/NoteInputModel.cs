namespace PatientNoteBackAPI.Models.InputModels
{
    public class NoteInputModel
    {
        public int PatientId { get; set; }
        public string Content { get; set; } = null!;             
    }
}
