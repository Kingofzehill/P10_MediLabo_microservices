//using Microsoft.OpenApi.Extensions;
//using NuGet.Common;
//using PatientBackAPI.Services;
using PatientDiabeteRiskBackAPI.Models;
//using PatientNoteBackAPI.Services;
//using System.Collections.Generic;

namespace PatientDiabeteRiskBackAPI.Services
{
    public class DiabeteService
    {
        private readonly PatientNoteService _patientNoteService;
        private readonly PatientService _patientService;
        private string[] triggerTerms = [
            "Hémoglobine A1C",
            "Microalbumine",
            "Taille",
            "Poids",
            "Fumeur",
            "Fumeuse",
            "Anormal",
            "Cholestérol",
            "Vertige",
            "Rechute",
            "Réaction",
            "Anticorps"
        ];

        public DiabeteService(PatientNoteService patientNoteService, PatientService patientService)
        {
            _patientNoteService = patientNoteService;
            _patientService = patientService;
        }

        /// <summary>PatientDiabeteRiskBackAPI. Diabete Service. Get method.
        /// Use PatientService for Patient Get from PatientBackAPI.
        /// Use PatientNoteService for Patient Note Get from PatientNoteBackAPI.</summary>    
        /// <param name="id">Patient id.</param>   
        /// <param name="authorization">Contains token requested for authentication
        /// in PatientBack and Patient API.</param>  
        /// <returns>Risk level (from Risk enum).</returns> 
        /// <remarks></remarks>
        public async Task<Risk?> EvaluateRisk(int id, string authorization)
        {
            // Get patient.
            var patient = await _patientService.Get(id, authorization);
            if (patient == null)
            {
                return null;
            }

            // Get Patient Notes.
            var notes = await _patientNoteService.List(id, authorization);
            if (notes is null)
            {
                return null;
            }

            // Patient age calculation.
            //      https://www.zmaster.fr/informatique_article_227.html
            DateTime todayDate = DateTime.Today;
            int patientAge = todayDate.Year - patient.BirthDate.Year;
            // exact age fix: if birthday is upper than actual year + Patient age, we substract one year.
            if (todayDate < patient.BirthDate.AddYears(patientAge))
            {
                patientAge--;
            }

            // Number of trigger terms found in Patient Notes.
            int nbTriggerTermsOccurence = 0;
            // evaluate number of trigger terms in PatientNotes.
            foreach (var note in notes)
            {
                foreach (var term in triggerTerms)
                {
                    // Ignore note case.
                    //      StringComparaison ==> property treats the characters in the strings to compare as if they were converted to uppercase using the convention/
                    //      https://learn.microsoft.com/fr-fr/dotnet/api/system.stringcomparer.ordinalignorecase?view=net-8.0
                    if (note.NoteContent.Contains(term, StringComparison.OrdinalIgnoreCase))
                    {
                        nbTriggerTermsOccurence++;
                    }
                }
            }

            // evaluate risk level depending of patient age and gender.
            if (string.Equals(patient.Gender, "M"))
            {
                if (patientAge < 30)
                {
                    if (nbTriggerTermsOccurence >= 3)
                    {
                        if (nbTriggerTermsOccurence >= 5)
                        {
                            return Risk.EarlyOnset;
                        }
                        else
                        {
                            return Risk.InDanger;
                        }
                    }
                }
            }

            if (string.Equals(patient.Gender, "F"))
            {
                if (patientAge < 30)
                {
                    if (nbTriggerTermsOccurence >= 4)
                    {
                        if (nbTriggerTermsOccurence >= 7)
                        {
                            return Risk.EarlyOnset;
                        }
                        else
                        {
                            return Risk.InDanger;
                        }
                    }
                }
            }


            if (patientAge > 30)
            {
                if (nbTriggerTermsOccurence >= 2 && nbTriggerTermsOccurence <= 5)
                {
                    return Risk.Borderline;
                }
                if (nbTriggerTermsOccurence == 6 || nbTriggerTermsOccurence == 7)
                {
                    return Risk.InDanger;
                }
                if (nbTriggerTermsOccurence >= 8)
                {
                    return Risk.EarlyOnset;
                }
            }

            // default diabete risk level.
            return Risk.None;
        }
    }
}
