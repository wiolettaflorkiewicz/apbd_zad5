using Zadanie7.Models;
using Zadanie7.DTO;


namespace Zadanie7.Interfaces;

public interface ITripsService
{
    Task<IEnumerable<TripDto>> GetTripsAsync();
    Task<int> AssignClientToTripAsync(ClientAssignmentDTO clientAssignmentDTO);
}