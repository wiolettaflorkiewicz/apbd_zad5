using Zadanie7.Models;
using Zadanie7.DTO;
using Zadanie7.Interfaces;

namespace Zadanie7.Services
{
	public class TripsService : ITripsService
	{
		private readonly ITripsRepository _tripsRepository;
		private readonly IClientRepository _clientRepository;

		public TripsService(ITripsRepository tripsRepository, IClientRepository clientRepository)
		{
			_tripsRepository = tripsRepository;
			_clientRepository = clientRepository;
		}

		public async Task<IEnumerable<TripDto>> GetTripsAsync()
		{
			return (await _tripsRepository.GetTripsAsync())
				.Select(trip => new TripDto
				{
					Name = trip.Name,
					Description = trip.Description,
					DateFrom = DateOnly.FromDateTime(trip.DateFrom),
					DateTo = DateOnly.FromDateTime(trip.DateTo),
					MaxPeople = trip.MaxPeople,
					Countries = trip.IdCountries.Select(country => new CountryDto
					{
						Name = country.Name
					})
						.ToList(),
					Clients = trip.ClientTrips.Select(clientTrip => new ClientDto
					{
						FirstName = clientTrip.IdClientNavigation.FirstName,
						LastName = clientTrip.IdClientNavigation.LastName
					})
						.ToList()
				});
		}

		public async Task<int> AssignClientToTripAsync(ClientAssignmentDTO clientAssignmentDTO)
		{
			var clientExists = await _clientRepository.ClientWithPeselExistsAsync(clientAssignmentDTO.Pesel);
			int idClient;

			if (!clientExists)
			{
				idClient = await _clientRepository.GetLastClientIdAsync() + 1;
				await _clientRepository.AddClientAsync(new Client
				{
					IdClient = idClient,
					FirstName = clientAssignmentDTO.FirstName,
					LastName = clientAssignmentDTO.LastName,
					Email = clientAssignmentDTO.Email,
					Telephone = clientAssignmentDTO.Telephone,
					Pesel = clientAssignmentDTO.Pesel
				});
			}
			else
			{
				idClient = await _clientRepository.GetClientIdByPeselAsync(clientAssignmentDTO.Pesel);
			}

			var isClientAssociatedWithTrip =
				await _clientRepository.IsClientAssociatedWithTripAsync(idClient, clientAssignmentDTO.IdTrip);
			if (isClientAssociatedWithTrip) throw new Exception("Client is already associated with this trip.");

			var tripExists = await _tripsRepository.TripExistsAsync(clientAssignmentDTO.IdTrip);
			if (!tripExists) throw new Exception("Trip does not exist.");

			return await _tripsRepository.AssignClientToTripAsync(new ClientTrip
			{
				IdClient = idClient,
				IdTrip = clientAssignmentDTO.IdTrip,
				PaymentDate = clientAssignmentDTO.PaymentDate?.ToDateTime(TimeOnly.MinValue),
				RegisteredAt = DateTime.Now
			});
		}
	}
}
