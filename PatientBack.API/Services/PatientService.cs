using PatientBack.API.Domain;
using PatientBack.API.Models.OutputModels;
using PatientBack.API.Repositories;

namespace PatientBack.API.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAddressRepository _addressRepository;

        public PatientService(IPatientRepository patientRepository, IAddressRepository addressRepository)
        {
            _patientRepository = patientRepository;
            _addressRepository = addressRepository;
        }

        private PatientOutputModel ToOutputModel(Patient patient)
        {
            var output = new PatientOutputModel()
            {
                Id = patient.Id,
                FirstName = patient.Firstname,
                LastName = patient.Lastname,
                DateOfBirth = patient.BirthDate,
                Gender = patient.Gender,
            };

            if (patient.Address is not null)
            {
                output.Address = patient.Address.AddressContent;
            }
            if (patient.PhoneNumber is not null)
            {
                output.PhoneNumber = patient.PhoneNumber;
            }
            return output;
        }

        public async Task<List<PatientOutputModel>> List()
        {
            try
            {
                var list = new List<PatientOutputModel>();
                var Patients = await _patientRepository.List();

                foreach (var Patient in Patients)
                {
                    list.Add(ToOutputModel(Patient));
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

    }
}
