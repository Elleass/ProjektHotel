using FluentAssertions;
using Hotel.API.Controllers;
using Hotel.Application.DTOs;
using Hotel.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hotel.API.Tests.Controllers;

public class RoomsControllerTests
{
    private readonly Mock<IRoomService> _roomServiceMock;
    private readonly RoomsController _controller;

    public RoomsControllerTests()
    {
        _roomServiceMock = new Mock<IRoomService>();
        _controller = new RoomsController(_roomServiceMock.Object);
    }

    #region CreateRoom Tests

    [Fact]
    public async Task CreateRoom_ValidDto_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateRoomDto
        {
            RoomNumber = 101,
            Type = "Standard",
            Price = 150.00m
        };

        var responseDto = new RoomResponseDto
        {
            Id = 1,
            RoomNumber = 101,
            Type = "Standard",
            Price = 150.00m,
            IsAvailable = true
        };

        _roomServiceMock
            .Setup(x => x.AddRoomAsync(createDto))
            .ReturnsAsync(responseDto);

        // Act
        var result = await _controller.CreateRoom(createDto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(_controller.GetRoom));
        createdResult.Value.Should().BeEquivalentTo(responseDto);
    }

    #endregion

    #region GetRoom Tests

    [Fact]
    public async Task GetRoom_ExistingId_ReturnsOkWithRoom()
    {
        // Arrange
        var roomId = 1;
        var responseDto = new RoomResponseDto
        {
            Id = roomId,
            RoomNumber = 101,
            Type = "Standard",
            Price = 150.00m,
            IsAvailable = true
        };

        _roomServiceMock
            .Setup(x => x.GetRoomByIdAsync(roomId))
            .ReturnsAsync(responseDto);

        // Act
        var result = await _controller.GetRoom(roomId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(responseDto);
    }

    [Fact]
    public async Task GetRoom_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var roomId = 999;
        _roomServiceMock
            .Setup(x => x.GetRoomByIdAsync(roomId))
            .ReturnsAsync((RoomResponseDto?)null);

        // Act
        var result = await _controller.GetRoom(roomId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    #endregion

    #region GetAllRooms Tests

    [Fact]
    public async Task GetAllRooms_ReturnsOkWithRoomsList()
    {
        // Arrange
        var rooms = new List<RoomResponseDto>
        {
            new() { Id = 1, RoomNumber = 101, Type = "Standard", Price = 150.00m, IsAvailable = true },
            new() { Id = 2, RoomNumber = 102, Type = "Deluxe", Price = 250.00m, IsAvailable = false }
        };

        _roomServiceMock
            .Setup(x => x.GetAllRoomsAsync())
            .ReturnsAsync(rooms);

        // Act
        var result = await _controller.GetAllRooms();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(rooms);
    }

    [Fact]
    public async Task GetAllRooms_EmptyList_ReturnsOkWithEmptyList()
    {
        // Arrange
        _roomServiceMock
            .Setup(x => x.GetAllRoomsAsync())
            .ReturnsAsync(new List<RoomResponseDto>());

        // Act
        var result = await _controller.GetAllRooms();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var rooms = okResult.Value.Should().BeAssignableTo<List<RoomResponseDto>>().Subject;
        rooms.Should().BeEmpty();
    }

    #endregion

    #region DeleteRoom Tests

    [Fact]
    public async Task DeleteRoom_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var roomId = 1;
        _roomServiceMock
            .Setup(x => x.DeleteRoomAsync(roomId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteRoom(roomId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteRoom_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var roomId = 999;
        _roomServiceMock
            .Setup(x => x.DeleteRoomAsync(roomId))
            .ThrowsAsync(new KeyNotFoundException($"Room with ID {roomId} not found"));

        // Act
        var result = await _controller.DeleteRoom(roomId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    #endregion
}