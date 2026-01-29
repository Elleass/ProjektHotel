# ProjektHotel
docker-compose down -v 
docker-compose up --build

# Tworzenie pokoju
{
  "roomNumber": 101,
  "type": "Apartament",
  "price": 100.00
}

# Tworzenie gościa
{
  "firstName": "Jan",
  "lastName": "Nowak",
  "email": "jan.nowak@email.com",
  "phoneNumber": "500123456"
}

# Tworzenie rezerwacji
{
  "roomId": 1,
  "guestId": 1,
  "startDate": "2026-07-01T14:00:00",
  "endDate": "2026-07-10T15:00:00"
}

# Test Walidacji
{
  "roomNumber": 102,
  "type": "Standard",
  "price": -100.00
}

# Test Soft Delete
1
docker exec -it hotel-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P StrongPassword123! -C -Q "SELECT Id, IsDeleted FROM HotelDb.dbo.Reservations"

# Wyświetl rezerwacje  
docker exec -it hotel-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P StrongPassword123! -C -Q "SELECT Id, IsDeleted, GuestId, RoomId FROM HotelDb.dbo.Reservations"


# Testy obejmują:

## RoomsController:
- POST /api/rooms	- Tworzenie pokoju
- GET /api/rooms/{id}	- Pobieranie istniejącego i nieistniejącego pokoju
- GET /api/rooms	- Pobieranie wszystkich pokoi (również pusta lista)
- DELETE /api/rooms/{id}	- Usuwanie istniejącego i nieistniejącego pokoju
## GuestsController:
- POST /api/guests	- Tworzenie gościa
- GET /api/guests/{email}	- Pobieranie gościa po emailu (istniejący/nieistniejący)
## ReservationsController:
- POST /api/reservations	- Tworzenie rezerwacji (sukces, pokój nie istnieje, pokój zajęty)
- GET /api/reservations/{id}	- Pobieranie rezerwacji (istniejąca/nieistniejąca)
- GET /api/reservations/guest/{guestId}	- Pobieranie rezerwacji gościa
- DELETE /api/reservations/{id}	- Anulowanie rezerwacji (sukces, nie istnieje, błąd)
