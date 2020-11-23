using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GildedRoseStore.IntegrationTests
{
    public class AccountControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public AccountControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsAuthenticationForm()
        {
            // Arrange
            var response = await _client.GetAsync("/Account/Login");

            // Act
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("/Account/Login", response.RequestMessage.RequestUri.AbsolutePath);
            Assert.Contains("form id=\"account\"", responseString);
        }
    }
}
