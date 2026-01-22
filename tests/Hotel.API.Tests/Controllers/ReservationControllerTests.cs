using FluentAssertions;
using Hotel.API.Controllers;
using Hotel.Application.DTOs;
using Hotel.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hotel.API.Tests.Controllers;

public class ReservationsControllerTests
{
    private readonly Mock<IReservationService> _reservationServiceMock;
    private readonly ReservationsController _controller;

    public ReservationsControllerTests()
    {
        _reservationServiceMock = new Mock<IReservationService>();
        _controller = new ReservationsController(_reservationServiceMock.Object);
    }

    #region CreateReservation Tests

    [Fact]
    public async Task CreateReservation_ValidDto_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateReservationDto
        {
            GuestId = 1,
            RoomId = 1,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(5)
        };

        var responseDto = new ReservationResponseDto
        {
            Id = 1,
            GuestId = 1,
            GuestName = "Jan Kowalski",
            RoomId = 1,
            RoomNumber = 101,
            StartDate = createDto.StartDate,
            EndDate = createDto.EndDate,
            CreatedAt = DateTime.Now
        };

        _reservationServiceMock
            .Setup(x => x.CreateReservationAsync(createDto))
            .ReturnsAsync(responseDto);

        // Act
        var result = await _controller.CreateReservation(createDto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(_controller.GetReservation));
        createdResult.Value.Should().BeEquivalentTo(responseDto);
    }

    [Fact]
    public async Task CreateReservation_RoomNotFound_ReturnsNotFound()
    {
        // Arrange
        var createDto = new CreateReservationDto
        {
            GuestId = 1,
            RoomId = 999,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(5)
        };

        _reservationServiceMock
            .Setup(x => x.CreateReservationAsync(createDto))
            .ThrowsAsync(new KeyNotFoundException("Room not found"));

        // Act
        var result = await _controller.CreateReservation(createDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task CreateReservation_RoomNotAvailable_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateReservationDto
        {
            GuestId = 1,
            RoomId = 1,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(5)
        };

        _reservationServiceMock
            .Setup(x => x.CreateReservationAsync(createDto))
            .ThrowsAsync(new InvalidOperationException("Pokój jest już zarezerwowany w tym terminie"));

        // Act
        var result = await _controller.CreateReservation(createDto);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    #endregion

    #region GetReservation Tests

    [Fact]
    public async Task GetReservation_ExistingId_ReturnsOkWithReservation()
    {
        // Arrange
        var reservationId = 1;
        var responseDto = new ReservationResponseDto
        {
            Id = reservationId,
            GuestId = 1,
            GuestName = "Jan Kowalski",
            RoomId = 1,
            RoomNumber = 101,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(5),
            CreatedAt = DateTime.Now
        };

        _reservationServiceMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(responseDto);

        // Act
        var result = await _controller.GetReservation(reservationId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(responseDto);
    }

    [Fact]
    public async Task GetReservation_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var reservationId = 999;
        _reservationServiceMock
            .Setup(x => x.GetReservationByIdAsync(reservationId))
            .ReturnsAsync((ReservationResponseDto?)null);

        // Act
        var result = await _controller.GetReservation(reservationId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    #endregion

    #region GetReservationsByGuest Tests

    [Fact]
    public async Task GetReservationsByGuest_ExistingGuestId_ReturnsOkWithReservations()
    {
        // Arrange
        var guestId = 1;
        var reservations = new List<ReservationResponseDto>
        {
            new()
            {
                Id = 1,
                GuestId = guestId,
                GuestName = "Jan Kowalski",
                RoomId = 1,
                RoomNumber = 101,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(5),
                CreatedAt = DateTime.Now
            },
            new()
            {
                Id = 2,
                GuestId = guestId,
                GuestName = "Jan Kowalski",
                RoomId = 2,
                RoomNumber = 102,
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime. Now.AddDays(15),
                CreatedAt = DateTime. Now
            }
        };

        _reservationServiceMock
            .Setup(x => x.GetReservationsByGuestIdAsync(guestId))
            .ReturnsAsync(reservations);

        // Act
        var result = await _controller.GetReservationsByGuest(guestId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(reservations);
    }

    [Fact]
    public async Task GetReservationsByGuest_NoReservations_ReturnsOkWithEmptyList()
    {
        // Arrange
        var guestId = 1;
        _reservationServiceMock
            .Setup(x => x.GetReservationsByGuestIdAsync(guestId))
            .ReturnsAsync(new List<ReservationResponseDto>());

        // Act
        var result = await _controller.GetReservationsByGuest(guestId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var reservations = okResult.Value.Should().BeAssignableTo<List<ReservationResponseDto>>().Subject;
        reservations.Should().BeEmpty();
    }

    #endregion

    #region CancelReservation Tests

    [Fact]
    public async Task CancelReservation_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var reservationId = 1;
        _reservationServiceMock
            .Setup(x => x.CancelReservationAsync(reservationId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CancelReservation(reservationId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task CancelReservation_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var reservationId = 999;
        _reservationServiceMock
            .Setup(x => x.CancelReservationAsync(reservationId))
            .ThrowsAsync(new KeyNotFoundException($"Reservation with ID {reservationId} not found"));

        // Act
        var result = await _controller.CancelReservation(reservationId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task CancelReservation_InvalidOperation_ReturnsBadRequest()
    {
        // Arrange
        var reservationId = 1;
        _reservationServiceMock
            .Setup(x => x.CancelReservationAsync(reservationId))
            .ThrowsAsync(new Exception("Nie można anulować rezerwacji"));

        // Act
        var result = await _controller.CancelReservation(reservationId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    #endregion
}