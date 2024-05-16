using Microsoft.EntityFrameworkCore;
using Zadanie7.Models;
using Zadanie7.Interfaces;

namespace Zadanie7.Repositories
{
    public class TripsRepository : ITripsRepository
    {
        private readonly S25007Context _context;

        public TripsRepository(S25007Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetTripsAsync()
        {
            var result = await _context.Trips
                .Include(trip => trip.IdCountries)
                .Include(trip => trip.ClientTrips)
                .ThenInclude(clientTrip => clientTrip.IdClientNavigation)
                .OrderByDescending(trip => trip.DateFrom)
                .ToListAsync();
            if (result.Count == 0) throw new Exception("No trips found");
            return result;
        }

        public async Task<bool> TripExistsAsync(int id)
        {
            return await _context.Trips.AnyAsync(trip => trip.IdTrip == id);
        }

        public async Task<int> AssignClientToTripAsync(ClientTrip clientTrip)
        {
            await _context.ClientTrips.AddAsync(clientTrip);
            return await _context.SaveChangesAsync();
        }
    }
}
