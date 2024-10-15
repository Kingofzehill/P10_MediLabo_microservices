using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
        /// <returns>Note Output model objects list.</returns> 
        /// <remarks>URI: /Note/List?id={Id}. Access limited to authenticated and 
        /// authorized User with role Practitioner.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>        
        [HttpGet]
        [Route("List")]
        [Authorize("Practitioner")]
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

        /// <summary>[HttpGet] Patient Note API method Get: get a Patient Note from his id.</summary>  
        /// <param name="id">Patient Note id (MongoDB ObjectId type).</param>        
        /// <returns>Patient Note POCO output model object.</returns> 
        /// <remarks> URI: /Note/Get?id={Id}..
        /// Access limited to authenticated and authorized User with role Practitioner.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>  
        /// <response code ="404">Not found.</response>
        /// <response code ="500">Internal error (exception).</response>
        [HttpGet]        
        [Route("Get")]
        [Authorize("Practitioner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] string id)
        {
            try
            {
                Log.Information("[PatientNoteBackAPI][HttpGet] Get patient Note from ObjectId: {id}.", id);
                var note = await _noteService.Get(id);

                if (note is not null)
                {
                    return Ok(note);
                }
                Log.Warning("[PatientNoteBackAPI] Patient Note not found (404).");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientNoteBackAPI] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>[HttpPost] PatientNotes API method Create: create a Patient Note.</summary>  
        /// <param name="input">Note POCO InputModel object.</param>
        /// <returns>Note Outuput Model object.</returns> 
        /// <remarks>URI: /Notes/Create/. Access limited to authenticated and 
        /// authorized User with role Practitioner.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>       
        /// <response code ="500">Internal error (exception).</response>
        [HttpPost]
        [Route("Create")]
        [Authorize("Practitioner")]
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

        /// <summary>[HttpDelete] Patient Note API method Delete: delete a Patient Note.</summary>  
        /// <param name="id">Note Id (MongoDB Object ID type).</param> 
        /// <returns>Note Outuput Model object.</returns>        
        /// <remarks>URI: /Note/Delete?id={Id}.
        /// Access limited to authenticated and authorized User with role Practitionner.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>  
        /// <response code ="404">Not found.</response>
        /// <response code ="500">Internal error (exception).</response>
        //[HttpDelete("{id:length(24)}")]
        [HttpDelete]
        [Route("Delete")]
        [Authorize("Practitioner")]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromQuery] string id) // {670d136f831d2836be4d400a}
        {
            try
            {
                Log.Information("[PatientNoteBackAPI][HttpDelete] Delete Patient Note, objectId: {id}.", id);
                var note = await _noteService.Delete(id);

                if (note is not null)
                {
                    return Ok(note);
                }

                Log.Warning("[PatientNoteBackAPI] Patient Note not found (404).");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientNoteBackAPI] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }
    }
}
