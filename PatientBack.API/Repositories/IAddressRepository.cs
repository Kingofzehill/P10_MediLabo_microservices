using PatientBack.API.Domain;

namespace PatientBack.API.Repositories
{
    public interface IAddressRepository
    {
        Task<Address?> GetById(int id);
        Task<Address?> GetByName(string name);
        Task<Address> Create(Address address);        
        Task<Address?> Update(Address address);
        Task<Address?> Delete(int id);
    }
}
