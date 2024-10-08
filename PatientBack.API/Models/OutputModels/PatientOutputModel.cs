namespace PatientBackAPI.Models.OutputModels
{
    /// <summary>
    /// POCO output Patient Class.
    /// </summary>
    /// <remarks></remarks>
    public class PatientOutputModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
