using EngageGov.Application.DTOs.Citizens;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace EngageGov.IntegrationTests.Controllers;

/// <summary>
/// Integration tests for Citizens API
/// Tests the full request/response pipeline
/// </summary>
public class CitizensControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CitizensControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/citizens");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreated()
    {
        // Arrange
        var newCitizen = new CreateCitizenDto
        {
            FullName = "Integration Test User",
            Email = $"test-{Guid.NewGuid()}@example.com",
            DocumentNumber = $"DOC-{Guid.NewGuid().ToString()[..10]}",
            PhoneNumber = "+1234567890"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/citizens", newCitizen);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var citizen = await response.Content.ReadFromJsonAsync<CitizenDto>();
        Assert.NotNull(citizen);
        Assert.Equal(newCitizen.FullName, citizen.FullName);
        Assert.Equal(newCitizen.Email.ToLowerInvariant(), citizen.Email);
    }

    [Fact]
    public async Task Create_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var invalidCitizen = new CreateCitizenDto
        {
            FullName = "Test User",
            Email = "invalid-email",
            DocumentNumber = "12345678901"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/citizens", invalidCitizen);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/citizens/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
