using Zadanie7.Models;

namespace Zadanie7.Interfaces;

public interface ITripsRepository
{
    Task<IEnumerable<Trip>> GetTripsAsync();
    Task<bool> TripExistsAsync(int id);
    Task<int> AssignClientToTripAsync(ClientTrip clientTrip);
}