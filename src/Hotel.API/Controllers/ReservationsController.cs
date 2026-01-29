using Hotel.Application.DTOs;
using Hotel.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponseDto>> CreateReservation(
        [FromBody] CreateReservationDto dto)
    {
        try
        {
            var reservation = await _reservationService.CreateReservationAsync(dto);
            return CreatedAtAction(
                nameof(GetReservation),
                new { id = reservation.Id },
                reservation);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationResponseDto>> GetReservation(int id)
    {
        var reservation = await _reservationService.GetReservationByIdAsync(id);

        if (reservation == null)
            return NotFound(new { message = $"Reservation with ID {id} not found" });

        return Ok(reservation);
    }

    [HttpGet("guest/{guestId}")]
    public async Task<ActionResult<List<ReservationResponseDto>>> GetReservationsByGuest(int guestId)
    {
        var reservations = await _reservationService.GetReservationsByGuestIdAsync(guestId);
        return Ok(reservations);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelReservation(int id)
    {
        try
        {
            await _reservationService.CancelReservationAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}