using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizCraft.Domain.API.APIClients;

namespace QuizCraft.Domain.API.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseConfiguration(
            new ConfigurationBuilder()
                .AddInMemoryCollection([
                    new KeyValuePair<string, string?>("GeminiAPIKey", "fake-api-key")
                ])
                .Build()
        );
        
        builder.ConfigureTestServices(services =>
        {
            var geminiAPIClientDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IGeminiAPIClient)
            );

            if(geminiAPIClientDescriptor != null)
                services.Remove(geminiAPIClientDescriptor);

            services.AddHttpClient<IGeminiAPIClient, GeminiAPIClient>(
                client => { client.BaseAddress = new Uri("http://localhost:8080/geminiAPI"); } 
            );
        });
    }
}