using PatientBack.API.Data;

namespace PatientBack.API.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly LocalDbContext _context;

        public AddressRepository(LocalDbContext context)
        {
            _context = context;
        }
    }
}
