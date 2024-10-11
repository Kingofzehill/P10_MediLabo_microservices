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
                    TempData["ErrorMessage"] = "La liste des Notes du Patient a généré une erreur.";
                    return View("404");
                }
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
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>PatientFront. PatientNote Controller. Create method.</summary>                
        /// Post Patient Note input model for creation.</summary>  
        /// <returns>Patient Note created.</returns> 
        /// <remarks>[HttpPost]</remarks>        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId", "Content")] NoteInputModel input)
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
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "Patient");
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

        /*// GET: PatientNotesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PatientNotesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PatientNotesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientNotesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatientNotesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PatientNotesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatientNotesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientNotesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
