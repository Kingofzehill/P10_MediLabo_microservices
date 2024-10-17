using Microsoft.AspNetCore.Mvc;
using PatientFront.Services;
using Serilog;

namespace PatientFront.Controllers
{
    public class PatientDiabeteController : Controller
    {
        private readonly PatientDiabeteService _patientDiabeteService;

        public PatientDiabeteController(PatientDiabeteService patientDiabeteService)
        {
            _patientDiabeteService = patientDiabeteService;
        }


        /// <summary>PatientFront. PatientDiabete Controller. GetReport method.</summary>  
        /// <param name="id">Patient id.</param>        
        /// <returns>Risk.</returns> 
        /// <remarks>[HttpGet]</remarks>
        [HttpGet]
        public async Task<IActionResult> GetReport(int id)
        {
            try
            {
                Log.Information($"[PatientFront][PatientDiabeteController][HttpGet]. Get diabete Report for Patient id: {id}.", id);
                var riskLevel = await _patientDiabeteService.GetReport(id);

                if (riskLevel == null)
                {
                    Log.Error($"[PatientFront][PatientDiabeteController][HttpGet]. Get diabete Report for Patient id: {id} = null.");
                    TempData["ErrorTitle"] = "Rapport Diabète";
                    TempData["ErrorMessage"] = "La génération du rapport de risque de diabète pour le patient a généré une erreur.";
                    return View("404");
                }

                ViewBag.RiskLevel = riskLevel;

                return View(riskLevel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientFront][PatientDiabeteController][HttpGet] Error on Get diabete Report for Patient id: {id}.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }
    }
}
