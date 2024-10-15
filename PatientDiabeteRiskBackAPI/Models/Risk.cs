using System.ComponentModel.DataAnnotations;

namespace PatientDiabeteRiskBackAPI.Models
{
    public enum Risk
    {
        [Display(Name = "aucun risque")]
        None,
        [Display(Name = "risque limité")]
        Borderline,
        [Display(Name = "danger")]
        InDanger,
        [Display(Name = "apparition")]
        EarlyOnset
    }
}
