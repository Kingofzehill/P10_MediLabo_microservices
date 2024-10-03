using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientBackAPI.Data;
using PatientBackAPI.Domain;
using PatientBackAPI.Models;
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


        /// <summary>API method: Get a list of Patient items.</summary>  
        /// <returns>Patients list.</returns> 
        /// <remarks>[HttpGet] Patients controller List method.
        /// Access limited to authenticated and authorized User with role Organizer or Practioner.
        /// URI: /Patient/List/.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>           
        [HttpGet]
        [Route("List")]
        [Authorize("OrganizerOrPractitioner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List()
        {            
            try
            {
                Log.Information("[HttpGet] /Patient/. Get patients List.");
                return Ok(await _patientService.List());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                // Returns detailed problem with message and stackTrace.
                //      https://learn.microsoft.com/fr-fr/aspnet/core/web-api/handle-errors?view=aspnetcore-8.0
                //var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
                //return Problem();
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }            
        }

        /// <summary>API method: Get a Patient from his id.</summary>  
        /// <returns>Patient POCO output model object.</returns> 
        /// <remarks>[HttpGet] Patients controller Get method.
        /// Access limited to authenticated and authorized User with role Organizer or Practioner.
        /// URI: /Patient/Get/.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>  
        /// <response code ="404">Not found.</response>
        /// <response code ="500">Internal error (exception).</response>
        [HttpGet]
        //[Route("Get/{id}")]
        [Route("Get")]
        [Authorize("OrganizerOrPractitioner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            try
            {
                Log.Information("[HttpGet] Patient/{id}. Get patient from id: {UserName}.", id);
                var patient = await _patientService.Get(id);

                if (patient is not null)
                {
                    return Ok(patient);
                }
                Log.Warning("Patient not found (404).");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");                                
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>API method: create a Patient.</summary>  
        /// <returns>Patient POCO object.</returns> 
        /// <remarks>[HttpGet] Patients controller Create method.
        /// Access limited to authenticated and authorized User with role Organizer.
        /// URI: /Patient/Create/.</remarks>
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
                Log.Information("[HttpPost] Patient/. Create patient {Firstname} {Lastname}.", input.Firstname, input.Lastname);
                return Ok(await _patientService.Create(input));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>API method: update a Patient.</summary>  
        /// <returns>Patient POCO object.</returns> 
        /// <param name="id">Patient id.</param>
        /// <param name="input">Patient POCO inputmodel object.</param>
        /// <remarks>[HttpGet] Patients controller Update method.
        /// Access limited to authenticated and authorized User with role Organizer.
        /// URI: /Patient/Update/.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>  
        /// <response code ="404">Not found.</response>
        /// <response code ="500">Internal error (exception).</response>
        [HttpPut]
        //[Route("Update/{id}")]
        [Route("Update")]
        [Authorize("Organizer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] PatientInputModel input)
        {
            try
            {
                Log.Information("[HttpPut] Patient/. Update patient {id} {Firstname} {Lastname}.", id, input.Firstname, input.Lastname);
                var patient = await _patientService.Update(id, input);

                if (patient is not null)
                {
                    return Ok(patient);
                }
                Log.Warning("Patient not found (404).");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>API method: delete a Patient.</summary>  
        /// <returns>Patient POCO object.</returns> 
        /// <param name="id">Patient id.</param>        
        /// <remarks>[HttpGet] Patients controller Delete method.
        /// Access limited to authenticated and authorized User with role Organizer.
        /// URI: /Patient/Delete/{id}.</remarks>
        /// <response code ="200">OK.</response>    
        /// <response code ="401">Unauthorized.</response>  
        /// <response code ="404">Not found.</response>
        /// <response code ="500">Internal error (exception).</response>
        [HttpDelete]
        //[Route("Delete/{id}")]
        [Route("Delete")]
        [Authorize("Organizer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                Log.Information("[HttpDelete] Patient/. Delete patient {id}.", id);
                var patient = await _patientService.Delete(id);

                if (patient is not null)
                {
                    return Ok(patient);
                }

                Log.Warning("Patient not found (404).");
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }
    }
}
