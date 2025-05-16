using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using webApiC.DTOs;
using webApiC.Services;

namespace webApiC.Controllers;

[ApiController]
[Route("api/[controller]")]

public class VisitController: ControllerBase
{
    private readonly IVisitService _visitService;
    
    public VisitController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpGet("visits/{id}")]
    public async Task<IActionResult> GetDeliveriesForIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _visitService.GetVisitById(id, cancellationToken);
        return Ok(response);
    }
}