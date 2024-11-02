using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientBackAPI.Domain;
using PatientFront.Services;
using PatientNoteBackAPI.Models.InputModels;
using Serilog;

namespace PatientFront.Controllers
{
    public class PatientNoteController : Controller
    {
        private readonly PatientNoteService _patientNoteService;
        public PatientNoteController(PatientNoteService patientNoteService)
        {
            _patientNoteService = patientNoteService;
        }

        /// <summary>PatientFront. PatientNote Controller. List method.</summary>  
        /// <param name="id">Patient id.</param> 
        /// <returns>Patient Notes List View.</returns> 
        /// <remarks>[HttpGet]</remarks>
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                Log.Information($"[PatientFront][PatientNoteController][List][HttpGet]. Get Patient Notes List.");
                var notes = await _patientNoteService.List(id);

                if (notes == null)
                {                                        
                    Log.Error($"[PatientFront][PatientNoteController][List][HttpGet]. Patients List = null.");
                    TempData["ErrorTitle"] = "Liste des Notes du Patient";
                    TempData["ErrorMessage"] = "Vous n'avez pas les droits pour accéder aux Notes du Patient.";
                    return View("404");
                }
                TempData["PatientId"] = id;
                return View(notes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientFront][PatientNoteController][List] Error on Patient Notes List.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>PatientFront. PatientNote Controller. Create method.
        /// Display Patient Note input model for creation.</summary>                
        /// <returns>Patient Note Create View.</returns> 
        /// <remarks>[HttpGet]</remarks>
        [HttpGet]
        public IActionResult Create(int id)
        {
            TempData["PatientId"] = id;
            return View();
        }

        /// <summary>PatientFront. PatientNote Controller. Create method.</summary>                
        /// Post Patient Note input model for creation.</summary>  
        /// <returns>Patient Note created.</returns> 
        /// <remarks>[HttpPost]</remarks>        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId", "NoteContent")] NoteInputModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var note = await _patientNoteService.Create(input);

                    if (note == null)
                    {
                        Log.Error($"[PatientFront][PatientNoteController][Create][HttpPost]. Patients Note Create = null.");
                        TempData["ErrorTitle"] = "Création d'une Note Patient";
                        TempData["ErrorMessage"] = "Une erreur est survenue durant la création de la note du Patient.";
                        return View("404");
                    }
                    TempData["SuccessMessage"] = "La note du patient a été créée.";
                    TempData["PatientId"] = note.PatientId;
                    //return RedirectToAction(nameof(Index));
                    //return RedirectToAction("Index", "Patient");
                    //return RedirectToAction("Index", "PatientNote");
                    return RedirectToAction("Index", new { id = note.PatientId });
                }
                ModelState.AddModelError(string.Empty, "Les informations fournies sont incomplètes. Veuillez vérifier les informations saisies.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientFront][PatientController][Create] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>PatientFront. PatientNote Controller. Delete method.</summary>  
        /// Displays Patient Note for delete confirmation.</summary>      
        /// <param name="id">Patient Note id (MongoDbB ObjectId type).</param> 
        /// <returns>Patient Note Outputmodel View.</returns> 
        /// <remarks>[HttpGet]</remarks>
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var note = await _patientNoteService.Get(id);

            if (note == null)
            {
                Log.Error($"[PatientFront][PatientNoteController][Delete][HttpGet]. Get Patient Note to delete, objectId: {id} = null.");
                TempData["ErrorTitle"] = "Recherche Note";
                TempData["ErrorMessage"] = "Une erreur est survenue lors de la recherche de la Note du Patient à supprimer.";
                return View("404");
            }
            TempData["PatientId"] = note.PatientId;
            return View(note);
        }

        /// <summary>PatientFront. PatientNote Controller. DeleteConfirmed method.
        /// Post delete of the Patient Note.</summary>                
        /// <param name="id">Patient id.</param>   
        /// <returns>Index view.</returns> 
        /// <remarks>[HttpGett]</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var note = await _patientNoteService.Delete(id);

                if (note == null)
                {
                    Log.Error($"[PatientFront][PatientNoteController][Delete][HttpPost]. Patients Note Delete, objectId: {id} = null.");
                    TempData["ErrorTitle"] = "Suppression Note";
                    TempData["ErrorMessage"] = "Une erreur est survenue lors de la suppression de la Note du Patient.";
                    return View("404");
                }

                TempData["SuccessMessage"] = "La Note du patient a été supprimée.";
                TempData["PatientId"] = note.PatientId;
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", new { id = note.PatientId }); 
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientFront][PatientNoteController][Delete][HttpPost] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);                
            }
        }
    }
}
