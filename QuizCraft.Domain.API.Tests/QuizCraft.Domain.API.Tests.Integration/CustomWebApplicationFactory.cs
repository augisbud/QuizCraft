using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizCraft.Domain.API.APIClients;
using QuizCraft.Domain.API.Data;

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
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<QuizzesDbContext>));

            if(dbContextDescriptor != null)
                services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            if(dbConnectionDescriptor != null)
                services.Remove(dbConnectionDescriptor);

            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<QuizzesDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });

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