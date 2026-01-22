using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Domain.Entities;
using Hotel.Domain.Repositories;

namespace Hotel.Application.Services;

public class GuestService : IGuestService
{
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GuestService(IGuestRepository guestRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GuestResponseDto> CreateGuestAsync(CreateGuestDto dto)
    {
        var guest = _mapper.Map<Guest>(dto);
        
        await _guestRepository.AddAsync(guest);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<GuestResponseDto>(guest);
    }

    public async Task<GuestResponseDto?> GetGuestByEmailAsync(string email)
    {
        var guest = await _guestRepository.GetByEmailAsync(email);
        return guest == null ? null : _mapper.Map<GuestResponseDto>(guest);
    }
}