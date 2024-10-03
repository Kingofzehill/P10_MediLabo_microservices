using PatientBackAPI.Domain;
using PatientBackAPI.Models.InputModels;
using PatientBackAPI.Models.OutputModels;

namespace PatientBackAPI.Services
{
    public interface IPatientService
    {
        Task<List<PatientOutputModel>> List();
        Task<PatientOutputModel> Get(int id);
        Task<PatientOutputModel> Create(PatientInputModel input);
        Task<PatientOutputModel?> Update(int id, PatientInputModel input);
        Task<PatientOutputModel?> Delete(int id); 
    }
}
