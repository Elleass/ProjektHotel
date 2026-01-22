using Hotel.Application.DTOs;
using Hotel.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestsController : ControllerBase
{
    private readonly IGuestService _guestService;

    public GuestsController(IGuestService guestService)
    {
        _guestService = guestService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGuestDto dto)
    {
        var result = await _guestService.CreateGuestAsync(dto);
        return Ok(result);
    }

    [HttpGet("{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var guest = await _guestService.GetGuestByEmailAsync(email);
        if (guest == null) return NotFound();
        return Ok(guest);
    }
}