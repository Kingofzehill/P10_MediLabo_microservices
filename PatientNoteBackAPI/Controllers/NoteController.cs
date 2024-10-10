using Microsoft.AspNetCore.Mvc;
using PatientNoteBackAPI.Services;

namespace PatientNoteBackAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NoteController : ControllerBase // ControllerBase: without views support.
    {
        private readonly INoteService _noteService;
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }
    }
}
