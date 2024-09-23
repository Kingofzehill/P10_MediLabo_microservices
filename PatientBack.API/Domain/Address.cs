namespace PatientBack.API.Domain
{
    /// <summary>
    /// DTO Address Class.
    /// </summary>
    /// <remarks></remarks>
    // (UPD002) Sprint 01, US 01 : Address class.
    public class Address
    {
        public int Id { get; set; }
        public string AddressContent { get; set; }
        // (UPD004) Sprint 01, US 01 : one (address) to many (patient) ==> patients for one address.
        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many#required-one-to-many
        public ICollection<Patient> Patients { get; set; }
    }
}
