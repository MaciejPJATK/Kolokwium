using webApiC.DTOs;
using webApiC.Exceptions;
using webApiC.Models;
using webApiC.Repositories;

namespace webApiC.Services;

public class VisitService: IVisitService
{
    private readonly IVisitRepository _visitRepository;
    public VisitService(IVisitRepository visitRepository)
    {
        _visitRepository = visitRepository;
    }

    public async Task<VisitDTO> GetVisitById(int id, CancellationToken cancellationToken)
    {
        if(id<0)
            throw new BadRequestException("id musi byc wieksze niz 0");
        
        return await _visitRepository.GetVisitById(id, cancellationToken);
    }

    // public async Task<int> AddNewDelivery(DeliveryDTORequest dto, CancellationToken cancellationToken)
    // {
    //     
    //     var delivery = new Delivery()
    //     {
    //         customer_id = dto.customerId,
    //         delivery_id = dto.deliveryId,
    //         driver_id = _visitRepository.GetDriverIdForLicenceNumber(dto.licence_number, cancellationToken).Result,
    //         date = DateTime.Now,
    //     };
    //      var product_list = new List<Product>();
    //      foreach (var product in dto.productDTO)
    //      {
    //          product_list.Add(_visitRepository.GetProductByProductName(product.name, cancellationToken).Result);
    //      }
    //      List<ProductDelivery> productDeliveries = new List<ProductDelivery>();
    //      foreach (var product in product_list)
    //      {
    //          productDeliveries.Add(new ProductDelivery()
    //          {
    //              product_id = product.product_id,
    //              amount = dto.productDTO.FindLast((productDto => productDto.name == product.name )).amount,
    //              delivery_id = delivery.delivery_id,
    //          });
    //      }
    //     return await _visitRepository.SaveDelivery(new DeliverySaveDTO()
    //     {
    //         delivery = delivery,
    //         products = product_list,
    //         productDeliveries = productDeliveries
    //     }, cancellationToken);
    // }
}