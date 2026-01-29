using FluentAssertions;
using Hotel.API.Controllers;
using Hotel.Application.DTOs;
using Hotel.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hotel.API.Tests.Controllers;

public class GuestsControllerTests
{
    private readonly Mock<IGuestService> _guestServiceMock;
    private readonly GuestsController _controller;

    public GuestsControllerTests()
    {
        _guestServiceMock = new Mock<IGuestService>();
        _controller = new GuestsController(_guestServiceMock.Object);
    }

    #region Create Tests

    [Fact]
    public async Task Create_ValidDto_ReturnsOkWithGuest()
    {
        // Arrange
        var createDto = new CreateGuestDto
        {
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "jan.kowalski@email.com",
            PhoneNumber = "500123456"
        };

        var responseDto = new GuestResponseDto
        {
            Id = 1,
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "jan.kowalski@email.com",
            PhoneNumber = "500123456"
        };

        _guestServiceMock
            .Setup(x => x.CreateGuestAsync(createDto))
            .ReturnsAsync(responseDto);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(responseDto);
    }

    #endregion

    #region GetByEmail Tests

    [Fact]
    public async Task GetByEmail_ExistingEmail_ReturnsOkWithGuest()
    {
        // Arrange
        var email = "jan.kowalski@email.com";
        var responseDto = new GuestResponseDto
        {
            Id = 1,
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = email,
            PhoneNumber = "500123456"
        };

        _guestServiceMock
            .Setup(x => x.GetGuestByEmailAsync(email))
            .ReturnsAsync(responseDto);

        // Act
        var result = await _controller.GetByEmail(email);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(responseDto);
    }

    [Fact]
    public async Task GetByEmail_NonExistingEmail_ReturnsNotFound()
    {
        // Arrange
        var email = "nieistniejacy@email.com";
        _guestServiceMock
            .Setup(x => x.GetGuestByEmailAsync(email))
            .ReturnsAsync((GuestResponseDto?)null);

        // Act
        var result = await _controller.GetByEmail(email);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    #endregion
}