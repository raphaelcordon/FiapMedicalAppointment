using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces.Services;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [Authorize]
    [HttpPost("schedule")]
    public async Task<IActionResult> ScheduleAppointment([FromBody] ScheduleAppointmentDto scheduleDto)
    {
        try
        {
            var appointment = await _appointmentService.ScheduleAppointmentAsync(scheduleDto);
            return Ok(appointment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetAppointmentsForDoctor(Guid doctorId)
    {
        try
        {
            var appointments = await _appointmentService.GetAppointmentsForDoctor(doctorId);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetAppointmentsForPatient(Guid patientId)
    {
        try
        {
            var appointments = await _appointmentService.GetAppointmentsForPatient(patientId);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("update/{appointmentId}")]
    public async Task<IActionResult> UpdateAppointmentStatus(Guid appointmentId, [FromBody] UpdateAppointmentDto updateDto)
    {
        try
        {
            var updatedAppointment = await _appointmentService.UpdateAppointmentStatus(appointmentId, updateDto.NewStatus);
            return Ok(updatedAppointment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("cancel/{appointmentId}")]
    public async Task<IActionResult> CancelAppointment(Guid appointmentId)
    {
        try
        {
            await _appointmentService.CancelAppointment(appointmentId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}