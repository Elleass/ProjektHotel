using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Domain.Entities;

namespace Hotel.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ========== Room Mappings ==========

        CreateMap<CreateRoomDto, Room>()
            .ConstructUsing(dto => new Room(dto.RoomNumber, dto.Type, dto.Price));

        CreateMap<Room, RoomResponseDto>();


        // ========== Guest Mappings ==========

        CreateMap<CreateGuestDto, Guest>()
            .ConstructUsing(dto => new Guest(
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.PhoneNumber));

        CreateMap<Guest, GuestResponseDto>();


        // ========== Reservation Mappings ==========

        CreateMap<CreateReservationDto, Reservation>()
            .ConstructUsing(dto => new Reservation(
                dto.GuestId,
                dto.RoomId,
                dto.StartDate,
                dto.EndDate));

        CreateMap<Reservation, ReservationResponseDto>()
            .ForMember(dest => dest.GuestName,
                opt => opt.MapFrom(src => $"{src.Guest.FirstName} {src.Guest.LastName}"))
            .ForMember(dest => dest.RoomNumber,
                opt => opt.MapFrom(src => src.Room.RoomNumber));
    }
}