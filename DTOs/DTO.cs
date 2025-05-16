using webApiC.Models;

namespace webApiC.DTOs;

public class DTO
{
    public DateTime date { get; set; }
    public Client client { get; set; }
    public Mechanic mechanic { get; set; }
    public List<VisitServiceDTO> visitServices { get; set; }
}