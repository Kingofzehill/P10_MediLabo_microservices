namespace PatientBackAPI.Models.InputModels
{
    /// <summary>
    /// POCO input Patient Class.
    /// </summary>
    /// <remarks></remarks>
    public class PatientInputModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
