using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediaBrowser.Model.Branding;
using Xunit;

namespace Jellyfin.Api.Tests
{
    public sealed class BrandingControllerTests : IClassFixture<JellyfinApplicationFactory>
    {
        private readonly JellyfinApplicationFactory _factory;

        public BrandingControllerTests(JellyfinApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetConfiguration_ReturnsCorrectResponse()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Branding/Configuration");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(MediaTypeNames.Application.Json, response.Content.Headers.ContentType?.MediaType);
            Assert.Equal(Encoding.UTF8.BodyName, response.Content.Headers.ContentType?.CharSet);
            var responseBody = await response.Content.ReadAsStreamAsync();
            _ = await JsonSerializer.DeserializeAsync<BrandingOptions>(responseBody);
        }

        [Theory]
        [InlineData("/Branding/Css")]
        [InlineData("/Branding/Css.css")]
        public async Task GetCss_ReturnsCorrectResponse(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("text/css", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal(Encoding.UTF8.BodyName, response.Content.Headers.ContentType?.CharSet);
        }
    }
}
