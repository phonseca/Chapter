using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Chapter.Aws.ParameterStore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParameterController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ParameterController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string parameterName)
        {
            var parameterValue = configuration[parameterName];

            return Ok(parameterValue);
        }
    }
}
