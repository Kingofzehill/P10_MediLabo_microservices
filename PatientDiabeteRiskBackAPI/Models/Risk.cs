using System.ComponentModel.DataAnnotations;
using System.Reflection;

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
        [Display(Name = "apparition précoce")]
        EarlyOnset
    }

    // Enum extension method: allows to get displayname of the enum value in view.
    //      https://stackoverflow.com/questions/40888865/cant-get-enum-display-name-property-in-razor-view/40889786
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
}
