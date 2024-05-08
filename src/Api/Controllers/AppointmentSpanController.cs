using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AppointmentSpanController : ControllerBase
{
    private readonly IBaseRepository<AppointmentSpan> _appointmentSpanRepository;

    public AppointmentSpanController(IBaseRepository<AppointmentSpan> appointmentSpanRepository)
    {
        _appointmentSpanRepository = appointmentSpanRepository;
    }

    // GET: api/AppointmentSpan
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentSpan>>> GetAllAppointmentSpans()
    {
        var spans = await _appointmentSpanRepository.ListAsync();
        return Ok(spans);
    }
}