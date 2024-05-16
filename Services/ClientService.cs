using Zadanie7.Interfaces;
using Zadanie7.DTO;

namespace Zadanie7.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<int> DeleteClientAsync(int id)
        {
            var hasTrips = await _clientRepository.GetClientTripsCountAsync(id) > 0;
            if (hasTrips) throw new Exception("Cannot delete client with trips.");
            return await _clientRepository.DeleteClientAsync(id);
        }
    }
}

