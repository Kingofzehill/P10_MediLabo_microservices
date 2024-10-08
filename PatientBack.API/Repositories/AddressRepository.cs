using Microsoft.EntityFrameworkCore;
using PatientBackAPI.Data;
using PatientBackAPI.Domain;

namespace PatientBackAPI.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly LocalDbContext _context;

        public AddressRepository(LocalDbContext context)
        {
            _context = context;
        }

        /// <summary>Address Repository (CRUD operation). Get by id.</summary>      
        /// <return>Returns Address DTO object.</return> 
        /// <param name="id">Address id.</param>
        /// <remarks></remarks>
        public async Task<Address?> GetById(int id)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(address => address.Id == id);
                return address;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Address Repository (CRUD operation). Get by Name.</summary>      
        /// <return>Returns Address DTO object.</return> 
        /// <param name="addressContent">Address content.</param>
        /// <remarks></remarks>
        public async Task<Address?> GetByName(string addressContent)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(address => address.AddressContent == addressContent);
                return address;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Address Repository (CRUD operation). Create.</summary>      
        /// <return>Returns Address DTO object created.</return> 
        /// <param name="address">Address DTO object.</param>
        /// <remarks></remarks>
        public async Task<Address> Create(Address address)
        {
            try
            {
                var addressFound = await _context.Addresses.FirstOrDefaultAsync(a => a.AddressContent == address.AddressContent);
                if (addressFound is null)
                {
                    await _context.Addresses.AddAsync(address);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    address = addressFound;
                }
                return address;
            }
            catch
            {
                throw;
            }            
        }

        /// <summary>Address Repository (CRUD operation). Update.</summary>      
        /// <return>Returns Address DTO object updated.</return> 
        /// <param name="address">Address DTO object.</param>
        /// <remarks></remarks>
        public async Task<Address?> Update(Address address)
        {
            try
            {
                var addressUpdate = await _context.Addresses.FirstOrDefaultAsync(p => p.Id == address.Id);
                if (addressUpdate is not null)
                {
                    addressUpdate.AddressContent = address.AddressContent;
                    await _context.SaveChangesAsync();
                }
                return addressUpdate;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Address Repository (CRUD operation). Delete.</summary>      
        /// <return>Returns Address DTO object deleted.</return> 
        /// <param name="id">Address id.</param>
        /// <remarks></remarks>
        public async Task<Address?> Delete(int id)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(address => address.Id == id);
                if (address is not null)
                {
                    _context.Addresses.Remove(address);
                    await _context.SaveChangesAsync();
                }
                return address;
            }
            catch
            {
                throw;
            }
        }
    }
}
