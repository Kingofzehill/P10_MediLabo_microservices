namespace PatientBack.API.Models.OutputModels
{
    /// <summary>
    /// POCO output Patient Class.
    /// </summary>
    /// <remarks></remarks>
    public class PatientOutputModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
