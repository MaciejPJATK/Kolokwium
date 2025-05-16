using webApiC.DTOs;

namespace webApiC.Services;

public interface IVisitService
{
    public Task<DTO> GetVisitById(int id, CancellationToken cancellationToken);
    // public Task<int> AddNewDelivery(DeliveryDTORequest dtoRequest, CancellationToken cancellationToken);
}