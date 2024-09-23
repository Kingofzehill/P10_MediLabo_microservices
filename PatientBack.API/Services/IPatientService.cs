using PatientBack.API.Models.OutputModels;

namespace PatientBack.API.Services
{
    public interface IPatientService
    {
        Task<List<PatientOutputModel>> List();     
    }
}
