using System.ComponentModel.DataAnnotations;

namespace PatientNoteBackAPI.Models.InputModels
{
    public class NoteInputModel
    {
        [Required]
        public int PatientId { get; set; }
        [Required]
        public string NoteContent { get; set; } = null!;             
    }
}
