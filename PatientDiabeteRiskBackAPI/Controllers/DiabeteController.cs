using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientDiabeteRiskBackAPI.Services;

namespace PatientDiabeteRiskBackAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DiabeteController : ControllerBase
    {
        private readonly DiabeteService _diabeteService;

        public DiabeteController(DiabeteService diabeteService)
        {
            _diabeteService = diabeteService;
        }
    }
}
