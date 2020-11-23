using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GildedRoseStore.IntegrationTests
{
    public class HomeControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public HomeControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsToIndexViewWithItems()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Home/Index");

            // Act
            var response = await _client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Trouser", responseString);
            Assert.Contains("T-Shirt", responseString);
            Assert.Contains("Hoodie", responseString);
        }

        [Fact]
        public async Task PurchaseItem_WhenPostExecuted_ReturnToIndexViewWithUpdatedItems()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Home/PurchaseItem");
            var formModel = new Dictionary<string, string>
            {
                {"id","1" }
            };
            postRequest.Content = new FormUrlEncodedContent(formModel);

            // Act
            var response = await _client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Stock: 4", responseString);
        }
    }
}
