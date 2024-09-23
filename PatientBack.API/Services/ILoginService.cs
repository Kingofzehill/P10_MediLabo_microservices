namespace PatientBack.API.Services
{
    public interface ILoginService
    {
        Task<string> Login(string username, string password);
    }
}
