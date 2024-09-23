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

        // List of patients. Get Patient Address using navigation link (include).
        public async Task<List<Patient>> List() => await Task.Run(() => _context.Patients.Include(p => p.Address).ToListAsync());
    }
}
