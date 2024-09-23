using System;
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


        /// <summary>Get a list of Patient items.</summary>  
        /// <returns>Patients list.</returns> 
        /// <remarks>[HttpGet] Patients controller List method.
        /// Access limited to authenticated and authorized User with role Organizer or Practioner.
        /// URI: /Patient/List.</remarks>
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
                Log.Information("[HttpGet] patients List.");
                return Ok(await _patientService.List());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Internal error (500) occurs on Patient List.");
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
