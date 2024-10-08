using PatientBackAPI.Domain;
using PatientBackAPI.Models.InputModels;
using PatientBackAPI.Models.OutputModels;
using PatientBackAPI.Repositories;

namespace PatientBackAPI.Services
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

        /// <summary>Patient Service (business rules). ToOutputModel. 
        /// Load Bidlist DTO object into POCO output model object.</summary>  
        /// <param name="bidList">Patient DTO object.</param>
        /// <remarks></remarks>
        private async Task<PatientOutputModel> ToOutputModel(Patient patient)
        {
            var output = new PatientOutputModel()
            {
                Id = patient.Id,
                Firstname = patient.Firstname,
                Lastname = patient.Lastname,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
            };
            
            // Postal address is optionnal.
            //if (patient.Address is not null)
            if (patient.AddressId.HasValue)
            {                
                var address = await _addressRepository.GetById((int)patient.AddressId.Value);                
                if (address != null)
                {
                    output.Address = address.AddressContent;
                }
            }

            // Phone number is optionnal.
            if (patient.PhoneNumber is not null)
            {
                output.PhoneNumber = patient.PhoneNumber;
            }

            return output;
        }

        /// <summary>Patient Service (Business rules). Entity private method.
        /// Create a Patient DTO object and Patient Address DTO object</summary>      
        /// <return>Returns POCO object of the created Patient.</return> 
        /// <param name="id">Patient id.</param>
        /// <param name="input">POCO Patient object.</param>
        /// <remarks></remarks>
        private async Task<Patient> PocoToDtoAndControls(PatientInputModel input, int id)
        {
            var patient = new Patient()
            {
                Id = id,
                Firstname = input.Firstname,
                Lastname = input.Lastname,
                BirthDate = input.BirthDate,
                Gender = input.Gender,
            };

            if (input.Address is not null)
            {
                // (TODO03) manage create address.
                var address = await _addressRepository.GetByName(input.Address);
                if (address == null)
                {
                    // Create.
                    var addressCreated = await _addressRepository.Create(new Address { AddressContent = input.Address });
                    patient.AddressId = addressCreated.Id;
                    patient.Address = addressCreated;
                    /*// Update.
                    var addressUpdated = await _addressRepository.Update(new Address { AddressContent = input.Address });
                    patient.Address = addressUpdated;
                    patient.AddressId = addressUpdated.Id;*/
                }        
                else
                {
                    patient.AddressId = address.Id;
                    patient.Address = address;
                }
            }

            if (input.PhoneNumber is not null)
            {
                patient.PhoneNumber = input.PhoneNumber;
            }
            return patient;
        }

        /// <summary>Patient Service (business rules). List.</summary>      
        /// <return>List of Patients POCO objects.</return>         
        /// <remarks></remarks>
        public async Task<List<PatientOutputModel>> List()
        {
            try
            {
                var list = new List<PatientOutputModel>();
                var Patients = await _patientRepository.List();

                foreach (var Patient in Patients)
                {
                    list.Add(await ToOutputModel(Patient));
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Patient Service (Business rules). Get by id.</summary>      
        /// <return>Returns POCO object of the Patient.</return> 
        /// <param name="id">Patient.</param>
        /// <remarks></remarks>
        public async Task<PatientOutputModel> Get(int id)
        {
            try
            {
                var patient = await _patientRepository.Get(id);

                if (patient is not null)
                {
                    var outputPatient = await ToOutputModel(patient);

                    // Get addressContent trough ID.
                    if (patient.AddressId.HasValue)
                    {
                        var address = await _addressRepository.GetById(patient.AddressId.Value);
                        if (address != null)
                        {
                            outputPatient.Address = address.AddressContent;
                        }
                    }

                    return outputPatient;
                }
                return null;
            }
            catch
            {
                throw;
            }            
        }

        /// <summary>Patient Service (Business rules). Creation.</summary>      
        /// <return>Returns POCO object of the created Patient.</return> 
        /// <param name="input">POCO Patient object.</param>
        /// <remarks></remarks>
        public async Task<PatientOutputModel> Create(PatientInputModel input)
        {
            try
            {
                var patient = await _patientRepository.Create(await PocoToDtoAndControls(input, 0));
                return await ToOutputModel(patient);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Patient Service (Business rules). Update.</summary>      
        /// <return>Returns POCO object of the updated Patient.</return> 
        /// <param name="id">Patient id</param>
        /// <param name="input">POCO Patient object.</param>
        /// <remarks></remarks>
        public async Task<PatientOutputModel?> Update(int id, PatientInputModel input)
        {
            try
            {
                var patientUpdated = await _patientRepository.Update(await PocoToDtoAndControls(input, id));

                if (patientUpdated is not null)
                {
                    return await ToOutputModel(patientUpdated);
                }
                return null;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>Patient Service (Business rules). Delete.</summary>      
        /// <return>Returns POCO object of the deleted Patient.</return> 
        /// <param name="id">Patient id</param>        
        /// <remarks></remarks>
        public async Task<PatientOutputModel?> Delete(int id)
        {
            try
            {
                // (TODO04) Manage address delete
                // Get patient with ID, and use address ID for delete.
                var patientCheck = await _patientRepository.Get(id);
                if (patientCheck is not null)
                {
                    if (patientCheck.AddressId is not null)
                    {
                        var addressDeleted = await _addressRepository.Delete((int)patientCheck.AddressId);
                    }
                }

                var patient = await _patientRepository.Delete(id);

                if (patient is null)
                {
                    return null;
                }
                return await ToOutputModel(patientCheck);
            }
            catch
            {
                throw;
            }
        }

    }
}
