using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientBackAPI.Domain;
using PatientFront.Services;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using PatientBackAPI.Models.InputModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using PatientBackAPI.Models.OutputModels;

namespace PatientFront.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>PatientFront. Patient Controller. List method.</summary>  
        /// <returns>Patients List View.</returns> 
        /// <remarks>[HttpGet]</remarks>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                Log.Information($"[PatientFront][PatientController][List][HttpGet]. Get Patients List.");
                var patients = await _patientService.List();
                if (patients == null)
                {
                    Log.Error($"[PatientFront][PatientController][List][HttpGet]. Patients List = null.");
                    TempData["ErrorTitle"] = "Liste Patients";
                    TempData["ErrorMessage"] = "La liste des patients a généré une erreur.";
                    return View("404");
                }

                return View(patients);
            }
            catch (Exception ex)
            {                
                Log.Error(ex, $"[PatientFront][PatientController][List] Error on Patient List.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");                
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>PatientFront. Patient Controller. Get method.</summary>  
        /// <param name="id">Patient id.</param>        
        /// <returns>Patient View.</returns> 
        /// <remarks>[HttpGet]</remarks>
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                Log.Information($"[PatientFront][Controller][Get][HttpGet]. Get Patient {id}.", id);
                var patient = await _patientService.Get(id);
                if (patient == null)
                {
                    Log.Error($"[PatientFront][PatientController][Get][HttpGet]. Patients Get = null.");
                    TempData["ErrorTitle"] = "Détail Patient";
                    TempData["ErrorMessage"] = "La recherche du patient a généré une erreur.";
                    return View("404");
                }

                return View(patient);
            }
            catch (Exception ex)
            {                
                Log.Error(ex, $"[PatientFront][PatientController][Get] Error on Patient Get.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");                
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }
        }

        /// <summary>PatientFront. Patient Controller. Create method.
        /// Display Patient input model for creation.</summary>                
        /// <returns>Patient Create View.</returns> 
        /// <remarks>[HttpGet]</remarks>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        /*public async Task<IActionResult> Create()
        {
            //return View(nameof(Create));
            var inputModel = new PatientInputModel
            {
                Firstname = "",
                Lastname = "",
                //BirthDate = new DateTime(),
                BirthDate = DateTime.Now,
                Gender = "",
                Address = "",
                PhoneNumber = ""
            };
            return View(inputModel);*/
            /*var outputModel = new PatientOutputModel
            {                
                Firstname = "",
                Lastname = "",
                BirthDate = DateTime.Now,
                Gender = "",
                Address = "",
                PhoneNumber = ""
            };
            return View(outputModel);*/
        //}

        /// <summary>PatientFront. Patient Controller. Create method.</summary>                
        /// Post Patient input model for creation.</summary>  
        /// <returns>Patient created.</returns> 
        /// <remarks>[HttpPost]</remarks>        
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Firstname, Lastname, BirthDate, Gender, Address, PhoneNumber")] PatientInputModel input)
        //public async Task<IActionResult> Create(PatientInputModel input)                
        //public async Task<IActionResult> Create(PatientOutputModel input)
        public async Task<IActionResult> Create([Bind("Firstname, Lastname, BirthDate, Gender, Address, PhoneNumber")] PatientInputModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    /*var patientInputModel = new PatientInputModel
                    {
                        Firstname = input.Firstname,
                        Lastname = input.Lastname,
                        BirthDate = input.BirthDate,
                        Gender = input.Gender,
                        Address = input.Address,
                        PhoneNumber = input.PhoneNumber
                    };
                    var patient = await _patientService.Create(patientInputModel);*/
                    var patient = await _patientService.Create(input);
                    if (patient == null)
                    {
                        Log.Error($"[PatientFront][PatientController][Create][HttpPost]. Patients Create = null.");
                        TempData["ErrorTitle"] = "Création Patient";
                        TempData["ErrorMessage"] = "Une erreur est survenue durant la création du Patient.";
                        return View("404");
                    }
                    
                    TempData["SuccessMessage"] = "Le patient "+patient.Firstname+" "+patient.Lastname+" a été créé.";                    
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError(string.Empty, "Les informations fournies sont incomplètes. Veuillez vérifier les informations saisies.");
                return View(nameof(Create));                
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

        /// <summary>PatientFront. Patient Controller. Update method.
        /// Get the Patient for the id in entry.</summary>                
        /// <param name="id">Patient id.</param>   
        /// <returns>Patient input model.</returns> 
        /// <remarks>[HttpGet]</remarks>
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                if (id == 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                var patient = await _patientService.Get(id);
                if (patient == null)
                {
                    Log.Error($"[PatientFront][PatientController][Update][HttpGet]. Patients get, id: {id} = null.");
                    TempData["ErrorTitle"] = "Recherche Patient pour mise à jour";
                    TempData["ErrorMessage"] = "Une erreur est survenue sur la recherche du Patient à mettre à jour.";
                    return View("404");
                }

                /*var inputModel = new PatientInputModel
                // Store Patient Id for update.                
                TempData["patientId"] = id;
                {
                    Firstname = patient.Firstname,
                    Lastname = patient.Lastname,
                    BirthDate = patient.BirthDate,
                    Gender = patient.Gender,
                    Address = patient.Address,
                    PhoneNumber = patient.PhoneNumber
                };
                return View(inputModel);*/
                var outputModel = new PatientOutputModel
                {
                    Id = patient.Id,
                    Firstname = patient.Firstname,
                    Lastname = patient.Lastname,
                    BirthDate = patient.BirthDate,
                    Gender = patient.Gender,
                    Address = patient.Address,
                    PhoneNumber = patient.PhoneNumber
                };
                return View(outputModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, "[PatientFront][PatientController][Update][HttpGet] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
                //return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>PatientFront. Patient Controller. Update method.
        /// Update the Patient with InputModel fields filled..</summary>                
        /// <param name="id">Patient id.</param>   
        /// <returns>Patient (output model) updated.</returns> 
        /// <remarks>[HttpPost]</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(int id, PatientInputModel input)
        public async Task<IActionResult> Update(int id, PatientOutputModel input)
        {
            try
            {                
                if (ModelState.IsValid)
                {                    
                    var patientInputModel = new PatientInputModel
                    {                        
                        Firstname = input.Firstname,
                        Lastname = input.Lastname,
                        BirthDate = input.BirthDate,
                        Gender = input.Gender,
                        Address = input.Address,
                        PhoneNumber = input.PhoneNumber
                    };
                    //var patient = await _patientService.Update(id, input);
                    var patient = await _patientService.Update(input.Id, patientInputModel);
                    if (patient == null)
                    {
                        Log.Error($"[PatientFront][PatientController][Update][HttpPost]. Patients Update, id: {id} = null.");
                        TempData["ErrorTitle"] = "Mise à jour Patient";
                        TempData["ErrorMessage"] = "Une erreur est survenue lors de la mise à jour du Patient.";
                        return View("404");
                    }
                    
                    TempData["SuccessMessage"] = "Le patient "+patient.Firstname+" "+patient.Lastname+" a été mis à jour.";
                    return RedirectToAction(nameof(Index));
                }
                return View(input);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientFront][PatientController][Update][HttpPost] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
                //return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>PatientFront. Patient Controller. Delete method.
        /// Displays Patient for delete confirmation.</summary>                
        /// <param name="id">Patient id.</param>   
        /// <returns>Patient output model.</returns> 
        /// <remarks>[HttpGett]</remarks>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientService.Get(id);

            if (patient == null)
            {
                Log.Error($"[PatientFront][PatientController][Delete][HttpGet]. Get Patients to delete, id: {id} = null.");
                TempData["ErrorTitle"] = "Recherche Patient pour suppression";
                TempData["ErrorMessage"] = "Une erreur est survenue lors de la recherche du Patient à supprimer.";
                return View("404");
            }

            return View(patient);
        }

        /// <summary>PatientFront. Patient Controller. DeleteConfirmed method.
        /// Post delete of the Patient.</summary>                
        /// <param name="id">Patient id.</param>   
        /// <returns>Index view.</returns> 
        /// <remarks>[HttpGett]</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var patient = await _patientService.Delete(id);

                if (patient == null)
                {
                    Log.Error($"[PatientFront][PatientController][Delete][HttpPost]. Patients Delete, id: {id} = null.");
                    TempData["ErrorTitle"] = "Suppression du Patient";
                    TempData["ErrorMessage"] = "Une erreur est survenue lors de la suppression du Patient.";
                    return View("404");
                }
                
                TempData["SuccessMessage"] = "Le patient "+patient.Firstname+" "+patient.Lastname+ " a été supprimé.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientFront][PatientController][Delete][HttpPost] Internal error (500) occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);              
            }
        }
    }
}
