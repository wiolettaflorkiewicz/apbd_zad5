using Microsoft.EntityFrameworkCore;
using Zadanie7.Models;
using Zadanie7.Interfaces;

namespace Zadanie7.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly S25007Context _context;

        public ClientRepository(S25007Context context)
        {
            _context = context;
        }

        public async Task<int> DeleteClientAsync(int id)
        {
            return await _context.Clients
                .Where(client => client.IdClient == id)
                .ExecuteDeleteAsync();
        }

        public async Task<int> GetClientTripsCountAsync(int id)
        {
            return await _context.ClientTrips
                .Where(clientTrip => clientTrip.IdClient == id)
                .CountAsync();
        }

        public async Task<bool> ClientWithPeselExistsAsync(string pesel)
        {
            return await _context.Clients.AnyAsync(client => client.Pesel == pesel);
        }

        public async Task<int> GetLastClientIdAsync()
        {
            return await _context.Clients
                .OrderByDescending(client => client.IdClient)
                .Select(client => client.IdClient)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddClientAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> IsClientAssociatedWithTripAsync(int idClient, int idTrip)
        {
            return await _context.ClientTrips
                .AnyAsync(clientTrip => clientTrip.IdClient == idClient && clientTrip.IdTrip == idTrip);
        }

        public async Task<int> GetClientIdByPeselAsync(string pesel)
        {
            return await _context.Clients
                .Where(client => client.Pesel == pesel)
                .Select(client => client.IdClient)
                .FirstOrDefaultAsync();
        }
    }
}
