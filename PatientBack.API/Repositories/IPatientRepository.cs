using PatientBackAPI.Domain;

namespace PatientBackAPI.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> List();
        Task<Patient> Get(int id);
        Task<Patient> Create(Patient patient);
        Task<Patient?> Update(Patient patient);
        Task<Patient?> Delete(int id);
    }
}
