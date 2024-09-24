﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientBack.API.Data;
using PatientBack.API.Domain;
using PatientBack.API.Models;
using PatientBack.API.Models.InputModels;
using PatientBack.API.Services;
using Serilog;

namespace PatientBack.API.Controllers
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

        /*
        /// GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatient()
        {
            return await _context.Patient.ToListAsync();
        }
        
         // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patient.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            _context.Patient.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }*/
    }
}
