using Microsoft.EntityFrameworkCore;
using PatientBack.API.Data;
using PatientBack.API.Domain;

namespace PatientBack.API.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly LocalDbContext _context;

        public PatientRepository(LocalDbContext context)
        {
            _context = context;
        }

        /// <summary>Patient Repository (CRUD operation). List.</summary>      
        /// <return>List of Patients DTO objects.</return>         
        /// <remarks></remarks>
        public async Task<List<Patient>> List() => await Task.Run(() => _context.Patients.Include(p => p.Address).ToListAsync());

        /// <summary>Patient Repository (CRUD operation). Get by id.</summary>      
        /// <return>Returns Patient DTO object.</return> 
        /// <param name="id">Patient id to get.</param>
        /// <remarks></remarks>
        public async Task<Patient> Get(int id) => await _context.Patients.FindAsync(id);

        /// <summary>Patient Repository (CRUD operation). Creation.</summary>      
        /// <return>Returns Patient DTO object.</return> 
        /// <param name="patient">Patient DTO object.</param>
        /// <remarks></remarks>
        public async Task<Patient> Create(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        /// <summary>Patient Repository (CRUD operation). Update.</summary>      
        /// <return>Returns Patient DTO object.</return> 
        /// <param name="Patient">Patient DTO object.</param>
        /// <remarks></remarks>
        public async Task<Patient?> Update(Patient patient)
        {
            var patientUpdated = await _context.Patients.Include(p => p.Address).FirstOrDefaultAsync(c => c.Id == patient.Id);

            if (patientUpdated is not null)
            {
                patientUpdated.Firstname = patient.Firstname;
                patientUpdated.Lastname = patient.Lastname;
                patientUpdated.BirthDate = patient.BirthDate;
                patientUpdated.Gender = patient.Gender;
                if (patientUpdated.Address is not null)
                {
                    patientUpdated.Address = patient.Address;
                }
                if (patientUpdated.PhoneNumber is not null)
                {
                    patientUpdated.PhoneNumber = patient.PhoneNumber;
                }
                await _context.SaveChangesAsync();
            }
            return patientUpdated;
        }

        /// <summary>Patient Repository (CRUD operation). Delete.</summary>      
        /// <return>Returns Patient DTO object.</return> 
        /// <param name="id">Patient DTO id.</param>
        /// <remarks></remarks>
        public async Task<Patient?> Delete(int id)
        {
            var Patient = await _context.Patients.Include(p => p.Address).FirstOrDefaultAsync(p => p.Id == id);

            if (Patient != null)
            {
                _context.Patients.Remove(Patient);
                await _context.SaveChangesAsync();
            }
            return Patient;
        }
    }
}
