using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Chapter.Aws.ParameterStore.Api
{
    public class Program
    {
        public static AWSOptions AwsOptions { get; set; }

        public static void Main(string[] args)
        {
            AwsOptions = new AWSOptions();
            AwsOptions.Region = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_REGION"));
            AwsOptions.Credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"),
                   Environment.GetEnvironmentVariable("AWS_SECRET_KEY"));

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .AddSystemsManager(Environment.GetEnvironmentVariable("PARAMETER_STORE_PATH"), AwsOptions, TimeSpan.FromMinutes(1))
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
