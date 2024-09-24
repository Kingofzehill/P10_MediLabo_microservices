using System.Net;

namespace PatientBack.API.Domain
{
    /// <summary>
    /// DTO Patient Class.
    /// </summary>
    /// <remarks></remarks>
    // (UPD001) Sprint 01, US 01 : Patient class.
    public class Patient
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        // (UPD003) Sprint 01, US 01 : one (address) to many (patient) ==> patient address.
        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many#required-one-to-many
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public string? PhoneNumber { get; set; }

        // Format BirthDate "yyyy/MM/dd".
        public string FormatBirthDate()
        {
            return BirthDate.ToString("yyyy/MM/dd");
        }
    }
}
