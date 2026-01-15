using FluentValidation;
using FluentValidation.AspNetCore;
using Hotel.Application.DTOs;
using Hotel.Application.Services;
using Hotel.Domain.Repositories;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRoomDto>(); // Skanuje assembly Application


builder.Services.AddScoped<ConcurrencyTokenInterceptor>();

builder.Services.AddDbContext<HotelDbContext>((sp, options) =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));

    options.AddInterceptors(
        sp.GetRequiredService<ConcurrencyTokenInterceptor>());
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(CreateRoomDto).Assembly);

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();