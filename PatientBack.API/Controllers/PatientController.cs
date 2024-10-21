using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientBackAPI.Models.InputModels;
using PatientBackAPI.Services;
using Serilog;

namespace PatientBackAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>[HttpGet] Patient API method List: get the list of Patients.</summary>  
        /// <returns>Patients list.</returns> 
        /// <remarks> URI: /Patient/List/.
        /// Access limited to authenticated and authorized User with role Organizer or Practitioner.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>           
        [HttpGet]
        [Route("List")]
        [Authorize(Policy = "OrganizerOrPractitioner")] // Authorize for specific policy (this policy check if user has role Organizer or Practitioner).
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List()
        {            
            try
            {
                Log.Information("[PatientBackAPI][HttpGet] Get patients List.");
                return Ok(await _patientService.List());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientBackAPI] error on Patient List.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                // Returns detailed problem with message and stackTrace.
                //      https://learn.microsoft.com/fr-fr/aspnet/core/web-api/handle-errors?view=aspnetcore-8.0                
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }            
        }

        /// <summary>[HttpGet] Patient API method Get: get a Patient from his id.</summary>  
        /// <param name="id">Patient id.</param>        
        /// <returns>Patient POCO output model object.</returns> 
        /// <remarks> URI: /Patient/Get/.
        /// Access limited to authenticated and authorized User with role Organizer or Practitioner.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>  
        /// <response code ="404">Not found.</response>
        /// <response code ="500">Internal error (exception).</response>
        [HttpGet]
        //[Route("Get/{id}")]
        [Route("Get")]
        [Authorize(Policy = "OrganizerOrPractitioner")] // Authorize for specific policy (this policy check if user has role Organizer or Practitioner).
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            try
            {
                Log.Information("[PatientBackAPI][HttpGet] Get patient from id: {id}.", id);
                var patient = await _patientService.Get(id);

                if (patient is not null)
                {
                    return Ok(patient);
                }
                Log.Warning("[PatientBackAPI] Patient not found (404).");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientBackAPI] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");                                
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>[HttpPost] Patient API method Create: create a Patient.</summary>          
        /// <param name="input">Patient POCO inputmodel object.</param>
        /// <returns>Patient POCO object.</returns> 
        /// <remarks> URI: /Patient/Create/.
        /// Access limited to authenticated and authorized User with role Organizer.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>  
        /// <response code ="500">Internal error (exception).</response>
        [HttpPost]
        [Route("Create")]        
        [Authorize("Organizer")]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] PatientInputModel input)
        {            
            try
            {
                Log.Information("[PatientBackAPI][HttpPost] Create patient {Firstname} {Lastname}.", input.Firstname, input.Lastname);
                return Ok(await _patientService.Create(input));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientBackAPI] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>[HttpPut] Patient API method Update: update a Patient.</summary>  
        /// <param name="id">Patient id.</param>
        /// <param name="input">Patient POCO inputmodel object.</param>
        /// <returns>Patient POCO object.</returns> 
        /// <remarks>URI: /Patient/Update/.
        /// Access limited to authenticated and authorized User with role Organizer.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>  
        /// <response code ="404">Not found.</response>
        /// <response code ="500">Internal error (exception).</response>
        [HttpPut]
        //[Route("Update/{id}")]
        [Route("Update")]
        [Authorize("Organizer")]
        //[Authorize(Roles = "Organizer")] // Authorize for user which has the specified role.
        //[Authorize(Policy = "Organizer")] // Authorize policy (user which has the specified role).
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] PatientInputModel input)
        {
            try
            {
                Log.Information("[PatientBackAPI][HttpPut] Update patient {id} {Firstname} {Lastname}.", id, input.Firstname, input.Lastname);
                var patient = await _patientService.Update(id, input);

                if (patient is not null)
                {
                    return Ok(patient);
                }
                Log.Warning("[PatientBackAPI] Patient not found (404).");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientBackAPI] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>[HttpDelete] Patient API method Delete: delete a Patient.</summary>  
        /// <param name="id">Patient id.</param>        
        /// <returns>Patient POCO object.</returns> 
        /// <remarks>URI: /Patient/Delete/.
        /// Access limited to authenticated and authorized User with role Organizer.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>  
        /// <response code ="404">Not found.</response>
        /// <response code ="500">Internal error (exception).</response>
        [HttpDelete]
        //[Route("Delete/{id}")]
        [Route("Delete")]
        [Authorize("Organizer")]
        //[Authorize(Roles = "Organizer")] // Authorize for user which has the specified role.
        //[Authorize(Policy = "Organizer")] // Authorize policy (user which has the specified role).
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                Log.Information("[PatientBackAPI][HttpDelete] Delete patient {id}.", id);
                var patient = await _patientService.Delete(id);

                if (patient is not null)
                {
                    return Ok(patient);
                }

                Log.Warning("[PatientBackAPI] Patient not found (404).");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientBackAPI] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }
    }
}
