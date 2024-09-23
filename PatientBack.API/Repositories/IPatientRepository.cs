using PatientBack.API.Domain;

namespace PatientBack.API.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> List();
    }
}
