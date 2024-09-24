using PatientBack.API.Domain;
using PatientBack.API.Models.InputModels;
using PatientBack.API.Models.OutputModels;

namespace PatientBack.API.Services
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
