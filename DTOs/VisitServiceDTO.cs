namespace webApiC.DTOs;

public class VisitServiceDTO
{
    public String name { get; set; }
    public float serviceFee { get; set; }
    public List<VisitServiceDTO> VisitServices { get; set; }
}