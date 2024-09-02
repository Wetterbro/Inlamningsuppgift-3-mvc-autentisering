using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MVC3ETTest;

[TestFixture]
public class AuthorizationIntegrationTest
{
    WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    
    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureKestrel(options =>
                {
                    options.ListenLocalhost(5001, listenOptions =>
                    {
                        listenOptions.UseHttps();
                    } );
                });
            });
    
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            BaseAddress = new Uri("https://localhost:5001/"),
            AllowAutoRedirect = false
        });
    }

    [TestCase("/Product/Create")]
    [TestCase("/Category/Create")]
    public async Task Get_SecurePage_RedirectToLogin(string url)
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        
        // Act
        var response = await _client.SendAsync(request);
        
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
        StringAssert.StartsWith("https://localhost:5001/Identity/Account/Login", response.Headers.Location.ToString());
    }
}