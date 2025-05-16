using webApiC.DTOs;
using webApiC.Models;

namespace webApiC.Repositories;

public interface IVisitRepository
{
    public Task<DTO> GetVisitById(int id, CancellationToken cancellationToken);
    public Task<bool> DoesVisitExistAsync(int id, CancellationToken cancellationToken);
    public Task<bool> DoesClientExistAsync(int id, CancellationToken cancellationToken);
    public Task<bool> DoesMechanicExistAsync(int id, CancellationToken cancellationToken);
    public Task<bool> DoesServiceNameExistAsync(String name, CancellationToken cancellationToken);
    
    public Task<int> GetDriverIdForLicenceNumber(string licenceNumber, CancellationToken cancellationToken);
    
    // public Task<Mechanic> GetProductByProductName(string productName, CancellationToken cancellationToken);
    
    // public Task<int> SaveDelivery(DeliverySaveDTO deliverySave, CancellationToken cancellationToken);
}