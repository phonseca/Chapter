using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System;
using System.IO;
using Amazon.Runtime;

namespace Chapter.Aws.ParameterStore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecretManagerController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public SecretManagerController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string secretName)
        {
            
            return Ok(GetSecret(secretName));
        }


        public string GetSecret(string name)
        {
            string secretName = $"/dev/chapter/{name}";
            string region = "sa-east-1";
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();

            var credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"), Environment.GetEnvironmentVariable("AWS_SECRET_KEY"));

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(credentials, RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

            GetSecretValueResponse response = null;

            try
            {
                response = client.GetSecretValueAsync(request).Result;
            }
            catch (Exception e)
            {
                throw;
            }

            if (response.SecretString != null)
            {
                secret = response.SecretString;
            }
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }

            return secret;
            // Your code goes here.
        }
    }
}
