using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientDiabeteRiskBackAPI.Models;
using PatientDiabeteRiskBackAPI.Services;
using Serilog;

namespace PatientDiabeteRiskBackAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DiabeteController : ControllerBase
    {
        private readonly DiabeteService _diabeteService;

        public DiabeteController(DiabeteService diabeteService)
        {
            _diabeteService = diabeteService;
        }

        /// <summary>[HttpGet] PatientDiabeteRiskBackAPI. Method Get: evaluate Patient diabete risk.</summary>  
        /// <param name="id">Patient id.</param>        
        /// <returns>Risk.</returns> 
        /// <remarks> URI: /Diabete/Get?id={Id}.
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
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            try
            {
                Log.Information($"[PatientDiabeteRiskBackAPI][HttpGet] Get diabete risk for Patient id: {id}.");
                Risk? risk = await _diabeteService.EvaluateRisk(id); 
                if (risk is null)
                {                
                    Log.Warning($"[PatientDiabeteRiskBackAPI] Get diabete risk for Patient id: {id} not found (404).");
                    return NotFound();
                }
                return Ok(risk);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientDiabeteRiskBackAPI] Internal error (500) occurs on get diabete risk for Patient id: {id}.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }
    }
}
