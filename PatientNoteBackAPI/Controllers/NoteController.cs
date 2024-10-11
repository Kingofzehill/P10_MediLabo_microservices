using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientNoteBackAPI.Models.InputModels;
using PatientNoteBackAPI.Services;
using Serilog;

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

        /// <summary>[HttpGet] PatientNotes API method List: get the Patient Notes list.</summary>  
        /// <param name="id">Patient Id.</param>
        /// <returns>Patient Notes list.</returns> 
        /// <remarks>URI: /Notes/List/. Access limited to authenticated and 
        /// authorized User with role Practitioner.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>        
        [HttpGet]
        [Route("List")]
        //[Authorize("Practitioner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List([FromQuery] int id)
        {
            try
            {
                Log.Information("[PatientNotesAPI][HttpGet] Get patient Notes List.");
                return Ok(await _noteService.List(id));
            }
            catch (Exception ex)
            {                
                Log.Error(ex, "[PatientNotesAPI] error on Patient Notes List.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                // Returns detailed problem with message and stackTrace.
                //      https://learn.microsoft.com/fr-fr/aspnet/core/web-api/handle-errors?view=aspnetcore-8.0                
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>[HttpPost] PatientNotes API method Create: create a Patient Note.</summary>  
        /// <param name="input">Patient POCO InputModel object.</param>
        /// <returns>Patient Notes list.</returns> 
        /// <remarks>URI: /Notes/List/. Access limited to authenticated and 
        /// authorized User with role Practitioner.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>       
        /// /// <response code ="500">Internal error (exception).</response>
        [HttpPost]
        [Route("Create")]
        //[Authorize("Practitioner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] NoteInputModel input)
        {
            try
            {
                Log.Information("[PatientNotesAPI][HttpPost] Get patient Notes List.");
                return Ok(await _noteService.Create(input));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientNotesAPI] error on Patient Note create.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                // Returns detailed problem with message and stackTrace.
                //      https://learn.microsoft.com/fr-fr/aspnet/core/web-api/handle-errors?view=aspnetcore-8.0                
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }
    }
}
