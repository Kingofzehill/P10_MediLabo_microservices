namespace PatientBack.API.Domain
{
    // (UPD002) Sprint 01, US 01 : Address class.
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // (UPD004) Sprint 01, US 01 : one (address) to many (patient) ==> patients for one address.
        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many#required-one-to-many
        public ICollection<Patient> Patients { get; set; }
    }
}
