using System.Net;
using Newtonsoft.Json;

namespace QuizCraft.Domain.API.Tests.Integration;

public class ControllerTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetQuizes_ReturnsExpected()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/quizes");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
        Assert.Equal(["Quiz 1", "Quiz 2"], data);
    }
}